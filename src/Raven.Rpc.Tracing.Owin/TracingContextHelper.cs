using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Raven.Rpc.Tracing;
using Owin;
using Raven.Rpc.Tracing.ContextData;
using System.Threading;

namespace Raven.Rpc.Tracing.Owin
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingContextHelper : ITracingContextHelper
    {
        /// <summary>
        /// 获取 HttpContextItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetContextItem<T>(ITraceContext traceContext, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default(T);

            object obj;
            if (traceContext != null && traceContext.Items.TryGetValue(key, out obj) && obj != null)
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
        public void SetContextItem(ITraceContext traceContext, string key, object val)
        {
            if (string.IsNullOrWhiteSpace(key) || traceContext == null)
                return;

            traceContext.Items[key] = val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetServerAddress(ITraceContext traceContext)
        {
            return GetContextItem<string>(traceContext, TraceContext.SERVER_ADDRESS_KEY);
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

        //    if (RequestScopeContext.Current != null)
        //    {
        //        var environment = RequestScopeContext.Current.Environment as IDictionary<string, object>;
        //        if (environment != null)
        //        {
        //            object ipAddr = null, localPort = null ;
        //            environment.TryGetValue("server.LocalIpAddress", out ipAddr);
        //            environment.TryGetValue("server.LocalPort", out ipAddr);
        //            res = string.Format($"{ipAddr}:{localPort}");
        //        }
        //    }

        //    return res;
        //}
    }
}
