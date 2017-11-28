using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Raven.Rpc.Tracing;
using Owin;
using Raven.Rpc.Tracing.ContextData;

namespace Raven.Rpc.Tracing.Owin
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpContextHelper : ITracingContextHelper
    {
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
            IRequestScopeContext context = RequestScopeContext.GetCurrent();
            if (context != null && context.Items.TryGetValue(key, out obj) && obj != null)
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
        public void SetContextItem(string key, object val)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;


            IRequestScopeContext context = RequestScopeContext.GetCurrent();

            if (context == null)
                return;

            context.Items[key] = val;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetServerAddress()
        {
            string res = string.Empty;

            //if (RequestScopeContext.Current != null)
            //{
            //    var environment = RequestScopeContext.Current.Environment as IDictionary<string, object>;
            //    if (environment != null)
            //    {
            //        res = string.Concat(environment.GetValue("server.LocalIpAddress"), ":", environment.GetValue("server.LocalPort"));
            //    }
            //}

            return res;
        }
    }
}
