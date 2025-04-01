using HRDemoAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;

namespace HRDemoAdminCore.Controllers
{
    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "AuthScopes")]
    public abstract class HRDemoControllerBase(IConfiguration configuration, IAuthorizationHeaderProvider authHeaderProvider) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IAuthorizationHeaderProvider _authHeaderProvider = authHeaderProvider;

        public string? BearerToken
        {
            get
            {
                var authScopes = _configuration["AuthScopes"]?.Split(",") ?? [];
                return _authHeaderProvider.CreateAuthorizationHeaderForUserAsync(authScopes).Result;
            }
        }

        public readonly string? ApiBaseUrl = configuration["HRDemoApiBaseUrl"];
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userResponse = new UserService(ApiBaseUrl, BearerToken).GetUserDetails();
            ViewBag.UserName = userResponse.Name;
            ViewBag.UserRole = userResponse.Role;
            base.OnActionExecuting(filterContext);
        }

        [NonAction]
        public ActionResult? HandleApiResponse<T>(IApiResponse response, T model)
        {
            if (!response.Success)
            {
                var modelState = response.ErrorResponse["ModelState"];
                var parsedState = modelState?.ToObject<IDictionary<string, string[]>>();
                if (parsedState != null)
                {
                    foreach (var error in parsedState)
                    {
                        ModelState.AddModelError(error.Key.Substring(error.Key.IndexOf('.') + 1), string.Join(", ", error.Value));
                    }
                    return View(model);
                }
                Response.StatusCode = (response.ErrorResponse["StatusCode"] ?? 500).ToObject<int>();
                return View("Error", response.ErrorResponse);
            }
            return null;
        }
    }
}