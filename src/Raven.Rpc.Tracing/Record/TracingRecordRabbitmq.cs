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
        /// <param name="rabbitMQOptions"></param>
        public TracingRecordRabbitmq(Raven.MessageQueue.WithRabbitMQ.Options rabbitMQOptions)
        {
            rabbitMQClient = RabbitMQClient.GetInstance(rabbitMQOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void RecordClientSR(ClientSR data)
        {
            rabbitMQClient.Send(Config.TrackClientSRQueueName, data, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void RecordServerRS(ServerRS data)
        {
            rabbitMQClient.Send(Config.TrackServerRSQueueName, data, true, true);
        }


    }
}
