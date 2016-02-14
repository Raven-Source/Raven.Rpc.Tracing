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
    public class ServerRS
    {
        /// <summary>
        /// TraceId
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// RpcId
        /// </summary>
        public string RpcId { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string MachineIP { get; set; }

        /// <summary>
        /// 请求开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 是否运行成功,true|成功,false|失败
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 是否运行异常,true|异常,false|未异常
        /// </summary>
        public bool IsException { get; set; }

        /// <summary>
        /// 返回状态值
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public byte[] SendContent { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 扩展
        /// </summary>
        public KeyValue<string, object>[] Extension { get; set; }
    }
}
