using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Context;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    public static class HttpControllerHandler
    {
        private const string TracingHttpControllerActivatorHandler = "TracingHttpControllerActivatorHandler";

        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            Action<IHttpController, HttpRequestMessage, HttpControllerDescriptor, Type> handler = (httpController, request, controllerDescriptor, controllerType) =>
            {
                if (httpController is ITracingApiController tracingController)
                {
                    var context = TracingContext.GetContext(request.Properties);
                    tracingController.TracingContextHelper = new TracingContextHelper(new HttpContextHelper(context));
                }
            };

            Dispatchers.HttpControllerActivatorHandler.Register(TracingHttpControllerActivatorHandler, handler);
        }
    }
}
