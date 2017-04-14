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
        static KafkaTracingRecord()
        {
            Init();
        }

        static void Init()
        {
            try
            {
                string fileName = "Raven.Rpc.Tracing.Kafka.dll.config";
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                if (!File.Exists(configFile))
                    configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", fileName);
                if (!File.Exists(configFile))
                    throw new FileNotFoundException("kafka config file not found", fileName);
                Message.Kafka.Impl.Configuration.App.ConfigFactory factory = new Message.Kafka.Impl.Configuration.App.ConfigFactory(configFile, "ravenKafka");
                Message.Kafka.Client.LoadConfig(factory);
            }
            catch { }
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
                            _connection = Message.Kafka.Client.GetConnection("broker");
                        }
                        catch { }
                }
            }
            return _connection;
        }
    }
}
