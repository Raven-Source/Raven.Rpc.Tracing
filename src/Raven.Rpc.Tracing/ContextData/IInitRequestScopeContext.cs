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
    internal interface IInitRequestScopeContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void BeginRequest(object context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void EndRequest(object context);
    }
}
