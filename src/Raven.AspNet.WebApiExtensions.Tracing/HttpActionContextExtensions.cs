using System.Web.Http.Controllers;
using System.Linq;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpActionContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <returns></returns>
        public static bool HasMarkerAttribute<T>(this HttpActionContext that)
            where T : class
        {
            return that.ActionDescriptor.GetCustomAttributes<T>().Any()
                || that.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<T>().Any();
        }
        
    }
}
