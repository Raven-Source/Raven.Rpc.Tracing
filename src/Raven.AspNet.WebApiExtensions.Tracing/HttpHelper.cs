using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Raven.Rpc.Tracing;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    public class HttpHelper : IHttpHelper
    {
        /// <summary>
        /// 获取 HttpContextItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetHttpContextItem<T>(HttpRequestMessage request, string key)
        {
            if (request == null || string.IsNullOrWhiteSpace(key))
                return default(T);

            //is owin
            if (System.Web.HttpContext.Current == null)
            {
                var context = request.GetOwinContext();
                return context.Get<T>(key);
            }
            else
            {
                var obj = System.Web.HttpContext.Current.Items[key];
                if (obj == null)
                {
                    return default(T);
                }
                return (T)obj;
            }
        }

        /// <summary>
        /// 设置 HttpContextItem
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetHttpContextItem(HttpRequestMessage request, string key, object val)
        {
            if (request == null || string.IsNullOrWhiteSpace(key))
                return;

            //is owin
            if (System.Web.HttpContext.Current == null)
            {
                var context = request.GetOwinContext();
                context.Set(key, val);
            }
            else
            {
                System.Web.HttpContext.Current.Items[key] = val;
            }
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
            if (System.Web.HttpContext.Current == null)
            {
                var environment = request.GetOwinEnvironment();
                res = string.Concat(environment["server.LocalIpAddress"].ToString(), ":", environment["server.LocalPort"].ToString());
            }
            else
            {
                return System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            }

            return res;
        }
    }
}
