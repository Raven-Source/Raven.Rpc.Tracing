using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class Config
    {
        //public const string TraceClientSRQueueName = "raven_trace_csr";
        //public const string TraceServerRSQueueName = "raven_trace_srs";
        /// <summary>
        /// 
        /// </summary>
        public const string TraceLogsQueueName = "raven_trace_logs";

        /// <summary>
        /// 
        /// </summary>
        public const string ExceptionKey = "Exception";
        /// <summary>
        /// 
        /// </summary>
        public const string ParamsKey = "Params";

        /// <summary>
        /// 
        /// </summary>
        public const string FormKey = "Form";

        /// <summary>
        /// 
        /// </summary>
        public const string ResultKey = "Result";


        /// <summary>
        /// 
        /// </summary>
        public const string ServerRSKey = "__raven_ServerRS";

        /// <summary>
        /// 
        /// </summary>
        public const string ResponseHeaderTraceKey = "R-TraceId";

        /// <summary>
        /// 
        /// </summary>
        public const string ResponseHeaderFolderKey = "R-RootFolder";
    }
}
