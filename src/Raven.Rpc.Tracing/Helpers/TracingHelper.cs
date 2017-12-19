using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Helpers
{
    /// <summary>
    /// TracingContextHelper
    /// </summary>
    public static class TracingHelper
    {
        /// <summary>
        /// TraceContext
        /// </summary>
        public static TraceLogs TraceLogs
        {
            get
            {
                return Util.TracingContextHelper.GetContextItem<TraceLogs>(Config.ServerRSKey);
            }
        }

        /// <summary>
        /// TraceId
        /// </summary>
        public static string TraceId
        {
            get
            {
                var trace = Util.TracingContextHelper.GetContextItem<TraceLogs>(Config.ServerRSKey);
                if (trace != null)
                {
                    return trace.TraceId;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// AddTraceLogsExtensions
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool AddTraceLogsExtensions(string key, object val)
        {
            var trace = Util.TracingContextHelper.GetContextItem<TraceLogs>(Config.ServerRSKey);
            if (trace != null)
            {
                //Extensions Max Count
                if (trace.Extensions.Count < 50 && !trace.Extensions.ContainsKey(key))
                {
                    trace.Extensions[key] = val;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// RecordSystemLogs
        /// </summary>
        /// <param name="data"></param>
        public static void RecordSystemLogs(SystemLogs data)
        {
            data.SystemID = data.SystemID ?? EnvironmentConfig.SystemID;
            data.SystemName = data.SystemName ?? EnvironmentConfig.SystemName;
            data.Environment = data.Environment ?? EnvironmentConfig.Environment;
            data.TraceId = data.TraceId ?? TraceId;

            ServiceContainer.Resolve<ITracingRecord>().RecordSystemLogs(data);
        }

    }
}
