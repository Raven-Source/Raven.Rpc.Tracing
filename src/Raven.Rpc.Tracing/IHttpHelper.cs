using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    public interface IHttpHelper
    {
        /// <summary>
        /// 获取 HttpContextItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetHttpContextItem<T>(string key);


        /// <summary>
        /// 设置 HttpContextItem
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        void SetHttpContextItem(string key, object val);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        string GetServerAddress();
        
    }
}
