using HRDemoAdmin.Services;
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
        public ActionResult Index()
        {
            ViewData.Model = _departmentService.GetDepartments();
            return View();
        }

        // GET: Departments/{id}
        public ActionResult Details(int id)
        {
            ViewData.Model = _departmentService.GetDepartmentDetails(id);
            return View();
        }

        // GET: Departments/Edit/{id}
        public ActionResult Edit(int id)
        {
            ViewData.Model = _departmentService.GetDepartmentDetails(id);
            return View();
        }

        // GET: Departments/Delete/{id}
        public ActionResult Delete(int id)
        {
            ViewData.Model = _departmentService.GetDepartmentDetails(id);
            return View();
        }
    }
}