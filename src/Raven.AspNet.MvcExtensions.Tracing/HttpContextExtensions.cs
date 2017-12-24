using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    public static class HttpContextExtensions
    {
        public static ITracingContext GetTracingContext(this HttpContext request)
        {
            return GetContextHelper(request.Items);
        }

        public static ITracingContext GetTracingContext(this HttpContextBase request)
        {
            return GetContextHelper(request.Items);
        }

        private static ITracingContext GetContextHelper(IDictionary items)
        {
            var context = TracingContext.GetContext(items);
            return context;
        }
    }
}
