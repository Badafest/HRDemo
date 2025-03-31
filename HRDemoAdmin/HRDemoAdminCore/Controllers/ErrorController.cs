using Microsoft.AspNetCore.Mvc;

namespace HRDemoAdminCore.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("Error");
        }
    }
}
