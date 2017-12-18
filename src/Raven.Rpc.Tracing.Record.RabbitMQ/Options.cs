using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public class Options
    {
        /// <summary>
        /// 默认最大队列数
        /// </summary>
        public const int DefaultMaxQueueCount = 500000;

        /// <summary>
        /// 
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 端口，默认5672
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// 最大内存队列数,默认100000
        /// </summary>
        public int MaxQueueCount { get; set; }

        /// <summary>
        /// 数据格式化方式，对应SerializeType
        /// </summary>
        public SerializerType SerializerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ILoger Loger { get; set; }

        ///// <summary>
        ///// 默认2000
        ///// </summary>
        //public int WaitMillisecondsTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public short WriteWorkerTaskNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Options()
        {
            MaxQueueCount = DefaultMaxQueueCount;
            SerializerType = SerializerType.NewtonsoftJson;
            WriteWorkerTaskNumber = 4;
        }
    }
}
