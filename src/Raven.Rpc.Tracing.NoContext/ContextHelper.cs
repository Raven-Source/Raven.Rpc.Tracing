using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.NoContext
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextHelper : ITracingContextHelper
    {
        /// <summary>
        /// 获取 HttpContextItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetContextItem<T>(string key)
        {
            return default(T);
        }

        /// <summary>
        /// 设置 HttpContextItem
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetContextItem(string key, object val)
        {
            ;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetServerAddress()
        {
            return string.Empty;
        }
    }

}
