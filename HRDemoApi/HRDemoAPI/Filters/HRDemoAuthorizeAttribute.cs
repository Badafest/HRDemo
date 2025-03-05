using HRDemoAPI.Data;
using HRDemoAPI.Utilities;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using Unity;

namespace HRDemoAPI.Filters
{
    public class HRDemoAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly HRDemoApiDbContainer _hRDemoApiDb;
        public HRDemoAuthorizeAttribute()
        {
            _hRDemoApiDb = UnityConfig.container.Resolve<HRDemoApiDbContainer>();
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            var principal = actionContext.ControllerContext.RequestContext.Principal;
            var identity = principal.Identity as ClaimsIdentity;

            // Fetch user data from the database
            var user = _hRDemoApiDb.Users
                .Include("Employee")
                .AsNoTracking()
                .FirstOrDefault(u => u.Name == identity.Name);

            if (user == null)
            {
                actionContext.Response = HttpUtilities.CreateResponseMessage($"User {identity?.Name} not found in the database", System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            if (user.Employee == null)
            {
                actionContext.Response = HttpUtilities.CreateResponseMessage($"User {identity?.Name} is not mapped to any employee", System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            var role = user.Role.ToString();
            identity.AddClaim(new Claim("userRole", role));

            identity.AddClaim(new Claim(identity.RoleClaimType, role));

            var managedDepartments = _hRDemoApiDb.Departments.Where(d => d.ManagerID !=null && d.ManagerID == user.EmployeeID).Select(d => d.DepartmentID);
            foreach (var departmentId in managedDepartments)
            {
                identity.AddClaim(new Claim(identity.RoleClaimType, $"Manager-{departmentId}"));
            }

            identity.AddClaim(new Claim("displayName", $"{user.Employee.FirstName} {user.Employee.LastName}"));
        }
    }

}