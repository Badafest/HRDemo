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
    public class AttendancesController(HRDemoApiContext hrDemoApiDb) : ControllerBase
    {
        private HRDemoApiContext _hRDemoAPIDb = hrDemoApiDb;

        [HttpGet]
        // GET api/attendances
        public IEnumerable<AttendanceResponse> Get(int count = default, int page = default, int employeeId = default, string? startDate = null, string? endDate = null)
        {
            var managedDepartments = HttpUtilities.GetManagedDepartments(HttpContext);

            var isStartDateParsed = DateTimeOffset.TryParse(startDate, out var startDateTime);
            var isEndDateParsed = DateTimeOffset.TryParse(endDate, out var endDateTime);

            return _hRDemoAPIDb.Attendances
                .Where(a => employeeId == default || a.EmployeeID == employeeId)
                .Where(a => !isStartDateParsed || a.Date >= startDateTime)
                .Where(a => !isEndDateParsed || a.Date <= endDateTime)
                .Include("Employee")
                .Where(a => managedDepartments.Count == 0 || (a.Employee != null && a.Employee.DepartmentID != null && managedDepartments.Contains((int)a.Employee.DepartmentID)))
                .OrderBy(a => a.AttendanceID)
                .Paginate(count, page, HttpContext)
                .AsNoTracking()
                .ToList()
                .Select(a => a.MapQueryResult());
        }

        [HttpGet("{id}")]

        // GET api/attendances/5
        public ObjectResult Get(int id)
        {
            var managedDepartments = HttpUtilities.GetManagedDepartments(HttpContext);

            AttendanceResponse? attendance = _hRDemoAPIDb.Attendances
                .Where(e => e.AttendanceID == id)
                .Include("Employee")
                .Where(a => managedDepartments.Count == 0 || (a.Employee != null && a.Employee.DepartmentID != null && managedDepartments.Contains((int)a.Employee.DepartmentID)))
                .AsNoTracking()
                .ToList()
                .Select(e => e.MapQueryResult())
                .FirstOrDefault();
            if (attendance == null || attendance.AttendanceID != id)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            return attendance.CreateResponseMessage();
        }

        [HttpPost]

        // POST api/attendances
        public ObjectResult Post([FromBody] AttendanceRequest attendanceRequest)
        {
            var employee = _hRDemoAPIDb.Employees.Find(attendanceRequest.EmployeeID);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage($"Employee not found for given ID {attendanceRequest.EmployeeID}", System.Net.HttpStatusCode.BadRequest);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Attendance newAttendance = attendanceRequest.MapPostRequest();
            Attendance savedAttendance = _hRDemoAPIDb.Attendances.Add(newAttendance).Entity;
            _hRDemoAPIDb.SaveChanges();
            return savedAttendance.CreateResponseMessage();
        }

        [HttpPut("{id}")]

        // PUT api/attendances/5
        public ObjectResult Put(int id, [FromBody] AttendanceRequest attendanceRequest)
        {
            Attendance? attendance = _hRDemoAPIDb.Attendances
                .Include("Employee")
                .Where(a => a.AttendanceID == id)
                .FirstOrDefault();
            if (attendance == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, attendance.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }

            Attendance newAttendance = attendanceRequest.MapPutRequest(id);
            attendance.Date = newAttendance.Date;
            attendance.CheckInTime = newAttendance.CheckInTime;
            attendance.CheckOutTime = newAttendance.CheckOutTime;
            attendance.Status = newAttendance.Status;
            _hRDemoAPIDb.SaveChanges();
            return attendance.CreateResponseMessage();
        }

        [HttpPatch("{id}")]

        // PATCH api/Attendances/5
        public ObjectResult Patch(int id, [FromBody] AttendanceRequest attendanceRequest)
        {
            Attendance? attendance = _hRDemoAPIDb.Attendances
                .Include("Employee")
                .Where(a => a.AttendanceID == id)
                .FirstOrDefault();

            if (attendance == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, attendance.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if (attendanceRequest.Date > DateTimeOffset.MinValue)
            {
                attendance.Date = attendanceRequest.Date;
            }
            if (attendanceRequest.CheckInTime > DateTimeOffset.MinValue)
            {
                attendance.CheckInTime = attendanceRequest.CheckInTime;
            }
            if (attendanceRequest.CheckOutTime > DateTimeOffset.MinValue)
            {
                attendance.CheckOutTime = attendanceRequest.CheckOutTime;
            }
            attendance.Status = AttendanceMapper.GetAttendanceStatus(attendance.Date, attendance.CheckInTime, attendance.CheckOutTime);
            _hRDemoAPIDb.SaveChanges();
            return attendance.CreateResponseMessage();
        }

        [HttpDelete("{id}")]

        // DELETE api/attendances/5
        public ObjectResult Delete(int id)
        {
            Attendance? attendance = _hRDemoAPIDb.Attendances
                .Include("Employee")
                .Where(a => a.AttendanceID == id)
                .FirstOrDefault();

            if (attendance == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(HttpContext, attendance.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            _hRDemoAPIDb.Attendances.Remove(attendance);
            _hRDemoAPIDb.SaveChanges();
            return HttpUtilities.CreateResponseMessage(null);
        }
    }
}
