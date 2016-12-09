using Owin;
using Raven.Rpc.Tracing.Record;
using Raven.Rpc.Tracing.ContextData;
using System.Threading;

namespace Raven.Rpc.Tracing.Owin
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppBuilderExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="tracingRecord"></param>
        /// <param name="systemID"></param>
        /// <param name="systemName"></param>
        /// <param name="environment"></param>
        public static void UseTracingContext(this IAppBuilder app, ITracingRecord tracingRecord, string systemID = null, string systemName = null, string environment = null)
        {
            app.UseRequestScopeContext();
            ServiceContainer.Register<ITracingContextHelper>(new HttpContextHelper());
            ServiceContainer.Register<ITracingRecord>(tracingRecord);
            ServiceContainer.Register<IInitRequestScopeContext>(new InitRequestScopeContext());

            EnvironmentConfig.SystemID = systemID;
            EnvironmentConfig.SystemName = systemName;
            EnvironmentConfig.Environment = environment;
            Util.SetThreadPool();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app">Owin app.</param>
        /// <returns></returns>
        private static IAppBuilder UseRequestScopeContext(this IAppBuilder app)
        {
            return app.Use(typeof(RequestScopeContextMiddleware));
        }
    }


}
