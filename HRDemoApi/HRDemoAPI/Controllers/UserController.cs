using HRDemoAPI.Filters;
using HRDemoApp.Utilities;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace HRDemoApp.Controllers
{
    [HRDemoAuthorize]
    public class UserController : ApiController
    {
        // GET: User
        public HttpResponseMessage Get()
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;
            var responseData = new
            {
                Name = user.Claims.FirstOrDefault(claim => claim.Type == "displayName").Value,
                Role = user.Claims.FirstOrDefault(claim => claim.Type == "userRole").Value,
            };
            return responseData.CreateResponseMessage();
        }
    }
}