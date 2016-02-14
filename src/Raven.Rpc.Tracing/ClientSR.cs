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
    public class ClientSR
    {
        /// <summary>
        /// 跟踪ID
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// 远程服务ID
        /// </summary>
        public string RpcId { get; set; }

        /// <summary>
        /// 请求开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 服务方法
        /// </summary>
        public string ServiceMethod { get; set; }

        /// <summary>
        /// 扩展
        /// </summary>
        public KeyValue<string, object>[] Extension { get; set; }

    }
}
