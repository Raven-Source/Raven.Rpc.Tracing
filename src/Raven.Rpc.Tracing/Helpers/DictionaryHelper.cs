using System;
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
        public static List<KeyValue<TKey, TValue>> GetKeyValueList<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            if (dict == null) return null;

            List<KeyValue<TKey, TValue>> kvList = new List<KeyValue<TKey, TValue>>();
            foreach (var kv in dict)
            {
                kvList.Add(new KeyValue<TKey, TValue>(kv.Key, kv.Value));
            }
            return kvList;
        }
    }
}
