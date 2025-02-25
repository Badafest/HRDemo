using System.Web.Mvc;

namespace HRDemoAdmin.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}