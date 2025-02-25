using System.Web.Mvc;

namespace HRDemoAdmin.Controllers
{
    public class AttendancesController : ControllerBase
    {
        // GET: Attendances
        public ActionResult Index()
        {
            return View();
        }
    }
}