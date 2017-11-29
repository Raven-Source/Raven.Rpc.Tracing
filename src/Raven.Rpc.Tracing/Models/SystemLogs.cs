using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class SystemLogs
    {
        /// <summary>
        /// TraceId
        /// </summary>
        public string TraceId;

        /// <summary>
        /// TotalMilliseconds
        /// </summary>
        public double TimeLength;

        /// <summary>
        /// 是否运行异常,true|异常,false|未异常
        /// </summary>
        public bool IsException;

        /// <summary>
        /// 级别
        /// </summary>
        public LogsLevel Level;

        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemID;

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName;

        /// <summary>
        /// 环境
        /// </summary>
        public string Environment;

        /// <summary>
        /// 调用ID
        /// </summary>
        public string InvokeID;

        /// <summary>
        /// server host
        /// </summary>
        public string ServerHost;

        /// <summary>
        /// 状态值
        /// </summary>
        public string Code;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content;

        /// <summary>
        /// 调用堆栈
        /// </summary>
        public string StackTrace;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime;

        /// <summary>
        /// SearchKey
        /// </summary>
        public string SearchKey;

        /// <summary>
        /// 
        /// </summary>
        public SystemLogs()
        {
            CreateTime = DateTime.Now;
            Level = LogsLevel.L1;
        }

    }
}
