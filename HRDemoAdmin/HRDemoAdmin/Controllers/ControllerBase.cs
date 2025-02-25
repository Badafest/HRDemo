using HRDemoAdmin.Services;
using System.Web.Mvc;

namespace HRDemoAdmin.Controllers
{
    public class ControllerBase : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var apiBaseUrl = System.Configuration.ConfigurationManager.AppSettings["HRDemoApiBaseUrl"];
            var userResponse = new UserService(apiBaseUrl).GetUserDetails();
            ViewBag.UserName = userResponse.Name;
            ViewBag.UserRole = userResponse.Role;
            base.OnActionExecuting(filterContext);
        }
    }
}