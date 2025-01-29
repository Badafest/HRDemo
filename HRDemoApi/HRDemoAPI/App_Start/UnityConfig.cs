using HRDemoAPI.Data;
using HRDemoAPI.Filters;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HRDemoAPI
{
    public static class UnityConfig
    {
        public static IUnityContainer container;
        public static void RegisterComponents()
        {
			container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<HRDemoApiDbContainer>();
            container.RegisterType<HRDemoAuthorizeAttribute>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}