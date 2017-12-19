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
    public class TraceLogs
    {
        /// <summary>
        /// TraceId
        /// </summary>
        public string TraceId;

        /// <summary>
        /// RpcId
        /// </summary>
        public string RpcId;

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string MachineAddr;

        /// <summary>
        /// 请求开始时间
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime EndTime;

        /// <summary>
        /// TotalMilliseconds
        /// </summary>
        public double TimeLength;

        /// <summary>
        /// 是否运行成功,true|成功,false|失败
        /// </summary>
        public bool IsSuccess;

        /// <summary>
        /// 是否运行异常,true|异常,false|未异常
        /// </summary>
        public bool IsException;

        /// <summary>
        /// 返回状态值
        /// </summary>
        public string Code;

        ///// <summary>
        ///// 响应内容
        ///// </summary>
        //public byte[] SendContent;

        ///// <summary>
        ///// 内容类型
        ///// </summary>
        //public string ContentType;

        /// <summary>
        /// 调用ID
        /// </summary>
        public string InvokeID;

        /// <summary>
        /// server host
        /// </summary>
        public string ServerHost;

        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemID;

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName;

        /// <summary>
        /// 协议
        /// </summary>
        public string Protocol;

        /// <summary>
        /// SearchKey
        /// </summary>
        public string SearchKey;

        /// <summary>
        /// 协议头
        /// </summary>
        public virtual Dictionary<string, object> ProtocolHeader
        { get; set; }

        /// <summary>
        /// ContextType
        /// </summary>
        public string ContextType;

        /// <summary>
        /// ResponseSize
        /// </summary>
        public long ResponseSize;

        /// <summary>
        /// 环境
        /// </summary>
        public string Environment;
        
        /// <summary>
        /// 扩展
        /// </summary>
        public virtual Dictionary<string, object> Extensions
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TraceLogs()
        {
            ProtocolHeader = new Dictionary<string, object>();
            Extensions = new Dictionary<string, object>();
        }
    }
}
