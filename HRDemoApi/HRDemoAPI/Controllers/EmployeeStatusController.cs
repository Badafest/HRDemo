using HRDemoAPI.Data;
using HRDemoAPI.Filters;
using HRDemoAPI.Utilities;
using System.Net.Http;
using System.Web.Http;

namespace HRDemoAPI.Controllers
{
    [HRDemoAuthorize]
    public class EmployeeStatusController : ApiController
    {
        private readonly HRDemoApiDbContainer _hRDemoAPIDb;
        public EmployeeStatusController(HRDemoApiDbContainer hRDemoAPIDb)
        {
            _hRDemoAPIDb = hRDemoAPIDb;
        }

        // POST api/EmployeeStatus/5
        public HttpResponseMessage Post(int id, bool hire = true)
        {
            Employee employee = _hRDemoAPIDb.Employees.Find(id);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(employee.DepartmentID);
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
                employee.DateOfHire = System.DateTimeOffset.UtcNow;
            }
            _hRDemoAPIDb.SaveChanges();
            return employee.CreateResponseMessage();
        }
    }
}
