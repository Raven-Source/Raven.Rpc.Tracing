using Raven.Rpc.Tracing.ContextData;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace Raven.Rpc.Tracing.WebHost
{
    /// <summary>
    /// 
    /// </summary>
    public static class TracingConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracingRecord"></param>
        public static void UseTracingContext(ITracingRecord tracingRecord)
        {
            ServiceContainer.Register<IHttpContextHelper>(new HttpContextHelper());
            ServiceContainer.Register<ITracingRecord>(tracingRecord);
            ServiceContainer.Register<IInitRequestScopeContext>(new InitRequestScopeContext());

            Util.SetThreadPool();
        }

    }
}
