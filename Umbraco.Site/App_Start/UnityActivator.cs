using Unity.AspNet.Mvc;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Umbraco.Site.App_Start.UnityActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Umbraco.Site.App_Start.UnityActivator), "Shutdown")]

namespace Umbraco.Site.App_Start
{
    /// <summary>Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET</summary>
    static class UnityActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}
