using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;

namespace HRDemoAdminCore.Controllers
{
    public class HomeController(IConfiguration configuration, IAuthorizationHeaderProvider authHeaderProvider) : HRDemoControllerBase(configuration, authHeaderProvider)
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}