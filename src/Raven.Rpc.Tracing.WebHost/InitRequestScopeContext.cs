using Raven.Rpc.Tracing.ContextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.WebHost
{
    internal class InitRequestScopeContext : IInitRequestScopeContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void BeginRequest(object context)
        {
            var environment = System.Web.HttpContext.Current.Request.ServerVariables;
            var scopeContext = new RequestScopeContext(environment);
            RequestScopeContext.Current = scopeContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void EndRequest(object context)
        {
            RequestScopeContext.FreeContextSlot();
        }
    }
}
