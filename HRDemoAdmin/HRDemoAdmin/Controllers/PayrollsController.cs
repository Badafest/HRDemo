using System.Web.Mvc;

namespace HRDemoAdmin.Controllers
{
    public class PayrollsController : ControllerBase
    {
        // GET: Payrolls
        public ActionResult Index()
        {
            return View();
        }
    }
}