using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    public static class Config
    {
        //public const string TraceClientSRQueueName = "raven_trace_csr";
        //public const string TraceServerRSQueueName = "raven_trace_srs";
        public const string TraceLogsQueueName = "raven_trace_logs";

        public const string ExceptionKey = "Exception";
        public const string ParamsKey = "Params";
        public const string ResultKey = "Result";


        public const string ServerRSKey = "__raven_ServerRS";
        public const string ResponseHeaderTraceKey = "RavenTraceId";
    }
}
