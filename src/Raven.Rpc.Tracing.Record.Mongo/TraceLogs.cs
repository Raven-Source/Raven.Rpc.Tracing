using MongoDB.Bson;
using Raven.Rpc.Tracing;
using Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record.Mongo.Entitys
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class TraceLogs : MongoDB.Bson.BsonDocument, IEntity<string>
    {
        public TraceLogs() : base()
        {

        }
        
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID
        {
            get { return base["_id"].ToString(); }
            set { base["_id"] = value; }
        }

    }

}
