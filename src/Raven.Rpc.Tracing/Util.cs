
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    public class Util
    {
        /// <summary>
        /// 获取32位唯一字符串
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueCode32()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var token1 = Convert.ToBase64String(guid1.ToByteArray()).TrimEnd('=').Replace("/", "_").Replace("+", "-");
            var token2 = Convert.ToBase64String(guid2.ToByteArray()).TrimEnd('=').Replace("/", "_").Replace("+", "-");
            return (token1 + token2).Substring(0, 32);
        }

        /// <summary>
        /// 版本号自增（eg:1.4.3）
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string VersionIncr(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "1";

            var i = val.LastIndexOf('.');
            if (i >= 0)
            {
                var ver = val.Substring(i + 1);
                if (string.IsNullOrEmpty(ver))
                {
                    val = val.Substring(0, i + 1) + "1";
                }
                else
                {
                    val = val.Substring(0, i + 1) + (int.Parse(ver) + 1);
                }
                return val;
            }
            else
            {
                return (int.Parse(val) + 1).ToString();
            }
        }
    }
}
