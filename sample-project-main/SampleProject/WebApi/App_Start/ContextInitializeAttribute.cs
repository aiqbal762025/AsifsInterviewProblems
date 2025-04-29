using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using SimpleInjector;


namespace WebApi.App_Start
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContextInitializeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //var container = GlobalConfiguration.Configuration.DependencyResolver;
            var container = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(Container)) as Container;

            var method = actionExecutedContext.Request.Method;
            if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete)
            {
                var session = container.GetInstance<IDocumentSession>();
                session.SaveChanges();
            }
        }
    }
}