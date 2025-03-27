using HRDemoAdmin.Services;
using HRDemoAdmin.Services.Models;
using System;
using System.Web.Mvc;

namespace HRDemoAdmin.Controllers
{
    public class LeavesController : ControllerBase
    {
        private readonly LeaveService _leaveService;
        public LeavesController()
        {
            var apiBaseUrl = System.Configuration.ConfigurationManager.AppSettings["HRDemoApiBaseUrl"];
            _leaveService = new LeaveService(apiBaseUrl);
        }
        // GET: Leaves
        public ActionResult Index(string employeeEmail, string type = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            int employeeId = 0;
            if (!string.IsNullOrEmpty(employeeEmail))
            {
                var employee = _leaveService.GetEmployeeByEmail(employeeEmail);
                employeeId = employee?.EmployeeID ?? -1;
            }

            var minStartDate = DateTimeOffset.Now.Subtract(new TimeSpan(2 * 365, 0, 0, 0));
            var maxEndDate = DateTimeOffset.Now;

            startDate = (startDate != null && startDate > minStartDate) ? startDate : minStartDate;
            endDate = (endDate != null && endDate < maxEndDate) ? endDate : maxEndDate;

            var response = _leaveService.GetLeaves(employeeId, type, startDate, endDate);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // GET: Leaves/{id}
        public ActionResult Details(int id)
        {
            var response = _leaveService.GetLeaveDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // Get: Leaves/Create
        [HttpGet]
        public ActionResult Create()
        {

            ViewData.Model = new LeaveRequest();
            return View();
        }

        // POST: Leaves/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(LeaveRequest leaveRequest)
        {
            if (string.IsNullOrEmpty(leaveRequest.employeeEmail))
            {
                ModelState.AddModelError("employeeEmail", $"Employee email is required");
                return View(leaveRequest);
            }

            var employee = _leaveService.GetEmployeeByEmail(leaveRequest.employeeEmail);
            if (employee == null)
            {
                ModelState.AddModelError("employeeEmail", $"Employee not found with email {leaveRequest.employeeEmail}");
                return View(leaveRequest);
            }

            leaveRequest.employeeId = employee.EmployeeID;

            var response = _leaveService.CreateLeave(leaveRequest);
            return HandleApiResponse(response, leaveRequest) ?? RedirectToAction("Details", new { id = response.Data.LeaveID });
        }

        // GET: Leaves/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var response = _leaveService.GetLeaveDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Leaves/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LeaveResponse leaveResponse)
        {
            LeaveRequest request = new LeaveRequest
            {
                reason = leaveResponse.Reason,
                startDate = leaveResponse.StartDate,
                endDate = leaveResponse.EndDate,
                type = leaveResponse.Type,
            };
            var response = _leaveService.EditLeave(id, request);
            return HandleApiResponse(response, leaveResponse) ?? RedirectToAction("Details", new { id });
        }

        // GET: Leaves/Delete/{id}
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var response = _leaveService.GetLeaveDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Leaves/Delete/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LeaveResponse leaveResponse)
        {
            var response = _leaveService.DeleteLeave(id);
            return HandleApiResponse(response, response.Data) ?? RedirectToAction("Index");
        }

        // POST: Leaves/LeaveStatus/{id}
        [HttpPost]
        public ActionResult LeaveStatus(int id, bool approve = true)
        {
            var response = _leaveService.LeaveStatus(id, approve);
            return HandleApiResponse(response, response.Data) ?? new JsonResult() { Data = response };
        }
    }
}