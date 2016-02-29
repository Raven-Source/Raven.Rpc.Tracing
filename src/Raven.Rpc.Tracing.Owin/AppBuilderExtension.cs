using Owin;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQOptions = Raven.MessageQueue.WithRabbitMQ.Options;

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
