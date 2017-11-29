using Raven.Rpc.Tracing.ContextData;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.WebHost
{
    internal class TraceContextHandle : ITraceContextHandle
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void BeginRequest(IDictionary environment)
        {
            //var scopeContext = new RequestScopeContext(environment);
            //RequestScopeContext.Current = scopeContext;

            var context = new TraceContext();
            //var environment = System.Web.HttpContext.Current.Request.ServerVariables;

            //res = environment["LOCAL_ADDR"] + System.Web.HttpContext.Current.Request.Url.Port;
            context.Items[TraceContext.SERVER_ADDRESS_KEY] = string.Concat(environment["LOCAL_ADDR"], ":", environment["SERVER_PORT"]);
            environment[TraceContext.TRACE_CONTEXT_KEY] = context;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void EndRequest(IDictionary environment)
        {
            environment?.Remove(TraceContext.TRACE_CONTEXT_KEY);
        }
    }
}
