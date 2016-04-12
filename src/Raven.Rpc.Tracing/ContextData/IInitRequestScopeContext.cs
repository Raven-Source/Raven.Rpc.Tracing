using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.ContextData
{
    internal interface IInitRequestScopeContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="tracingRecord"></param>
        void BeginRequest(object context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="tracingRecord"></param>
        void EndRequest(object context);
    }
}
