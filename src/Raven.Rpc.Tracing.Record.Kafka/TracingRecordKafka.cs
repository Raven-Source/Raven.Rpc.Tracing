using Raven.Message.Kafka.Impl.Configuration.Simple;
using System;
using System.Collections.Generic;

namespace Raven.Rpc.Tracing.Record
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingRecordKafka : ITracingRecord
    {
        const string BrokerName = "LogKafkaBroker";
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="kafkaBrokers">kafka节点，多个节点用逗号分隔，例如ip:port,ip:port</param>
        /// <param name="logType">日志类型，默认不记录任何日志</param>
        public TracingRecordKafka(string kafkaBrokers, string logType = "Raven.Rpc.Tracing.Record.DefaultLog,Raven.Rpc.Tracing.Record.Kafka")
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
            lock (typeof(TracingRecordKafka))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void RecordSystemLogs(SystemLogs data)
        {
            var connection = GetConnection();
            if (connection == null)
                return;
            connection.Producer.ProduceAndForget(Config.SystemLogsQueueName, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void RecordTraceLog(TraceLogs data)
        {
            var connection = GetConnection();
            if (connection == null)
                return;
            connection.Producer.ProduceAndForget(Config.TraceLogsQueueNameV1, data);
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
