using Raven.Rpc.Tracing.ContextData;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Owin
{
    /// <summary>
    /// 
    /// </summary>
    internal class TraceContextHandle : ITraceContextHandle
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void BeginRequest(IDictionary environment)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void EndRequest(IDictionary environment)
        {
        }
    }
}
