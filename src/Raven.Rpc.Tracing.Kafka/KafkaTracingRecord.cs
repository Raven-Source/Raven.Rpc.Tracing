using Raven.Message.Kafka.Impl.Configuration.Simple;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Kafka
{
    public class KafkaTracingRecord : ITracingRecord
    {
        const string BrokerName = "LogKafkaBroker";
        public KafkaTracingRecord(string kafkaBrokers, string logType = "Raven.Message.Kafka.Impl.Configuration.App.ClientConfig,Raven.Message.Kafka")
        {
            if (string.IsNullOrEmpty(kafkaBrokers))
                throw new ArgumentNullException(nameof(kafkaBrokers));

            var clientConfig = CreateConfig(kafkaBrokers, logType);
            Message.Kafka.Client.LoadConfig(clientConfig);
        }

        ClientConfig CreateConfig(string kafkaBrokers, string logType)
        {
            ClientConfig config = new ClientConfig();
            config.LogType = logType;

            BrokerConfig brokerConfig = new BrokerConfig();
            brokerConfig.Name = BrokerName;
            brokerConfig.SerializerType = Serializer.SerializerType.NewtonsoftJson;
            brokerConfig.Uri = kafkaBrokers;
            var brokers = new List<BrokerConfig>(1) { brokerConfig };
            config.Brokers = brokers;

            return config;
        }

        public void RecordSystemLogs(SystemLogs data)
        {
            var connection = GetConnection();
            if (connection == null)
                return;
            connection.Producer.ProduceAndForget("syslog", data);
        }

        public void RecordTraceLog(TraceLogs data)
        {
            var connection = GetConnection();
            if (connection == null)
                return;
            connection.Producer.ProduceAndForget("tracelog", data);
        }

        Message.Kafka.Connection _connection = null;
        Message.Kafka.Connection GetConnection()
        {
            if (_connection == null)
            {
                lock (this)
                {
                    if (_connection == null)
                        try
                        {
                            _connection = Message.Kafka.Client.GetConnection(BrokerName);
                        }
                        catch { }
                }
            }
            return _connection;
        }
    }
}
