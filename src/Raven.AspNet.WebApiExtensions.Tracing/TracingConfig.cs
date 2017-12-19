using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Record;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Raven.AspNet.WebApiExtensions.Dispatchers;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    public static class TracingConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="tracingRecord"></param>
        /// <param name="systemID"></param>
        /// <param name="systemName"></param>
        /// <param name="environment"></param>
        public static void UseTracing(this HttpConfiguration config, ITracingRecord tracingRecord, string systemID = null, string systemName = null, string environment = null)
        {
            HttpControllerHandler.Register();

            //ServiceContainer.Register<ITracingContextHelper>(new HttpContextHelper());
            ServiceContainer.Register<ITracingRecord>(tracingRecord);
            config.UseHandlerHttpControllerActivator();
            config.MessageHandlers.Add(new TracingDelegatingHandler());
            //ServiceContainer.Register<IInitRequestScopeContext>(new InitRequestScopeContext());

            EnvironmentConfig.SystemID = systemID;
            EnvironmentConfig.SystemName = systemName;
            EnvironmentConfig.Environment = environment;
            Raven.Rpc.Tracing.Util.SetThreadPool();
        }
    }
}
