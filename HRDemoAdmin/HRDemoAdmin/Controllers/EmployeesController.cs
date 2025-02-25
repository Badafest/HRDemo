using System.Web.Mvc;

namespace HRDemoAdmin.Controllers
{
    public class EmployeesController : ControllerBase
    {
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }
    }
}