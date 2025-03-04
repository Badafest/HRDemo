using HRDemoAPI.DataCore.Models;
using HRDemoAPICore.Filters;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDemoAPICore.Controllers
{
    [ServiceFilter<HRDemoAuthorizeFilter>]
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollStatusController(HRDemoApiContext hRDemoAPIDb) : ControllerBase
    {
        private readonly HRDemoApiContext _hRDemoAPIDb = hRDemoAPIDb;

        [HttpPost("{id}")]

        // POST api/PayrollStatus/5
        public ObjectResult Post(int id, bool processed = true)
        {
            Payroll? payroll = _hRDemoAPIDb.Payrolls.Include("Employee").Where(l => l.PayrollID == id).FirstOrDefault();
            if (payroll == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, payroll.Employee.DepartmentID);
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
