using Owin;
using Raven.Rpc.Tracing.Record;

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
        public static void UseTracingContext(this IAppBuilder app, ITracingRecord tracingRecord)
        {
            app.UseRequestScopeContext();
            ServiceContainer.Register<IHttpHelper>(new HttpHelper());
            ServiceContainer.Register<ITracingRecord>(tracingRecord);
        }
    }
}
