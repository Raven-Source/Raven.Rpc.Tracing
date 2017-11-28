using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Owin
{
    using ContextData;
    using AppFunc = Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    /// <summary>
    /// 
    /// </summary>
    public class RequestScopeContextMiddleware
    {
        readonly AppFunc next;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public RequestScopeContextMiddleware(AppFunc next)
        {
            this.next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public async Task Invoke(IDictionary<string, object> environment)
        {
            var scopeContext = new RequestScopeContext(environment);
            RequestScopeContext.InitCurrent(scopeContext);

            try
            {
                await next(environment);
            }
            finally
            {
                RequestScopeContext.FreeContextSlot();
            }
        }
    }
}
