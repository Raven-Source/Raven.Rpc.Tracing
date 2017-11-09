using System;
using Raven.Message.RabbitMQ;
using Raven.Message.RabbitMQ.Configuration;

namespace Raven.TracingRecord.V2
{
    internal class RabbitTaskChannel
    {
        private Client _client
        {
            get
            {
                return Client.GetInstance("RabbitMQ_RavenLogs_v2");
            }
        }
        internal RabbitTaskChannel()
        {
            ClientConfiguration config=ClientConfiguration.Instance;
            //MQBrokerWatcher watcher = new MQBrokerWatcher();
            Client.Init(config);
        }

        internal void Dispose()
        {
            Client.Dispose();
        }

        internal bool Enqueue<T>(string queue, int priority, T obj)
        {
            if (priority > 0 && priority <= 10)
                return _client.Producer.Send(obj, queue, new SendOption() { Priority = (byte)priority });
            else
                return _client.Producer.Send(obj, queue);
        }

        internal bool OnReceive<T>(string queue, Func<T, bool> objReceived)
        {
            return _client.Consumer.OnReceive<T>(queue, (message, key, id, coId, args) => { return objReceived(message); });
        }

        internal bool Publish<T>(string exchange, string key, T obj)
        {
            _client.Producer.PublishToBuff(obj, exchange, key);
            return true;
        }

        internal bool Subscribe<T>(string exchange, string queue, string keyPattern, Func<T, bool> objReceived)
        {
            return _client.Consumer.Subscribe<T>(exchange, queue, keyPattern, (message, key, id, coId, args) => { return objReceived(message); });
        }
    }
}
