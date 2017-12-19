using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.ContextData
{
    /// <summary>
    /// 
    /// </summary>
    internal interface ITraceContextHandle
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void BeginRequest(IDictionary environment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void EndRequest(IDictionary environment);
    }
}
