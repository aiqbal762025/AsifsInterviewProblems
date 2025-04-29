using System.Web.Http;
using System.Web.Http.Filters;
using Common;
using Core;
using Data;
using Data.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using WebApi.App_Start;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();
            var lifestyle = Lifestyle.Scoped;
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var assembly = typeof(WebApiConfig).Assembly;
            InitializeAssemblyInstancesService.Initialize(container, lifestyle, assembly);

            DataConfiguration.Initialize(container, lifestyle);
            CoreConfiguration.Initialize(container, lifestyle);

            container.RegisterWebApiControllers(config);

            // Asif added the following code

            // Initialize RavenDB DocumentStore
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:443" },  // Your RavenDB server URL
                Database = "SampleProject"  // Your database name
            }.Initialize();

            // Register the IDocumentStore as a singleton if it's not already registered
            if (container.GetRegistration(typeof(IDocumentStore)) == null)
            {
                container.Register(() => documentStore, Lifestyle.Singleton);
            }

            // Register IDocumentSession with a scoped lifestyle if it's not already registered
            if (container.GetRegistration(typeof(IDocumentSession)) == null)
            {
                container.Register<IDocumentSession>(() =>
                {
                    var store = container.GetInstance<IDocumentStore>();
                    return store.OpenSession();  // Open session for each request
                }, lifestyle);
            }

            if (container.GetRegistration(typeof(IUserRepository)) == null)
            {
                container.Register<IUserRepository, UserRepository>(Lifestyle.Scoped);
            }

            // Asif added the above code

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = config.DependencyResolver;

            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.DateParseHandling = DateParseHandling.DateTime;
            settings.DefaultValueHandling = DefaultValueHandling.Include;
            settings.Converters.Add(new StringEnumConverter());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                                       "DefaultApi",
                                       "api/{controller}/{id}",
                                       new { id = RouteParameter.Optional }
                                      );

            config.Filters.AddRange(new IFilter[]
                                    {
                                        new ContextInitializeAttribute()
                                    });
        }
    }
}