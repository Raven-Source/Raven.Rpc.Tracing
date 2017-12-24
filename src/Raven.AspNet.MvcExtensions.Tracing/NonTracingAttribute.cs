using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Raven.AspNet.MvcExtensions.Tracing
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
    public class NotToRecordAttribute : ActionFilterAttribute
    {
    }
}
