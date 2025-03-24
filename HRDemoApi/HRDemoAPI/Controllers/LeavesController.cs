using HRDemoAPI.Data;
using HRDemoAPI.Filters;
using HRDemoAPI.Models;
using HRDemoAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace HRDemoAPI.Controllers
{
    [HRDemoAuthorize]
    public class LeavesController : ApiController
    {
        private readonly HRDemoApiDbContainer _hRDemoAPIDb;
        public LeavesController(HRDemoApiDbContainer hRDemoAPIDb)
        {
            _hRDemoAPIDb = hRDemoAPIDb;
        }
        // GET api/leaves
        public IEnumerable<LeaveResponse> Get(int count = default, int page = default, int employeeId = 0, string type = null, string startDate = null, string endDate = null)
        {
            var managedDepartments = HttpUtilities.GetManagedDepartments();

            var isStartDateParsed = DateTimeOffset.TryParse(startDate, out var startDateTime);
            var isEndDateParsed = DateTimeOffset.TryParse(endDate, out var endDateTime);

            return _hRDemoAPIDb.Leaves
                .Where(l => employeeId == default || (l.EmployeeID == employeeId))
                .Where(l => !isStartDateParsed || l.StartDate >= startDateTime)
                .Where(l => !isEndDateParsed || l.EndDate <= endDateTime)
                .Where(l => type == null || l.Type.ToString().ToLower() == type.ToLower())
                .Include("Employee")
                .Where(l => managedDepartments.Count == 0 || (l.Employee != null && l.Employee.DepartmentID != null && managedDepartments.Contains((int)l.Employee.DepartmentID)))
                .OrderBy(a => a.LeaveID)
                .Paginate(count, page)
                .AsNoTracking()
                .ToList()
                .Select(a => a.MapQueryResult());
        }

        // GET api/leaves/5
        public HttpResponseMessage Get(int id)
        {
            var managedDepartments = HttpUtilities.GetManagedDepartments();

            LeaveResponse leave = _hRDemoAPIDb.Leaves
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

        // POST api/leaves
        public HttpResponseMessage Post([FromBody]LeaveRequest leaveRequest)
        {
            var employee = _hRDemoAPIDb.Employees.Find(leaveRequest.EmployeeID);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage($"Employee not found for given ID {leaveRequest.EmployeeID}", System.Net.HttpStatusCode.BadRequest);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Leave newLeave = leaveRequest.MapPostRequest();
            Leave savedLeave = _hRDemoAPIDb.Leaves.Add(newLeave);
            _hRDemoAPIDb.SaveChanges();
            return savedLeave.CreateResponseMessage();
        }

        // PUT api/leaves/5
        public HttpResponseMessage Put(int id, [FromBody]LeaveRequest leaveRequest)
        {
            Leave leave = _hRDemoAPIDb.Leaves
                .Include("Employee")
                .Where(a => a.LeaveID == id)
                .FirstOrDefault();
            if (leave == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(leave.Employee.DepartmentID);
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

        // PATCH api/Leaves/5
        public HttpResponseMessage Patch(int id, [FromBody] LeaveRequest leaveRequest)
        {
            Leave leave = _hRDemoAPIDb.Leaves
                .Include("Employee")
                .Where(a => a.LeaveID == id)
                .FirstOrDefault();

            if (leave == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(leave.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if(leaveRequest.Reason != null)
            {
                leave.Reason = leaveRequest.Reason;
            }
            if (leaveRequest.StartDate != null)
            {
                leave.StartDate = leaveRequest.StartDate;
            }
            if (leaveRequest.EndDate != null)
            {
                leave.EndDate = leaveRequest.EndDate;
            }
            _hRDemoAPIDb.SaveChanges();
            return leave.CreateResponseMessage();
        }

        // DELETE api/leaves/5
        public HttpResponseMessage Delete(int id)
        {
            Leave leave = _hRDemoAPIDb.Leaves
                .Include("Employee")
                .Where(a => a.LeaveID == id)
                .FirstOrDefault();

            if (leave == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(leave.Employee.DepartmentID);
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
