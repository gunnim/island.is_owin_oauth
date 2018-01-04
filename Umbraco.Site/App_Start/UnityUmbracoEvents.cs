using Common.Logging;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.HealthCheck;
using Umbraco.Web.Trees;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using Unity.Lifetime;
using Unity.RegistrationByConvention;

namespace Umbraco.Site.App_Start
{
    class UnityEvents : IApplicationEventHandler
    {
        public void OnApplicationStarted(
            UmbracoApplicationBase httpApplication,
            ApplicationContext applicationContext
        )
        {
            var container = UnityConfig.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            GlobalConfiguration.Configure(config =>
            {
                //config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());
                config.DependencyResolver = new Unity.AspNet.WebApi.UnityDependencyResolver(container);
            });
            DependencyResolver.SetResolver(new Unity.AspNet.Mvc.UnityDependencyResolver(container));

            container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(UnityEvents).Assembly),
                WithMappings.FromMatchingInterface,
                WithName.Default
            );

            container.RegisterType<ApplicationContext>(new InjectionFactory(c => ApplicationContext.Current));

            container.RegisterType<HttpContextBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpContextWrapper(HttpContext.Current)));
            container.RegisterType<UmbracoContext>(new PerRequestLifetimeManager(), new InjectionFactory(c => UmbracoContext.Current));
            container.RegisterType<UmbracoHelper>(new PerRequestLifetimeManager(), new InjectionConstructor(typeof(UmbracoContext)));
            container.RegisterType<HealthCheckController>(new InjectionConstructor());
            container.RegisterType<UsersController>(new InjectionConstructor());
            container.RegisterType<LegacyTreeController>(new InjectionConstructor());
            container.RegisterType<ILogManager, LogManager>(new ContainerControlledLifetimeManager());
        }

        public void OnApplicationInitialized(UmbracoApplicationBase httpApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarting(UmbracoApplicationBase httpApplication, ApplicationContext applicationContext)
        {
        }
    }
}
