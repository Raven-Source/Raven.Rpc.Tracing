using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class FilterContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <returns></returns>
        public static bool HasMarkerAttribute<T>(this ActionExecutingContext that)
        {
            return that.ActionDescriptor.GetFilterAttributes(true).FirstOrDefault(x => x is T) != null
                || that.ActionDescriptor.ControllerDescriptor.GetFilterAttributes(true).FirstOrDefault(x => x is T) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <returns></returns>
        public static bool HasMarkerAttribute<T>(this ActionExecutedContext that)
        {
            return that.ActionDescriptor.GetFilterAttributes(true).FirstOrDefault(x => x is T) != null
                || that.ActionDescriptor.ControllerDescriptor.GetFilterAttributes(true).FirstOrDefault(x => x is T) != null;
        }
    }
}
