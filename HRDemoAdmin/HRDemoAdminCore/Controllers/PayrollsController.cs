﻿using HRDemoAdmin.Services;
using HRDemoAdmin.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Abstractions;

namespace HRDemoAdminCore.Controllers
{
    public class PayrollsController : HRDemoControllerBase
    {
        private readonly PayrollService _payrollService;
        public PayrollsController(IConfiguration configuration, IAuthorizationHeaderProvider authHeaderProvider) : base(configuration, authHeaderProvider)
        {
            _payrollService = new PayrollService(ApiBaseUrl, BearerToken);
        }
        // GET: Payrolls
        public ActionResult Index(string employeeEmail, int? year = null, int? month = null)
        {
            int employeeId = 0;
            if (!string.IsNullOrEmpty(employeeEmail))
            {
                var employee = _payrollService.GetEmployeeByEmail(employeeEmail);
                employeeId = employee?.EmployeeID ?? -1;
            }
            var response = _payrollService.GetPayrolls(employeeId, year ?? DateTime.Now.Year, month ?? DateTime.Now.Month);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // GET: Payrolls/{id}
        public ActionResult Details(int id)
        {
            var response = _payrollService.GetPayrollDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // Get: Payrolls/Create
        public ActionResult Create()
        {
            ViewData.Model = new PayrollRequest();
            return View();
        }

        // POST: Payrolls/Edit/{id}
        [HttpPost]
        public ActionResult Create(PayrollRequest payrollRequest, string buttonType)
        {
            if (string.IsNullOrEmpty(payrollRequest.employeeEmail))
            {
                ModelState.AddModelError("employeeEmail", $"Employee email is required");
                return View(payrollRequest);
            }

            var employee = _payrollService.GetEmployeeByEmail(payrollRequest.employeeEmail);
            if (employee == null)
            {
                ModelState.AddModelError("employeeEmail", $"Employee not found with email {payrollRequest.employeeEmail}");
                return View(payrollRequest);
            }

            payrollRequest.employeeId = employee.EmployeeID;

            if (buttonType == "Report")
            {
                var reportResponse = _payrollService.EmployeeReport(payrollRequest.employeeId, payrollRequest.year, payrollRequest.month, payrollRequest.offset);
                ViewBag.reportData = reportResponse.Data;
                return View(payrollRequest);
            }

            var response = _payrollService.CreatePayroll(payrollRequest);
            return HandleApiResponse(response, payrollRequest) ?? RedirectToAction("Details", new { id = response.Data.PayrollID });
        }

        // GET: Payrolls/Edit/{id}
        public ActionResult Edit(int id)
        {
            var response = _payrollService.GetPayrollDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Payrolls/Edit/{id}
        [HttpPost]
        public ActionResult Edit(int id, PayrollResponse payrollResponse)
        {
            PayrollRequest request = new PayrollRequest
            {
                month = payrollResponse.Month,
                year = payrollResponse.Year,
                salary = new PayrollRequestSalary
                {
                    grossAmount = payrollResponse.Salary.GrossAmount,
                    preTaxDeduction = payrollResponse.Salary.PreTaxDeduction,
                    taxDeduction = payrollResponse.Salary.TaxDeduction,
                    postTaxDeduction = payrollResponse.Salary.PostTaxDeduction
                }
            };
            var response = _payrollService.EditPayroll(id, request);
            return HandleApiResponse(response, payrollResponse) ?? RedirectToAction("Details", new { id });
        }

        // GET: Payrolls/Delete/{id
        public ActionResult Delete(int id)
        {
            var response = _payrollService.GetPayrollDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Payrolls/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id, PayrollResponse payrollResponse)
        {
            var response = _payrollService.DeletePayroll(id);
            return HandleApiResponse(response, response.Data) ?? RedirectToAction("Index");
        }

        // POST: Payrolls/PayrollStatus/{id}
        [HttpPost]
        public ActionResult PayrollStatus(int id, bool processed = true)
        {
            var response = _payrollService.PayrollStatus(id, processed);
            return HandleApiResponse(response, response.Data) ?? new JsonResult(new { Data = response });
        }
    }
}