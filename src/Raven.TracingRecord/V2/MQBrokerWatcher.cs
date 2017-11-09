using System;
using System.Configuration;
using Raven.Message.RabbitMQ.Abstract;

namespace Raven.TracingRecord.V2
{
    public class MQBrokerWatcher : IBrokerWatcher
    {
        public event EventHandler<BrokerChangeEventArg> BrokerUriChanged;

        public string GetBrokerUri(string brokerName)
        {
            return ConfigurationManager.AppSettings[brokerName];
        }
    }
}
