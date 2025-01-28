using HRDemoAPI.Data;
using HRDemoAPI.Filters;
using HRDemoApp.Utilities;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace HRDemoAPI.Controllers
{
    [HRDemoAuthorize]
    public class PayrollStatusController : ApiController
    {
        private readonly HRDemoApiDbContainer _hRDemoAPIDb;
        public PayrollStatusController(HRDemoApiDbContainer hRDemoAPIDb)
        {
            _hRDemoAPIDb = hRDemoAPIDb;
        }

        // POST api/PayrollStatus/5
        public HttpResponseMessage Post(int id, bool processed = true)
        {
            Payroll payroll = _hRDemoAPIDb.Payrolls.Include("Employee").Where(l => l.PayrollID == id).FirstOrDefault();
            if (payroll == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(payroll.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if (payroll.Status == PayrollStatus.Processed && processed)
            {
                return HttpUtilities.CreateResponseMessage($"Payroll is already processed", System.Net.HttpStatusCode.BadRequest);
            }
            if (payroll.Status == PayrollStatus.Failed && !processed)
            {
                return HttpUtilities.CreateResponseMessage($"Payroll is already failed", System.Net.HttpStatusCode.BadRequest);
            }
            payroll.Status = processed ? PayrollStatus.Processed : PayrollStatus.Failed;

            _hRDemoAPIDb.SaveChanges();
            return payroll.CreateResponseMessage();
        }
    }
}
