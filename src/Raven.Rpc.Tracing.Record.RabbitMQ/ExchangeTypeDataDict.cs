using System.Collections.Generic;
using RabbitMQExchangeType = global::RabbitMQ.Client.ExchangeType;

namespace Raven.Rpc.Tracing.Record.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExchangeTypeDataDict
    {
        /// <summary>
        /// exchangeType对应数值表
        /// </summary>
        public readonly static IDictionary<ExchangeType, string> ExchangeTypeDict = new Dictionary<ExchangeType, string>()
        {
            {ExchangeType.Default, string.Empty},
            {ExchangeType.Fanout, RabbitMQExchangeType.Fanout},
            {ExchangeType.Direct, RabbitMQExchangeType.Direct},
            {ExchangeType.Topic, RabbitMQExchangeType.Topic},
            {ExchangeType.Headers, RabbitMQExchangeType.Headers}
        };
    }
}
