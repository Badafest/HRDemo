using HRDemoAdmin.Services;
using HRDemoAdmin.Services.Models;
using System.Collections.Generic;
using System.Linq;
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
                        var nestedKeys = error.Key.Split(new char[] { '.' });
                        ModelState.AddModelError(nestedKeys[nestedKeys.Length - 1], string.Join(", ", error.Value));
                    }
                    return View(model);
                }
                return View("Error", response.ErrorResponse);
            }
            return null;
        }
    }
}