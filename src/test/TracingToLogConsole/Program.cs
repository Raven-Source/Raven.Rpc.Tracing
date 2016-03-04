using MongoDB.Repository;
using Raven.MessageQueue;
using Raven.MessageQueue.WithRabbitMQ;
using Raven.Rpc.Tracing;
using Raven.Serializer;
using Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TracingToLogConsole
{
    public class Loger : ILoger
    {
        public void LogError(Exception ex, object dataObj)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    class Program
    {
        private static readonly string hostName = ConfigurationManager.AppSettings["RabbitMQHost"];
        private static readonly string username = "liangyi";
        private static readonly string password = "123456";

        private static RabbitMQClient rabbitMQClient = null;

        static void Main(string[] args)
        {
            var rabbitMQOptions = new Raven.MessageQueue.WithRabbitMQ.Options()
            {
                SerializerType = SerializerType.NewtonsoftJson,
                HostName = hostName,
                Password = password,
                UserName = username,
                //MaxQueueCount = 100000,
                Loger = new Loger()
            };
            rabbitMQClient = RabbitMQClient.GetInstance(rabbitMQOptions);
            ServerRSLogsRep serverRSlogRep = new ServerRSLogsRep();

            while (true)
            {
                var list = rabbitMQClient.ReceiveBatch<ServerRSLogs>(Util.TrackServerRSQueueName);

                if (list != null && list.Count > 0)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        var l = list[i];
                        //List<string> jsonObjectKey = new List<string>();
                        if (l.Extension != null)
                        {
                            foreach (var kv in l.Extension)
                            {
                                if (kv.Value.GetType().FullName == "Newtonsoft.Json.Linq.JObject")//"Jil.DeserializeDynamic.JsonObject")
                                {
                                    var str = Newtonsoft.Json.JsonConvert.SerializeObject(kv.Value);
                                    l.Extensions.Add(kv.Key, MongoDB.Bson.BsonDocument.Parse(str));
                                }
                                else
                                {
                                    l.Extensions.Add(kv.Key, MongoDB.Bson.BsonDocument.Create(kv.Value));
                                }
                            }
                            l.Extension = null;
                            //if (jsonObjectKey.Count > 0)
                            //{
                            //    foreach (var k in jsonObjectKey)
                            //    {
                            //        var value = l.Extension[k];
                            //        var str = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                            //        l.Extension[k] = MongoDB.Bson.BsonDocument.Parse(str);
                            //    }
                            //}
                        }
                    }
                    serverRSlogRep.InsertBatch(list);
                }

                Console.WriteLine(list.Count);
                Thread.Sleep(10000);
            }
        }
    }

    public class ServerRSLogsRep : MongoRepository<ServerRSLogs, string>
    {
        private static readonly string connString = System.Configuration.ConfigurationManager.AppSettings["MongoDB_RavenLogs"];
        private static readonly string dbName = "RavenLogs";

        public ServerRSLogsRep() :
            base(connString, dbName)
        { }
    }

    public class ServerRSLogs : ServerRS, IEntity<string>
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [MsgPack.Serialization.MessagePackIgnore]
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

        public ServerRSLogs()
        {
            Extensions = new MongoDB.Bson.BsonDocument();
        }
    }
}
