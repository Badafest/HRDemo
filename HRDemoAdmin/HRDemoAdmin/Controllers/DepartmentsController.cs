using HRDemoAdmin.Services;
using HRDemoAdmin.Services.Models;
using System.Web.Mvc;

namespace HRDemoAdmin.Controllers
{
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _departmentService;
        public DepartmentsController()
        {
            var apiBaseUrl = System.Configuration.ConfigurationManager.AppSettings["HRDemoApiBaseUrl"];
            _departmentService = new DepartmentService(apiBaseUrl);
        }
        // GET: Departments
        public ActionResult Index(string name = "")
        {
            var response = _departmentService.GetDepartments(name);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // GET: Departments/{id}
        public ActionResult Details(int id)
        {
            var response = _departmentService.GetDepartmentDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // Get: Departments/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewData.Model = new DepartmentRequest();
            return View();
        }

        // POST: Departments/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentRequest departmentRequest)
        {
            var response = _departmentService.CreateDepartment(departmentRequest);
            var handledView = HandleApiResponse(response, departmentRequest);
            return handledView ?? RedirectToAction("Details", new { id = response.Data.DepartmentID });
        }

        // GET: Departments/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var response = _departmentService.GetDepartmentDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Departments/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DepartmentResponse departmentResponse)
        {
            int managerId = 0;
            if (!string.IsNullOrEmpty(departmentResponse.Manager?.Email))
            {
                var manager = _departmentService.GetManagerByEmail(departmentResponse.Manager.Email);
                if (manager == null || manager?.Department?.DepartmentID != id)
                {
                    ModelState.AddModelError("Manager", $"Manager not found for email {departmentResponse.Manager.Email} in this department");
                    departmentResponse.DepartmentID = id;
                    return View(departmentResponse);
                }
                managerId = manager.EmployeeID;
            }
            DepartmentRequest request = new DepartmentRequest { 
                name = departmentResponse.Name,
                description = departmentResponse.Description,
                managerId = managerId,
            };
            var response = _departmentService.EditDepartment(id, request);
            return HandleApiResponse(response, departmentResponse) ?? RedirectToAction("Details", new { id });
        }

        // GET: Departments/Delete/{id}
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var response = _departmentService.GetDepartmentDetails(id);
            return HandleApiResponse(response, response.Data) ?? View(response.Data);
        }

        // POST: Departments/Delete/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DepartmentResponse departmentResponse)
        {
            var response = _departmentService.DeleteDepartment(id);
            return HandleApiResponse(response, response.Data) ?? RedirectToAction("Index");
        }
    }
}