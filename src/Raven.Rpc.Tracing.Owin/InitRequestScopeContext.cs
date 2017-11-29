using Raven.Rpc.Tracing.ContextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Owin
{
    /// <summary>
    /// 
    /// </summary>
    internal class InitRequestScopeContext : IInitRequestScopeContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void BeginRequest(object context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void EndRequest(object context)
        {
        }
    }
}
