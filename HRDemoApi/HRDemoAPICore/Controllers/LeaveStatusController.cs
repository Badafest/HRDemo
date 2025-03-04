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
    public class LeaveStatusController(HRDemoApiContext hrDemoApiDb) : ControllerBase
    {
        private readonly HRDemoApiContext _hRDemoAPIDb = hrDemoApiDb;

        [HttpPost("{id}")]

        // POST api/LeaveStatus/5
        public ObjectResult Post(int id, bool approve = true)
        {
            Leave? leave = _hRDemoAPIDb.Leaves.Include("Employee").Where(l => l.LeaveID == id).FirstOrDefault();
            if (leave == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, leave.Employee.DepartmentID);
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
