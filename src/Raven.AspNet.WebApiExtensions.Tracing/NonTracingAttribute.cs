using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    /// <summary>
    /// NonTracing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NonTracingAttribute : ActionFilterAttribute
    {
    }

    /// <summary>
    /// Not To Log
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NotToLogAttribute : ActionFilterAttribute
    {
    }
}
