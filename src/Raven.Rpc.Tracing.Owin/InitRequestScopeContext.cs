using Raven.Rpc.Tracing.ContextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Owin
{
    internal class InitRequestScopeContext : IInitRequestScopeContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="tracingRecord"></param>
        public void BeginRequest(object context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="tracingRecord"></param>
        public void EndRequest(object context)
        {
        }
    }
}
