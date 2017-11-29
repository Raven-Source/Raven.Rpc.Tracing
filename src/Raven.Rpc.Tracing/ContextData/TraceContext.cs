using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.ContextData
{
    /// <summary>
    /// 
    /// </summary>
    public class TraceContext : ITraceContext
    {
        /// <summary>
        /// 
        /// </summary>
        public const string TRACE_CONTEXT_KEY = "raven_context";

        /// <summary>
        /// 
        /// </summary>
        public const string SERVER_ADDRESS_KEY = "server_address";

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Items { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public TraceContext()
        {
            Items = new Dictionary<string, object>();
            Timestamp = DateTime.Now;
        }
        
    }
}
