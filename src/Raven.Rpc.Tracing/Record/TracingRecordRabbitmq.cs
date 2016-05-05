using Raven.MessageQueue.WithRabbitMQ;
using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using RabbitMQOptions = Raven.MessageQueue.WithRabbitMQ.Options;

namespace Raven.Rpc.Tracing.Record
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingRecordRabbitmq : ITracingRecord
    {
        //public RabbitMQClient Instance = RabbitMQClient.GetInstance(new Options()
        //{
        //    SerializerType = SerializerType.MsgPack,
        //    HostName = hostName,
        //    Password = password,
        //    UserName = username,
        //    MaxQueueCount = 100000,
        //    Loger = new Loger()
        //});

        private RabbitMQClient rabbitMQClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="loger"></param>
        public TracingRecordRabbitmq(string host, string userName, string pwd, MessageQueue.ILoger loger = null)
        {
            Raven.MessageQueue.WithRabbitMQ.Options rabbitMQOptions = new Options();
            rabbitMQOptions.SerializerType = SerializerType.Jil;
            rabbitMQOptions.HostName = host;
            rabbitMQOptions.UserName = userName;
            rabbitMQOptions.Password = pwd;
            rabbitMQOptions.Loger = loger;

            rabbitMQClient = RabbitMQClient.GetInstance(rabbitMQOptions);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        //public void RecordClientSR(ClientSR data)
        //{
        //    rabbitMQClient.Send(Config.TraceClientSRQueueName, data, true, true);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        //public void RecordServerRS(ServerRS data)
        //{
        //    rabbitMQClient.Send(Config.TraceServerRSQueueName, data, true, true);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void RecordTraceLog(TraceLogs data)
        {
            rabbitMQClient.Send(Config.TraceLogsQueueName, data, true, true);
        }

    }
}
