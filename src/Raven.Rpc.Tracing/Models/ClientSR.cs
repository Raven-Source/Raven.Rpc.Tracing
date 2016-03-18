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
        public string TraceId;

        /// <summary>
        /// 远程服务ID
        /// </summary>
        public string RpcId;

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string MachineAddr;

        /// <summary>
        /// 请求开始时间
        /// </summary>
        public DateTime SendSTime;

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime? ReceiveETime;

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ExceptionTime;

        /// <summary>
        /// TotalMilliseconds
        /// </summary>
        public double TimeLength;

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
        ///// 服务方法/url
        ///// </summary>
        //public string ServiceMethod;

        /// <summary>
        /// 调用服务方法/url
        /// </summary>
        public string InvokeID;

        /// <summary>
        /// server host
        /// </summary>
        public string ServerHost;

        /// <summary>
        /// 协议
        /// </summary>
        public string Protocol;

        /// <summary>
        /// 扩展
        /// </summary>
        public virtual Dictionary<string, object> Extension
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ClientSR()
        {
            Extension = new Dictionary<string, object>();
        }

    }
}
