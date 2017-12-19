using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    public static class ControllerHandler
    {
        private const string TracingControllerFactoryHandler = "TracingControllerFactoryHandler";

        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            Action<IController, RequestContext, string> handler = (controller, requestContext, controllerName) =>
            {
                if (controller is ITracingController tracingController)
                {
                    var context = TracingContext.GetContext(requestContext.HttpContext.Items);
                    tracingController.TracingContextHelper = new TracingContextHelper(new HttpContextHelper(context));
                }
            };

            Factorys.ControllerFactoryHandler.Register(TracingControllerFactoryHandler, handler);
        }
    }
}
