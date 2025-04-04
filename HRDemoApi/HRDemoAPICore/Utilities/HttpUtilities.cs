﻿using HRDemoAPI.DataCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace HRDemoAPICore.Utilities
{
    public static class HttpUtilities
    {
        public static IConfiguration Configuration { get; set; } = null!;
        public static ObjectResult CreateResponseMessage<T>(this T content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ObjectResult(content)
            {
                StatusCode = (int)statusCode,
            };
        }

        public static ObjectResult CreateResponseMessage(string? message, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var responseObject = string.IsNullOrEmpty(message) ? null : new
            {
                Message = message,
            };
            return responseObject.CreateResponseMessage(statusCode);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> filterQuery, int queryCount, int queryPage, HttpContext? context)
        {
            int totalCount = filterQuery.Count();
            int count = queryCount == default ? totalCount : queryCount;
            int page = queryPage == default ? 1 : queryPage;
            context?.Response.Headers.Append("X-Total-Count", totalCount.ToString());
            context?.Response.Headers.Append("X-Total-Pages", Math.Ceiling((double)totalCount / count).ToString());
            context?.Response.Headers.Append("X-Current-Page", page.ToString());
            return filterQuery.Skip((page - 1) * count).Take(count);
        }

        public static ISet<int> GetManagedDepartments(HttpContext context)
        {
            if (ValidateAdminRole(context) == null)
            {
                return new HashSet<int>();
            }
            var managedDepartments = context.User.Claims
                .Where(claim => claim.Type == ClaimTypes.Role && claim.Value.Length > 8)
                .Select(claim => int.Parse(claim.Value.Substring(8)))
                .ToHashSet();
            if (managedDepartments.Count == 0)
            {
                managedDepartments.Add(0);
            }
            return managedDepartments;
        }
        public static ObjectResult? ValidateAdminRole(HttpContext context)
        {
            if (context.User.IsInRole(UserRole.Admin.ToString()))
            {
                return null;
            }
            return CreateResponseMessage("You are not an admin", HttpStatusCode.Unauthorized);
        }

        public static ObjectResult? ValidateManagerRole(HttpContext context, int? departmentID)
        {
            if (departmentID != null && departmentID != default && context.User.IsInRole($"Manager-{departmentID}"))
            {
                return null;
            }
            var adminResponse = ValidateAdminRole(context);
            if (adminResponse == null)
            {
                return null;
            }
            return CreateResponseMessage("You are not a manager or an admin", HttpStatusCode.Unauthorized);
        }

        public static ObjectResult? ValidateManagerRole(HttpContext context, int?[] departmentIDs)
        {
            foreach (var departmentID in departmentIDs)
            {
                var response = ValidateManagerRole(context, departmentID);
                if (response != null)
                {
                    return response;
                }
            }
            return null;
        }
    }
}