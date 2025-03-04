using HRDemoAPICore.Filters;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace HRDemoAPICore.Controllers
{
    [ServiceFilter<HRDemoAuthorizeFilter>]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        // GET: User
        public ObjectResult Get()
        {
            var user = HttpContext.User;
            var responseData = new
            {
                Name = user.Claims.FirstOrDefault(claim => claim.Type == "displayName")?.Value,
                Role = user.Claims.FirstOrDefault(claim => claim.Type == "userRole")?.Value,
            };
            return responseData.CreateResponseMessage();
        }
    }
}