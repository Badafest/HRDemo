using HRDemoAPI.DataCore.Models;
using HRDemoAPICore.Filters;
using HRDemoAPICore.Models;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDemoAPICore.Controllers
{
    [ServiceFilter<HRDemoAuthorizeFilter>]
    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController(HRDemoApiContext hrDemoApiDb) : ControllerBase
    {
        private readonly HRDemoApiContext _hRDemoAPIDb = hrDemoApiDb;

        [HttpGet]
        // GET api/leaves
        public IEnumerable<LeaveResponse> Get(int count = default, int page = default, int employeeId = 0, string? type = null, string? startDate = null, string? endDate = null)
        {
            var managedDepartments = HttpUtilities.GetManagedDepartments(HttpContext);

            var isStartDateParsed = DateTimeOffset.TryParse(startDate, out var startDateTime);
            var isEndDateParsed = DateTimeOffset.TryParse(endDate, out var endDateTime);

            return _hRDemoAPIDb.Leaves
                .Where(l => employeeId == default || (l.EmployeeID == employeeId))
                .Where(l => !isStartDateParsed || l.StartDate >= startDateTime)
                .Where(l => !isEndDateParsed || l.EndDate <= endDateTime)
                .Where(l => string.IsNullOrEmpty(type) || l.Type.ToString().Equals(type, StringComparison.CurrentCultureIgnoreCase))
                .Include("Employee")
                .Where(l => managedDepartments.Count == 0 || (l.Employee != null && l.Employee.DepartmentID != null && managedDepartments.Contains((int)l.Employee.DepartmentID)))
                .OrderBy(a => a.LeaveID)
                .Paginate(count, page, HttpContext)
                .AsNoTracking()
                .ToList()
                .Select(a => a.MapQueryResult());
        }

        [HttpGet("{id}")]

        // GET api/leaves/5
        public ObjectResult Get(int id)
        {
            var managedDepartments = HttpUtilities.GetManagedDepartments(HttpContext);

            LeaveResponse? leave = _hRDemoAPIDb.Leaves
                .Where(e => e.LeaveID == id)
                .Include("Employee")
                .Where(l => managedDepartments.Count == 0 || (l.Employee != null && l.Employee.DepartmentID != null && managedDepartments.Contains((int)l.Employee.DepartmentID)))
                .AsNoTracking()
                .ToList()
                .Select(e => e.MapQueryResult())
                .FirstOrDefault();
            if (leave == null || leave.LeaveID != id)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            return leave.CreateResponseMessage();
        }

        [HttpPost]

        // POST api/leaves
        public ObjectResult Post([FromBody] LeaveRequest leaveRequest)
        {
            var employee = _hRDemoAPIDb.Employees.Find(leaveRequest.EmployeeID);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage($"Employee not found for given ID {leaveRequest.EmployeeID}", System.Net.HttpStatusCode.BadRequest);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Leave newLeave = leaveRequest.MapPostRequest();
            Leave savedLeave = _hRDemoAPIDb.Leaves.Add(newLeave).Entity;
            _hRDemoAPIDb.SaveChanges();
            return savedLeave.CreateResponseMessage();
        }

        [HttpPut("{id}")]

        // PUT api/leaves/5
        public ObjectResult Put(int id, [FromBody] LeaveRequest leaveRequest)
        {
            Leave? leave = _hRDemoAPIDb.Leaves
                .Include("Employee")
                .Where(a => a.LeaveID == id)
                .FirstOrDefault();
            if (leave == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, leave.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }

            Leave newLeave = leaveRequest.MapPutRequest(id);
            leave.Type = newLeave.Type;
            leave.Reason = newLeave.Reason;
            leave.StartDate = newLeave.StartDate;
            leave.EndDate = newLeave.EndDate;
            _hRDemoAPIDb.SaveChanges();
            return leave.CreateResponseMessage();
        }

        [HttpPatch("{id}")]

        // PATCH api/Leaves/5
        public ObjectResult Patch(int id, [FromBody] LeaveRequest leaveRequest)
        {
            Leave? leave = _hRDemoAPIDb.Leaves
                .Include("Employee")
                .Where(a => a.LeaveID == id)
                .FirstOrDefault();

            if (leave == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, leave.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if (leaveRequest.Reason != null)
            {
                leave.Reason = leaveRequest.Reason;
            }
            if (leaveRequest.StartDate > DateTimeOffset.MinValue)
            {
                leave.StartDate = leaveRequest.StartDate;
            }
            if (leaveRequest.EndDate > DateTimeOffset.MinValue)
            {
                leave.EndDate = leaveRequest.EndDate;
            }
            _hRDemoAPIDb.SaveChanges();
            return leave.CreateResponseMessage();
        }

        [HttpDelete("{id}")]

        // DELETE api/leaves/5
        public ObjectResult Delete(int id)
        {
            Leave? leave = _hRDemoAPIDb.Leaves
                .Include("Employee")
                .Where(a => a.LeaveID == id)
                .FirstOrDefault();

            if (leave == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, leave.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            _hRDemoAPIDb.Leaves.Remove(leave);
            _hRDemoAPIDb.SaveChanges();
            return HttpUtilities.CreateResponseMessage(null);
        }
    }
}
