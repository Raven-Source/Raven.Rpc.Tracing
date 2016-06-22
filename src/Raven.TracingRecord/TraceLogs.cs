using Raven.Rpc.Tracing;
using Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class TraceLogs : MongoDB.Bson.BsonDocument, IEntity<string>
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID
        {
            get { return base["_id"].ToString(); }
            set { base["_id"] = value; }
        }
        

        //    /// <summary>
        //    /// 扩展
        //    /// </summary>
        //    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        //    public override Dictionary<string, object> Extension { get; set; }

        //    /// <summary>
        //    /// 扩展
        //    /// </summary>
        //    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        //    public override Dictionary<string, object> ProtocolHeader { get; set; }

        //    /// <summary>
        //    /// 扩展
        //    /// </summary>
        //    public MongoDB.Bson.BsonDocument ProtocolHeader;

        //    /// <summary>
        //    /// 扩展
        //    /// </summary>
        //    public MongoDB.Bson.BsonDocument Extensions;

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public TraceLogs()
        //    {
        //        Extensions = new MongoDB.Bson.BsonDocument();
        //    }
    }

}
