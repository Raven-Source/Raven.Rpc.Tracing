using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record
{
    public class KafkaClient
    {
        IEnumerable<KeyValuePair<string, object>> config = new Dictionary<string, object>();

        public void ProduceAsync()
        {

        }


    }
}
