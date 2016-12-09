using Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemLogs : Raven.Rpc.Tracing.SystemLogs, IEntity<string>
    {
        /// <summary>
        /// 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
    }
}
