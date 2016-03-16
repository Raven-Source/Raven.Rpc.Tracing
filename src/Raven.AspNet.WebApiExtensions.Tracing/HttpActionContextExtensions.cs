using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    public static class HttpActionContextExtensions
    {
        public static bool HasMarkerAttribute<T>(this HttpActionContext that)
            where T : class
        {
            return that.ActionDescriptor.GetCustomAttributes<T>().Count > 0
                || that.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<T>().Count > 0;
        }
        
    }
}
