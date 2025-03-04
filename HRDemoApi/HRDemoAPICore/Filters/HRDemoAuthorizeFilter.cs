using HRDemoAPI.DataCore.Models;
using HRDemoAPICore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRDemoAPICore.Filters
{
    public class HRDemoAuthorizeFilter(IAuthorizationService authorizationService, IAuthorizationPolicyProvider policyProvider) : IAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly IAuthorizationPolicyProvider _policyProvider = policyProvider;
        public async void OnAuthorization(AuthorizationFilterContext actionContext)
        {
            var principal = actionContext.HttpContext.User;

            // Retrieve the default authorization policy
            var policy = await _policyProvider.GetDefaultPolicyAsync();

            // Evaluate the authorization policy
            var result = await _authorizationService.AuthorizeAsync(principal, null, policy);

            if (!result.Succeeded)
            {
                // Handle authorization failure (e.g., set result to challenge)
                actionContext.Result = new ChallengeResult();
                return;
            }

            var identity = principal.Identity as ClaimsIdentity;
            if (identity?.Name == null)
            {
                string[] possibleNames = ["email", "preferred_username", "username", "name"];
                foreach (var name in possibleNames)
                {
                    var claim = identity?.Claims.FirstOrDefault(c => c.Type == name);
                    if (claim != null)
                    {
                        identity?.AddClaim(new(ClaimTypes.Name, claim.Value));
                        break;
                    }
                }
            }
            var hrDemoApiDb = actionContext.HttpContext.RequestServices.GetRequiredService<HRDemoApiContext>();
            // Fetch user data from the database
            var user = hrDemoApiDb.Users
                .Include("Employee")
                .AsNoTracking()
                .FirstOrDefault(u => identity != null && u.Name == identity.Name);

            if (user == null)
            {
                actionContext.Result = HttpUtilities.CreateResponseMessage($"User {identity?.Name} not found in the database", System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            if (user.Employee == null)
            {
                actionContext.Result = HttpUtilities.CreateResponseMessage($"User {identity?.Name} is not mapped to any employee", System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            var role = user?.Role.ToString() ?? UserRole.User.ToString();
            identity?.AddClaim(new Claim("userRole", role));

            identity?.AddClaim(new Claim(identity.RoleClaimType, role));

            var managedDepartments = hrDemoApiDb.Departments.Where(d => d.ManagerID != null && user != null &&  d.ManagerID == user.EmployeeID).Select(d => d.DepartmentID);
            foreach (var departmentId in managedDepartments)
            {
                identity?.AddClaim(new Claim(identity.RoleClaimType, $"Manager-{departmentId}"));
            }

            identity?.AddClaim(new Claim("displayName", $"{user?.Employee.FirstName} {user?.Employee.LastName}"));
        }
    }

}