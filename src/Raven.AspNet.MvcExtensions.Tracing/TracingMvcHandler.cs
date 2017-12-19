using Raven.Rpc.Tracing.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingMvcHandler : IHttpAsyncHandler, IHttpHandler, IRequiresSessionState
    {
        MvcHandler handler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        public TracingMvcHandler(MvcHandler handler)
        {
            this.handler = handler;
        }

        bool IHttpHandler.IsReusable
        {
            get
            {
                return ((IHttpHandler)handler).IsReusable;
            }
        }

        IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var tracingContext = new TracingContext();
            TracingContext.InitCurrent(context.Items, tracingContext);
            if (extraData == null)
            {
                extraData = context;
            }
            try
            {
                return ((IHttpAsyncHandler)handler).BeginProcessRequest(context, cb, extraData);
            }
            finally
            { }
        }

        void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result)
        {
            if (result.AsyncState is HttpContext context)
            {
                TracingContext.FreeContext(context.Items);
            }
            try
            {
                ((IHttpAsyncHandler)handler).EndProcessRequest(result);
            }
            finally
            { }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var tracingContext = new TracingContext();
            TracingContext.InitCurrent(context.Items, tracingContext);
            try
            {
                ((IHttpHandler)handler).ProcessRequest(context);
            }
            finally
            {
                TracingContext.FreeContext(context.Items);
            }
        }
    }

}
