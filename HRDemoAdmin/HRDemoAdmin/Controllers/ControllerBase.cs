using HRDemoAdmin.Services;
using System.Collections.Generic;
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

        public ActionResult HandleApiResponse<T>(IApiResponse response, T model)
        {
            if (!response.Success)
            {
                var modelState = response.ErrorResponse["ModelState"];
                if (modelState != null)
                {
                    var parsedState = modelState.ToObject<IDictionary<string, string[]>>();
                    foreach (var error in parsedState)
                    {
                        ModelState.AddModelError(error.Key.Substring(error.Key.IndexOf('.') + 1), string.Join(", ", error.Value));
                    }
                    return View(model);
                }
                return View("Error", response.ErrorResponse);
            }
            return null;
        }
    }
}