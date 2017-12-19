using Raven.Rpc.Tracing.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    public class TracingDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tracingContext = new TracingContext();
            TracingContext.InitCurrent(request.Properties, tracingContext);
            //return base.SendAsync(request, cancellationToken);
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                TracingContext.FreeContext(request.Properties);
            }
        }
    }
}
