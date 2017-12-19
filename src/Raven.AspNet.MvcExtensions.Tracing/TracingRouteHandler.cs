using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingRouteHandler : IRouteHandler
    {
        private MvcRouteHandler handler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        public TracingRouteHandler(MvcRouteHandler handler)
        {
            this.handler = handler;
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            IHttpHandler httpHandler = ((IRouteHandler)handler).GetHttpHandler(requestContext);
            if (httpHandler is MvcHandler mvcHandler)
            {
                return new TracingMvcHandler(mvcHandler);
            }
            else
            {
                return httpHandler;
            }
        }
    }
}
