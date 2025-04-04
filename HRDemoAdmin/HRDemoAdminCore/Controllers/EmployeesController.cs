﻿using HRDemoAdmin.Services;
using HRDemoAdmin.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Abstractions;

namespace HRDemoAdminCore.Controllers
{
    public class EmployeesController : HRDemoControllerBase
    {
        private readonly EmployeeService _employeeService;
        public EmployeesController(IConfiguration configuration, IAuthorizationHeaderProvider authHeaderProvider) : base(configuration, authHeaderProvider)
        {
            _employeeService = new EmployeeService(ApiBaseUrl, BearerToken);
        }
        // GET: Employees
        public ActionResult Index(string firstName = "", string lastName = "", string jobTitle = "", string email = "", string phone = "")
        {
            var response = _employeeService.GetEmployees(firstName, lastName, jobTitle, phone, email);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // GET: Employees/{id}
        public ActionResult Details(int id)
        {
            var response = _employeeService.GetEmployeeDetails(id, true);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // Get: Employees/Create
        public ActionResult Create()
        {

            ViewData.Model = new EmployeeRequest();
            return View();
        }

        // POST: Employees/Edit/{id}
        [HttpPost]
        public ActionResult Create(EmployeeRequest employeeRequest)
        {
            if (!string.IsNullOrEmpty(employeeRequest.departmentName))
            {
                var department = _employeeService.GetDepartmentByName(employeeRequest.departmentName);
                if (department == null)
                {
                    ModelState.AddModelError("departmentName", $"Department not found with name {employeeRequest.departmentName}");
                    return View(employeeRequest);
                }
                employeeRequest.departmentId = department.DepartmentID;
            }
            var response = _employeeService.CreateEmployee(employeeRequest);
            return HandleApiResponse(response, employeeRequest) ?? RedirectToAction("Details", new { id = response.Data.EmployeeID });
        }

        // GET: Employees/Edit/{id}
        public ActionResult Edit(int id)
        {
            var response = _employeeService.GetEmployeeDetails(id, true);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Employees/Edit/{id}
        [HttpPost]
        public ActionResult Edit(int id, EmployeeResponse employeeResponse)
        {
            int departmentId = 0;
            if (!string.IsNullOrEmpty(employeeResponse.Department?.Name))
            {
                var department = _employeeService.GetDepartmentByName(employeeResponse.Department.Name);
                if (department == null)
                {
                    ModelState.AddModelError("Department", $"Department not found with name {employeeResponse.Department.Name}");
                    employeeResponse.EmployeeID = id;
                    return View(employeeResponse);
                }
                departmentId = department.DepartmentID;
            }
            EmployeeRequest request = new EmployeeRequest
            {
                firstName = employeeResponse.FirstName,
                lastName = employeeResponse.LastName,
                email = employeeResponse.Email,
                phone = employeeResponse.Phone,
                jobTitle = employeeResponse.JobTitle,
                salary = employeeResponse.Salary,
                address = new EmployeeRequestAddress
                {
                    line1 = employeeResponse.Address.Line1,
                    line2 = employeeResponse.Address.Line2,
                    city = employeeResponse.Address.City,
                    state = employeeResponse.Address.State,
                    postalCode = employeeResponse.Address.PostalCode,
                    country = employeeResponse.Address.Country,
                },
                departmentId = departmentId,
            };
            var response = _employeeService.EditEmployee(id, request);
            return HandleApiResponse(response, employeeResponse) ?? RedirectToAction("Details", new { id });
        }

        // GET: Employees/Delete/{id}
        public ActionResult Delete(int id)
        {
            var response = _employeeService.GetEmployeeDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Employees/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id, EmployeeResponse employeeResponse)
        {
            var response = _employeeService.DeleteEmployee(id);
            return HandleApiResponse(response, response.Data) ?? RedirectToAction("Index");
        }

        // POST: Employees/EmployeeStatus/{id}
        [HttpPost]
        public ActionResult EmployeeStatus(int id, bool hire = true)
        {
            var response = _employeeService.EmployeeStatus(id, hire);
            return HandleApiResponse(response, response.Data) ?? new JsonResult(new { Data = response });
        }
    }
}