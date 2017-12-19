using MongoDB.Bson;
using MongoDB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    //public class ServerRSLogsRep : MongoRepository<ServerRSLogs, string>
    //{
    //    public ServerRSLogsRep() :
    //        base(DBConfig.connString, DBConfig.dbName)
    //    { }
    //}

    //public class ClientSRLogsRep : MongoRepository<ClientSRLogs, string>
    //{
    //    public ClientSRLogsRep() :
    //        base(DBConfig.connString, DBConfig.dbName)
    //    { }
    //}
    public class TraceLogsRep : MongoRepository<Raven.Rpc.Tracing.Record.Mongo.Entitys.TraceLogs, string>
    {
        public TraceLogsRep() :
            base(DBConfig.connString, DBConfig.dbName)
        { }


        public void Insert(BsonDocument document)
        {
            base.Database.GetCollection<BsonDocument>(nameof(Raven.Rpc.Tracing.Record.Mongo.Entitys.TraceLogs)).InsertOne(document);
        }

        public void InsertBatch(IEnumerable<BsonDocument> documents)
        {
            base.Database.GetCollection<BsonDocument>(nameof(Raven.Rpc.Tracing.Record.Mongo.Entitys.TraceLogs)).InsertMany(documents);
        }
    }

    //public class SystemLogsRep : MongoRepository<SystemLogs, string>
    //{
    //    public SystemLogsRep() :
    //        base(DBConfig.connString, DBConfig.dbName)
    //    { }
    //}

    public static class DBConfig
    {
        public static readonly string connString = System.Configuration.ConfigurationManager.AppSettings["MongoDB_RavenLogs"];
        public static readonly string dbName = "RavenLogs";
    }
}
