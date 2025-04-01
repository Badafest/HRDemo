using HRDemoAPI.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace HRDemoAPI.Utilities
{
    public static class HttpUtilities
    {
        public static HttpResponseMessage CreateResponseMessage<T>(this T content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            StringContent responseContent = null;
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            if (content != null)
            {
                var jsonString = JsonConvert.SerializeObject(content, Formatting.None, jsonFormatter.SerializerSettings);
                responseContent = new StringContent(jsonString);
                responseContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = responseContent
            };
        }


        public static HttpResponseMessage CreateResponseMessage(string message, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var responseObject = new
            {
                Message = message,
            };
            return responseObject.CreateResponseMessage(statusCode);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> filterQuery, int queryCount, int queryPage)
        {
            int totalCount = filterQuery.Count();
            int count = queryCount == default ? totalCount : queryCount;
            int page = queryPage == default ? 1 : queryPage;
            HttpContext.Current?.Response?.Headers?.Add("X-Total-Count", totalCount.ToString());
            HttpContext.Current?.Response?.Headers?.Add("X-Total-Pages", Math.Ceiling((double)totalCount / count).ToString());
            HttpContext.Current?.Response?.Headers?.Add("X-Current-Page", page.ToString());
            return filterQuery.Skip(((page) - 1) * count).Take(count);
        }

        public static ISet<int> GetManagedDepartments()
        {
            var user = HttpContext.Current.User as ClaimsIdentity;
            if (ValidateAdminRole() == null)
            {
                return new HashSet<int>();
            }
            var managedDepartments = user.Claims
                .Where(claim => claim.Type == ClaimTypes.Role && claim.Value.Length > 8)
                .Select(claim => int.Parse(claim.Value.Substring(8)))
                .ToHashSet();
            if (managedDepartments.Count == 0)
            {
                managedDepartments.Add(0);
            }
            return managedDepartments;
        }

        public static HttpResponseMessage ValidateAdminRole()
        {
            if (HttpContext.Current.User.IsInRole(UserRole.Admin.ToString()))
            {
                return null;
            }
            return CreateResponseMessage("You are not an admin", HttpStatusCode.Unauthorized);
        }

        public static HttpResponseMessage ValidateManagerRole(int? departmentID)
        {
            if (departmentID != null && departmentID != default && HttpContext.Current.User.IsInRole($"Manager-{departmentID}"))
            {
                return null;
            }
            var adminResponse = ValidateAdminRole();
            if (adminResponse == null)
            {
                return null;
            }
            return CreateResponseMessage("You are not a manager or an admin", HttpStatusCode.Unauthorized);
        }

        public static HttpResponseMessage ValidateManagerRole(int?[] departmentIDs)
        {
            foreach (var departmentID in departmentIDs)
            {
                var response = ValidateManagerRole(departmentID);
                if(response != null)
                {
                    return response;
                }
            }
            return null;
        }
    }
}