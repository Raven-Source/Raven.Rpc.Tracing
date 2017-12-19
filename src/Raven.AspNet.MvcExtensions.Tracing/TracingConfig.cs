using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Raven.AspNet.MvcExtensions.Factorys;
using Raven.Rpc.Tracing;
using System.Web.Routing;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class TracingConfig
    {
        /// <summary>
        /// 必须放在所有Route注册之后
        /// eg. RouteConfig.RegisterRoutes(RouteTable.Routes) 之后
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="routes"></param>
        /// <param name="tracingRecord"></param>
        /// <param name="systemID"></param>
        /// <param name="systemName"></param>
        /// <param name="environment"></param>
        public static void UseTracing(ControllerBuilder builder, RouteCollection routes , ITracingRecord tracingRecord, string systemID = null, string systemName = null, string environment = null)
        {
            ControllerHandler.Register();
            foreach (RouteBase routeBase in routes)
            {
                if (routeBase is Route route)
                {
                    if (route.RouteHandler is MvcRouteHandler mvcRouteHandler)
                    {
                        route.RouteHandler = new TracingRouteHandler(mvcRouteHandler);
                    }
                }
            }
            //ServiceContainer.Register<ITracingContextHelper>(new HttpContextHelper());
            ServiceContainer.Register<ITracingRecord>(tracingRecord);
            //ServiceContainer.Register<IInitRequestScopeContext>(new InitRequestScopeContext());
            builder.UseHandlerControllerFactory();

            EnvironmentConfig.SystemID = systemID;
            EnvironmentConfig.SystemName = systemName;
            EnvironmentConfig.Environment = environment;
            Util.SetThreadPool();
        }
    }
}
