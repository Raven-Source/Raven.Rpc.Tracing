using Raven.Rpc.Tracing.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Raven.Rpc.Tracing;
using System.Collections.Specialized;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpContextHelper : IRequestContextHelper
    {
        ITracingContext context;
        public HttpContextHelper(ITracingContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 获取 HttpContextItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetContextItem<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default(T);

            object obj;

            if (context.Items != null && context.Items.TryGetValue(key, out obj) && obj != null)
            {
                return (T)obj;
            }
            else
            {
                return default(T);
            }
            //var obj = OwinRequestScopeContext.Current.Environment[key];
            //if (obj == null)
            //{
            //    return default(T);
            //}
            //return (T)obj;
        }

        /// <summary>
        /// 设置 HttpContextItem
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetContextItem( string key, object val)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            

            if (context.Items == null)
                return;

            context.Items[key] = val;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public string GetServerAddress()
        //{
        //    string res = string.Empty;
        //    //is owin
        //    //if (System.Web.HttpContext.Current == null)
        //    //{
        //    //    var environment = OwinRequestScopeContext.Current.Environment;
        //    //    res = string.Concat(environment["server.LocalIpAddress"], ":", environment["server.LocalPort"]);
        //    //}
        //    //else
        //    //{
        //    //    return System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        //    //}

        //    //return System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        //    //var environment = System.Web.HttpContext.Current.Request.ServerVariables;

        //    //if (RequestScopeContext.Current != null)
        //    //{
        //    //    var environment = RequestScopeContext.Current.Environment as NameValueCollection;
        //    //    if (environment != null)
        //    //    {
        //    //        //res = environment["LOCAL_ADDR"] + System.Web.HttpContext.Current.Request.Url.Port;
        //    //        res = string.Concat(environment["LOCAL_ADDR"], ":", environment["SERVER_PORT"]);
        //    //    }
        //    //}

        //    return res;
        //}
    }
}
