using HRDemoAPI.Data;
using HRDemoAPI.Filters;
using HRDemoApp.Models;
using HRDemoApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace HRDemoAPI.Controllers
{
    [HRDemoAuthorize]
    public class AttendancesController : ApiController
    {
        private readonly HRDemoApiDbContainer _hRDemoAPIDb;
        public AttendancesController(HRDemoApiDbContainer hRDemoAPIDb)
        {
            _hRDemoAPIDb = hRDemoAPIDb;
        }
        // GET api/attendances
        public IEnumerable<AttendanceResponse> Get(int count = default, int page = default, int employeeId = default, string startDate = null, string endDate = null)
        {
            var isStartDateParsed = DateTimeOffset.TryParse(startDate, out var startDateTime);
            var isEndDateParsed = DateTimeOffset.TryParse(endDate, out var endDateTime);

            return _hRDemoAPIDb.Attendances
                .Where(a => employeeId == default || (a.EmployeeID == employeeId))
                .Where(a => !isStartDateParsed || a.Date >= startDateTime)
                .Where(a => !isEndDateParsed || a.Date <= endDateTime)
                .OrderBy(a => a.AttendanceID)
                .Paginate(count, page)
                .Include("Employee")
                .AsNoTracking()
                .ToList()
                .Select(a => a.MapQueryResult());
        }

        // GET api/attendances/5
        public HttpResponseMessage Get(int id)
        {
            AttendanceResponse attendance = _hRDemoAPIDb.Attendances
                .Where(e => e.AttendanceID == id)
                .Include("Employee")
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

        // POST api/attendances
        public HttpResponseMessage Post([FromBody]AttendanceRequest attendanceRequest)
        {
            var employee = _hRDemoAPIDb.Employees.Find(attendanceRequest.EmployeeID);
            if (employee == null)
            {
                return HttpUtilities.CreateResponseMessage($"Employee not found for given ID {attendanceRequest.EmployeeID}", System.Net.HttpStatusCode.BadRequest);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            Attendance newAttendance = attendanceRequest.MapPostRequest();
            Attendance savedAttendance = _hRDemoAPIDb.Attendances.Add(newAttendance);
            _hRDemoAPIDb.SaveChanges();
            return savedAttendance.CreateResponseMessage();
        }

        // PUT api/attendances/5
        public HttpResponseMessage Put(int id, [FromBody]AttendanceRequest attendanceRequest)
        {
            Attendance attendance = _hRDemoAPIDb.Attendances
                .Include("Employee")
                .Where(a => a.AttendanceID == id)
                .FirstOrDefault();
            if (attendance == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(attendance.Employee.DepartmentID);
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

        // PATCH api/Attendances/5
        public HttpResponseMessage Patch(int id, [FromBody] AttendanceRequest attendanceRequest)
        {
            Attendance attendance = _hRDemoAPIDb.Attendances
                .Include("Employee")
                .Where(a => a.AttendanceID == id)
                .FirstOrDefault();

            if (attendance == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(attendance.Employee.DepartmentID);
            if (validatedResponse != null)
            {
                return validatedResponse;
            }
            if(attendanceRequest.Date != null)
            {
                attendance.Date = attendanceRequest.Date;
            }
            if (attendanceRequest.CheckInTime != null)
            {
                attendance.CheckInTime = attendanceRequest.CheckInTime;
            }
            if (attendanceRequest.CheckOutTime != null)
            {
                attendance.CheckOutTime = attendanceRequest.CheckOutTime;
            }
            attendance.Status = AttendanceMapper.GetAttendanceStatus(attendance.Date, attendance.CheckInTime, attendance.CheckOutTime);
            _hRDemoAPIDb.SaveChanges();
            return attendance.CreateResponseMessage();
        }

        // DELETE api/attendances/5
        public HttpResponseMessage Delete(int id)
        {
            Attendance attendance = _hRDemoAPIDb.Attendances
                .Include("Employee")
                .Where(a => a.AttendanceID == id)
                .FirstOrDefault();

            if (attendance == null)
            {
                return HttpUtilities.CreateResponseMessage(null, System.Net.HttpStatusCode.NotFound);
            }
            var validatedResponse = HttpUtilities.ValidateManagerRole(attendance.Employee.DepartmentID);
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
