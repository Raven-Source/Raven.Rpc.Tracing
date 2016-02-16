using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Raven.Rpc.Tracing;
using Owin;

namespace Raven.Rpc.Tracing.Owin
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
        public T GetHttpContextItem<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default(T);

            var obj = OwinRequestScopeContext.Current.Environment[key];
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }

        /// <summary>
        /// 设置 HttpContextItem
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetHttpContextItem(string key, object val)
        {
            if (request == null || string.IsNullOrWhiteSpace(key))
                return;
            
            OwinRequestScopeContext.Current.Environment[key] = val;
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
