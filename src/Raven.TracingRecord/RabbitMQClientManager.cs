using Raven.MessageQueue.WithRabbitMQ;
using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    public class RabbitMQClientManager
    {
        #region GetInstance

        private static Lazy<RabbitMQClientManager> _instance = new Lazy<RabbitMQClientManager>(() => new RabbitMQClientManager());

        public static RabbitMQClientManager GetInstance
        {
            get { return _instance.Value; }
        }


        Options rabbitMQOptions;
        public RabbitMQClient rabbitMQClient;

        #endregion

        public RabbitMQClientManager()
        {
            RabbitMQConfig rabbitMQConfig = new RabbitMQConfig("RabbitMQ_RavenLogs");
            rabbitMQOptions = new Raven.MessageQueue.WithRabbitMQ.Options()
            {
                SerializerType = SerializerType.NewtonsoftJson,
                HostName = rabbitMQConfig.hostName,
                Password = rabbitMQConfig.password,
                UserName = rabbitMQConfig.username,
                //MaxQueueCount = 100000,
                Loger = new Loger()
            };
            rabbitMQClient = RabbitMQClient.GetInstance(rabbitMQOptions);
        }

    }
}
