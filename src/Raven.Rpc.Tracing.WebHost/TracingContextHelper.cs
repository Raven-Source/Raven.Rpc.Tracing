using Raven.Rpc.Tracing.ContextData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Raven.Rpc.Tracing;
using System.Collections.Specialized;

namespace Raven.Rpc.Tracing.WebHost
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
    }
}
