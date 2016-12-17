using Raven.Rpc.Tracing.ContextData;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.NoContext
{
    /// <summary>
    /// 
    /// </summary>
    public static class TracingConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracingRecord"></param>
        /// <param name="systemID"></param>
        /// <param name="systemName"></param>
        /// <param name="environment"></param>
        public static void UseTracingContext(ITracingRecord tracingRecord, string systemID = null, string systemName = null, string environment = null)
        {
            ServiceContainer.Register<ITracingContextHelper>(new ContextHelper());
            ServiceContainer.Register<ITracingRecord>(tracingRecord);
            ServiceContainer.Register<IInitRequestScopeContext>(new InitRequestScopeContext());

            EnvironmentConfig.SystemID = systemID;
            EnvironmentConfig.SystemName = systemName;
            EnvironmentConfig.Environment = environment;
        }

    }
}
