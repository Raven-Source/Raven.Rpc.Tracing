using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.WebHost
{
    public class HttpHelper : IHttpHelper
    {/// <summary>
     /// 获取 HttpContextItem
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="request"></param>
     /// <param name="key"></param>
     /// <returns></returns>
        public T GetHttpContextItem<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default(T);

            object obj;
            obj = System.Web.HttpContext.Current.Items[key];
            if (obj != null)
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
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetHttpContextItem(string key, object val)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            System.Web.HttpContext.Current.Items[key] = val;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetServerAddress(HttpRequestMessage request)
        {
            string res = string.Empty;
            //is owin
            //if (System.Web.HttpContext.Current == null)
            //{
            //    var environment = OwinRequestScopeContext.Current.Environment;
            //    res = string.Concat(environment["server.LocalIpAddress"], ":", environment["server.LocalPort"]);
            //}
            //else
            //{
            //    return System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            //}

            //return System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            var environment = System.Web.HttpContext.Current.Request.ServerVariables;
            res = environment["LOCAL_ADDR"];

            return res;
        }
    }
}
