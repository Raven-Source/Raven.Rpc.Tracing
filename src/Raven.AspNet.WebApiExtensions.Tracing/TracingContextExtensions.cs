using Raven.Rpc.Tracing.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class TracingContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiController"></param>
        /// <returns></returns>
        public static ITracingContext GetTracingContext(this ApiController apiController)
        {
            return TracingContext.GetContext(apiController.Request.Properties);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static ITracingContext GetTracingContext(this HttpRequestMessage requestMessage)
        {
            return TracingContext.GetContext(requestMessage.Properties);
        }
    }
}
