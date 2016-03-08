using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.WebHost
{
    public static class TracingConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void UseTracingContext(ITracingRecord tracingRecord)
        {
            ServiceContainer.Register<IHttpHelper>(new HttpHelper());
            ServiceContainer.Register<ITracingRecord>(tracingRecord);
        }
    }
}
