using System.Web.Mvc;

namespace HRDemoAdmin.Controllers
{
    public class LeavesController : ControllerBase
    {
        // GET: Leaves
        public ActionResult Index()
        {
            return View();
        }
    }
}