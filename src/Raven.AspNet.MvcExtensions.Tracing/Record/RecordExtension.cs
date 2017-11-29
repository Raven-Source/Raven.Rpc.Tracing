using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class RecordExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="request"></param>
        public static void RecordTraceLog(this ITracingRecord record, HttpRequestMessage request)
        {
            record.RecordTraceLog(new Rpc.Tracing.TraceLogs());
            //new TracingRecordRabbitmq(null, null, null).RecordTraceLog2(request);
        }
    }
}
