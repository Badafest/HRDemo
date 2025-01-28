using HRDemoAPI.Data;
using HRDemoAPI.Filters;
using HRDemoApp.Utilities;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace HRDemoAPI.Controllers
{
    [HRDemoAuthorize]
    public class LeaveStatusController : ApiController
    {
        private readonly HRDemoApiDbContainer _hRDemoAPIDb;
        public LeaveStatusController(HRDemoApiDbContainer hRDemoAPIDb)
        {
            _hRDemoAPIDb = hRDemoAPIDb;
        }

        // POST api/LeaveStatus/5
        public HttpResponseMessage Post(int id, bool approve = true)
        {
            Leave leave = _hRDemoAPIDb.Leaves.Include("Employee").Where(l => l.LeaveID == id).FirstOrDefault();
            if (leave == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(leave.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if (leave.Status == LeaveStatus.Approved && approve)
            {
                return HttpUtilities.CreateResponseMessage($"Leave is already approved", System.Net.HttpStatusCode.BadRequest);
            }
            if (leave.Status == LeaveStatus.Rejected && !approve)
            {
                return HttpUtilities.CreateResponseMessage($"Leave is already rejected", System.Net.HttpStatusCode.BadRequest);
            }
            leave.Status = approve ? LeaveStatus.Approved : LeaveStatus.Rejected;

            _hRDemoAPIDb.SaveChanges();
            return leave.CreateResponseMessage();
        }
    }
}
