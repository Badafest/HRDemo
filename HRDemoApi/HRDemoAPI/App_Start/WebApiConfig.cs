using HRDemoAPI.Filters;
using HRDemoAPI.Filters;
using Newtonsoft.Json.Converters;
using System.Web.Http;

namespace HRDemoAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            config.BindParameter(typeof(string), new NullToEmptyStringModelBinder());

            config.Filters.Add(new ValidateModelAttribute());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
