using Raven.Rpc.Tracing;
using Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    public class TraceLogs : Raven.Rpc.Tracing.TraceLogs, IEntity<string>
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// 扩展
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public override Dictionary<string, object> Extension { get; set; }

        /// <summary>
        /// 扩展
        /// </summary>
        public MongoDB.Bson.BsonDocument Extensions;

        /// <summary>
        /// 
        /// </summary>
        public TraceLogs()
        {
            Extensions = new MongoDB.Bson.BsonDocument();
        }
    }

    //public class ServerRSLogs : ServerRS, IEntity<string>
    //{
    //    [MongoDB.Bson.Serialization.Attributes.BsonId]
    //    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    //    public string ID
    //    {
    //        get;
    //        set;
    //    }

    //    /// <summary>
    //    /// 扩展
    //    /// </summary>
    //    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    //    public override Dictionary<string, object> Extension { get; set; }

    //    /// <summary>
    //    /// 扩展
    //    /// </summary>
    //    public MongoDB.Bson.BsonDocument Extensions;

    //    public ServerRSLogs()
    //    {
    //        Extensions = new MongoDB.Bson.BsonDocument();
    //    }
    //}

    //public class ClientSRLogs : ClientSR, IEntity<string>
    //{
    //    [MongoDB.Bson.Serialization.Attributes.BsonId]
    //    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    //    public string ID
    //    {
    //        get;
    //        set;
    //    }

    //    /// <summary>
    //    /// 扩展
    //    /// </summary>
    //    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    //    public override Dictionary<string, object> Extension { get; set; }

    //    /// <summary>
    //    /// 扩展
    //    /// </summary>
    //    public MongoDB.Bson.BsonDocument Extensions;

    //    public ClientSRLogs()
    //    {
    //        Extensions = new MongoDB.Bson.BsonDocument();
    //    }


    //}
}
