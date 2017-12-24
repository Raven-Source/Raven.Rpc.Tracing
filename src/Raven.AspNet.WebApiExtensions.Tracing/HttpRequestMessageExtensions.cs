using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Context;
using System.Net.Http;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    public static class HttpRequestMessageExtensions
    {
        public static ITracingContext GetTracingContext(this HttpRequestMessage request)
        {
            var context = TracingContext.GetContext(request.Properties);
            return context;
        }
    }
}
