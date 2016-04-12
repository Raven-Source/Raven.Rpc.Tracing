using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Owin
{
    using ContextData;
    using AppFunc = Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class RequestScopeContextMiddleware
    {
        readonly AppFunc next;
        
        public RequestScopeContextMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var scopeContext = new RequestScopeContext(environment);
            RequestScopeContext.Current = scopeContext;

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
