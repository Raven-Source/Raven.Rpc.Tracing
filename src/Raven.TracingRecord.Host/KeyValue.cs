using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven.TracingRecord.Host
{
    /// <summary>
    /// 键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValue<TKey, TValue>
    {
        private TKey _Key;
        private TValue _Value;

        /// <summary>键名
        /// 键名
        /// </summary>
        public TKey Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        /// <summary>键值
        /// 键值
        /// </summary>
        public TValue Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

    }
}
