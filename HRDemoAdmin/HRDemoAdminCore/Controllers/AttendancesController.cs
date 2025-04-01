using HRDemoAdmin.Services;
using HRDemoAdmin.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Abstractions;

namespace HRDemoAdminCore.Controllers
{
    public class AttendancesController : HRDemoControllerBase
    {
        private readonly AttendanceService _attendanceService = null!;

        public AttendancesController(IConfiguration configuration, IAuthorizationHeaderProvider authHeaderProvider): base(configuration, authHeaderProvider)
        {
            _attendanceService = new(ApiBaseUrl, BearerToken);
        }
        // GET: Attendances
        public ActionResult Index(string employeeEmail, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            int employeeId = 0;
            if (!string.IsNullOrEmpty(employeeEmail))
            {
                var employee = _attendanceService.GetEmployeeByEmail(employeeEmail);
                employeeId = employee?.EmployeeID ?? -1;
            }

            var minStartDate = DateTimeOffset.Now.Subtract(new TimeSpan(2 * 365, 0, 0, 0));
            var maxEndDate = DateTimeOffset.Now;

            startDate = (startDate != null && startDate > minStartDate) ? startDate : minStartDate;
            endDate = (endDate != null && endDate < maxEndDate) ? endDate : maxEndDate;

            var response = _attendanceService.GetAttendances(employeeId, startDate, endDate);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // GET: Attendances/{id}
        public ActionResult Details(int id)
        {
            var response = _attendanceService.GetAttendanceDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // Get: Attendances/Create
        public ActionResult Create()
        {

            ViewData.Model = new AttendanceRequest();
            return View();
        }

        // POST: Attendances/Edit/{id}
        [HttpPost]
        public ActionResult Create(AttendanceRequest attendanceRequest)
        {
            if (string.IsNullOrEmpty(attendanceRequest.employeeEmail))
            {
                ModelState.AddModelError("employeeEmail", $"Employee email is required");
                return View(attendanceRequest);
            }

            var employee = _attendanceService.GetEmployeeByEmail(attendanceRequest.employeeEmail);
            if (employee == null)
            {
                ModelState.AddModelError("employeeEmail", $"Employee not found with email {attendanceRequest.employeeEmail}");
                return View(attendanceRequest);
            }

            attendanceRequest.employeeId = employee.EmployeeID;

            attendanceRequest.date = new DateTime(attendanceRequest.date.Year, attendanceRequest.date.Month, attendanceRequest.date.Day, 9, 0, 0);
            attendanceRequest.checkIn = new DateTime(attendanceRequest.date.Year, attendanceRequest.date.Month, attendanceRequest.date.Day, attendanceRequest.checkIn.Hour, attendanceRequest.checkIn.Minute, attendanceRequest.checkIn.Second);
            attendanceRequest.checkOut = new DateTime(attendanceRequest.date.Year, attendanceRequest.date.Month, attendanceRequest.date.Day, attendanceRequest.checkOut.Hour, attendanceRequest.checkOut.Minute, attendanceRequest.checkOut.Second);

            var response = _attendanceService.CreateAttendance(attendanceRequest);
            return HandleApiResponse(response, attendanceRequest) ?? RedirectToAction("Details", new { id = response.Data.AttendanceID });
        }

        // GET: Attendances/Edit/{id}
        public ActionResult Edit(int id)
        {
            var response = _attendanceService.GetAttendanceDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Attendances/Edit/{id}
        [HttpPost]
        public ActionResult Edit(int id, AttendanceResponse attendanceResponse)
        {
            AttendanceRequest attendanceRequest = new AttendanceRequest
            {
                date = attendanceResponse.Date,
                checkIn = attendanceResponse.CheckInTime,
                checkOut = attendanceResponse.CheckOutTime
            };

            attendanceRequest.date = new DateTime(attendanceRequest.date.Year, attendanceRequest.date.Month, attendanceRequest.date.Day, 9, 0, 0);
            attendanceRequest.checkIn = new DateTime(attendanceRequest.date.Year, attendanceRequest.date.Month, attendanceRequest.date.Day, attendanceRequest.checkIn.Hour, attendanceRequest.checkIn.Minute, attendanceRequest.checkIn.Second);
            attendanceRequest.checkOut = new DateTime(attendanceRequest.date.Year, attendanceRequest.date.Month, attendanceRequest.date.Day, attendanceRequest.checkOut.Hour, attendanceRequest.checkOut.Minute, attendanceRequest.checkOut.Second);

            var response = _attendanceService.EditAttendance(id, attendanceRequest);
            return HandleApiResponse(response, attendanceResponse) ?? RedirectToAction("Details", new { id });
        }

        // GET: Attendances/Delete/{id}
        public ActionResult Delete(int id)
        {
            var response = _attendanceService.GetAttendanceDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Attendances/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id, AttendanceResponse attendanceResponse)
        {
            var response = _attendanceService.DeleteAttendance(id);
            return HandleApiResponse(response, response.Data) ?? RedirectToAction("Index");
        }
    }
}