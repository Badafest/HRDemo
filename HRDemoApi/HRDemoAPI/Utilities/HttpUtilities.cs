﻿using HRDemoAPI.Data;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace HRDemoApp.Utilities
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

        public static IQueryable<T> Paginate<T>(this IQueryable<T> filterQuery, int count, int page)
        {
            int totalCount = filterQuery.Count();
            HttpContext.Current?.Response?.Headers?.Add("X-Total-Count", totalCount.ToString());
            HttpContext.Current?.Response?.Headers?.Add("X-Total-Pages", Math.Floor((double)totalCount/count + 1).ToString());
            HttpContext.Current?.Response?.Headers?.Add("X-Current-Page", page.ToString());
            return filterQuery.Skip((page - 1) * count).Take(count);
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
            if (departmentID != null && departmentID != default && HttpContext.Current.User.IsInRole("Manager-${departmentID}"))
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