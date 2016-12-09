using Raven.MessageQueue.WithRabbitMQ;
using Raven.Rpc.Tracing;
using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    public class SystemLogRecordHandle : Handle
    {
        #region GetInstance

        private static Lazy<SystemLogRecordHandle> _instance = new Lazy<SystemLogRecordHandle>(() => new SystemLogRecordHandle("Raven.SystemLogRecordHandle"));

        public static SystemLogRecordHandle GetInstance
        {
            get { return _instance.Value; }
        }

        #endregion

        protected override Action ProcessWorkAction
        {
            get
            {
                return ProcessResetAwardData;
            }
        }

        //ServerRSLogsRep serverRSlogRep;
        //ClientSRLogsRep clientSRlogRep;
        SystemLogsRep sysLogsRep;

        public SystemLogRecordHandle(string serverName)
            : base(serverName, 1000)
        {
            sysLogsRep = new SystemLogsRep();
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        public void ProcessResetAwardData()
        {
            try
            {
                var list = RabbitMQClientManager.GetInstance.rabbitMQClient.ReceiveBatch<Raven.TracingRecord.SystemLogs>(Config.SystemLogsQueueName, noAck: true);
                sysLogsRep.InsertBatch(list);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
