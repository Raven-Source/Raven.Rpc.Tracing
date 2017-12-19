using Raven.Rpc.Tracing.ContextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.WebHost
{
    internal class InitRequestScopeContext : IInitRequestTracingContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void BeginRequest(ITracingContextHelper contextHelper, IDictionary<string, object> environment)
        {
            var environment = System.Web.HttpContext.Current.Request.ServerVariables;
            var scopeContext = new RequestScopeContext(environment);
            RequestScopeContext.InitCurrent(scopeContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void EndRequest(ITracingContextHelper contextHelper, IDictionary<string, object> environment)
        {
            RequestScopeContext.FreeContextSlot();
        }
    }
}
