using HRDemoAPI.DataCore.Models;
using HRDemoAPICore.Filters;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HRDemoAPICore.Controllers
{
    [ServiceFilter<HRDemoAuthorizeFilter>]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeStatusController(HRDemoApiContext hrDemoApiDb) : ControllerBase
    {
        private readonly HRDemoApiContext _hRDemoAPIDb = hrDemoApiDb;

        [HttpPost("{id}")]

        // POST api/EmployeeStatus/5
        public ObjectResult Post(int id, bool hire = true)
        {
            Employee? employee = _hRDemoAPIDb.Employees.Find(id);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if (employee.Status == EmployeeStatus.Active && hire)
            {
                return HttpUtilities.CreateResponseMessage($"Employee is already hired on {employee.DateOfHire}", System.Net.HttpStatusCode.BadRequest);
            }
            if (employee.Status == EmployeeStatus.Terminated && !hire)
            {
                return HttpUtilities.CreateResponseMessage($"Employee is already fired", System.Net.HttpStatusCode.BadRequest);
            }
            employee.Status = hire ? EmployeeStatus.Active : EmployeeStatus.Terminated;
            if (hire)
            {
                employee.DateOfHire = DateTimeOffset.UtcNow;
            }
            _hRDemoAPIDb.SaveChanges();
            return employee.CreateResponseMessage();
        }
    }
}
