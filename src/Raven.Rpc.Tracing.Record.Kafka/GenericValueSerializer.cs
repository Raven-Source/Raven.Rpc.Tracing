using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record
{
    public class GenericValueSerializer<T> : Confluent.Kafka.Serialization.ISerializer<T>, IDeserializer<T>
    {
        private Raven.Serializer.IDataSerializer serializer;

        public GenericValueSerializer(Raven.Serializer.IDataSerializer serializer)
        {
            this.serializer = serializer;
        }

        public IEnumerable<KeyValuePair<string, object>> Configure(IEnumerable<KeyValuePair<string, object>> config, bool isKey)
            => config;

        public T Deserialize(string topic, byte[] data)
            => serializer.Deserialize<T>(data);

        public byte[] Serialize(string topic, T data)
            => serializer.Serialize(data);
    }
}
