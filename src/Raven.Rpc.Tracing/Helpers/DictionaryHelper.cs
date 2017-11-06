using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static List<KeyValue<TKey, TValue>> GetKeyValueList<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            if (dict == null) return null;

            List<KeyValue<TKey, TValue>> kvList = new List<KeyValue<TKey, TValue>>();
            foreach (var kv in dict)
            {
                kvList.Add(new KeyValue<TKey, TValue>(kv.Key, kv.Value));
            }
            return kvList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue<TKey>(this IDictionary dict, TKey key)
        {
            if (dict.Contains(key))
            {
                return dict[key];
            }
            else
            {
                return null;
            }
        }
    }
}
