using RabbitMQ.Client;
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
        IDisposable model;

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
                if (model != null)
                {
                    return;
                }

                model = RabbitMQClientManager.GetInstance.rabbitMQClient.RegisterReceive<Raven.TracingRecord.SystemLogs>(Config.SystemLogsQueueName
                    , (l) =>
                    {
                        sysLogsRep.Insert(l);

                    }, noAck: true);


                //var logs = RabbitMQClientManager.GetInstance.rabbitMQClient.ReceiveBatch<Raven.TracingRecord.SystemLogs>(Config.SystemLogsQueueName, noAck: true);
                //if (logs != null && logs.Count > 0)
                //{
                //    int pi = 0;
                //    int ps = 200;
                //    while (pi * ps < logs.Count)
                //    {
                //        var temp = logs.Skip(pi * ps).Take(ps).ToList();
                //        pi++;
                //        sysLogsRep.InsertBatch(temp);
                //    }

                //    //sysLogsRep.InsertBatch(list);
                //}

                model.Dispose();
                model = null;

            }
            catch (Exception ex)
            {
                Loger.GetInstance.LogError(ex, null);
            }
        }

    }
}
