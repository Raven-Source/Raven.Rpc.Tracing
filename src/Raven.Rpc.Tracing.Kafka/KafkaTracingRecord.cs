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
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="kafkaBrokers">kafka节点，多个节点用逗号分隔，例如ip:port,ip:port</param>
        /// <param name="logType">日志类型，默认不记录任何日志</param>
        public KafkaTracingRecord(string kafkaBrokers, string logType = "Raven.Rpc.Tracing.Kafka.DefaultLog,Raven.Rpc.Tracing.Kafka")
        {
            if (string.IsNullOrEmpty(kafkaBrokers))
                throw new ArgumentNullException(nameof(kafkaBrokers));
            Init(kafkaBrokers, logType);
        }

        static bool _inited = false;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="kafkaBrokers"></param>
        /// <param name="logType"></param>
        static void Init(string kafkaBrokers, string logType)
        {
            lock (typeof(KafkaTracingRecord))
            {
                if (_inited)
                    return;
                var clientConfig = CreateConfig(kafkaBrokers, logType);
                Message.Kafka.Client.LoadConfig(clientConfig);
                _inited = true;
            }
        }

        static ClientConfig CreateConfig(string kafkaBrokers, string logType)
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
