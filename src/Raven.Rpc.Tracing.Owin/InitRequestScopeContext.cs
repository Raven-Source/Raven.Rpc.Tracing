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
    internal class InitRequestScopeContext : IInitRequestTracingContext
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="context"></param>
        //public void BeginRequest(ITracingContextHelper contextHelper, IDictionary<string, object> environment)
        //{

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="context"></param>
        //public void EndRequest(ITracingContextHelper contextHelper, IDictionary<string, object> environment)
        //{
        //}
        public ITracingContext GetTracingContext(object requestContext)
        {
            if(requestContext is System.Web.Http.Owin.OwinHttpRequestContext )
            return (ITracingContext)environment[TracingContext.CallContextKey];
        }
    }
}
