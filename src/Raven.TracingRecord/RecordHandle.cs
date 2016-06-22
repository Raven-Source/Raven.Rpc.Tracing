using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Raven.MessageQueue;
using Raven.MessageQueue.WithRabbitMQ;
using Raven.Rpc.Tracing;
using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{

    public class RecordHandle : Handle
    {
        #region GetInstance

        private static Lazy<RecordHandle> _instance = new Lazy<RecordHandle>(() => new RecordHandle("Raven.TracingRecord"));

        public static RecordHandle GetInstance
        {
            get { return _instance.Value; }
        }

        #endregion

        protected override Action ProcessWorkAction
        {
            get
            {
                return ProcessResetAwardData;
            }
        }

        Options rabbitMQOptions;
        RabbitMQClient rabbitMQClient;

        //ServerRSLogsRep serverRSlogRep;
        //ClientSRLogsRep clientSRlogRep;
        TraceLogsRep traceLogsRep;

        public RecordHandle(string serverName)
            : base(serverName, 5000)
        {
            RabbitMQConfig rabbitMQConfig = new RabbitMQConfig("RabbitMQ_RavenLogs");
            rabbitMQOptions = new Raven.MessageQueue.WithRabbitMQ.Options()
            {
                SerializerType = SerializerType.NewtonsoftJson,
                HostName = rabbitMQConfig.hostName,
                Password = rabbitMQConfig.password,
                UserName = rabbitMQConfig.username,
                //MaxQueueCount = 100000,
                Loger = new Loger()
            };
            rabbitMQClient = RabbitMQClient.GetInstance(rabbitMQOptions);
            traceLogsRep = new TraceLogsRep();
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        public void ProcessResetAwardData()
        {
            try
            {
                var list = rabbitMQClient.ReceiveBatch<JObject>(Config.TraceLogsQueueName);
                var logs = new List<MongoDB.Bson.BsonDocument>();

                if (list != null && list.Count > 0)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        var l = list[i];
                        var json = JsonConvert.SerializeObject(l, Newtonsoft.Json.Formatting.None, new JavaScriptDateTimeConverter());// l.ToString(Newtonsoft.Json.Formatting.None);
                        var log = Raven.TracingRecord.TraceLogs.Parse(json);

                        if (log.Contains("Extension"))
                        {
                            if (!log.Contains("Extensions"))
                            {
                                var ext = log["Extension"];
                                log.Add("Extensions", ext);
                            }
                            log.Remove("Extension");
                        }

                        logs.Add(log);
                    }


                    traceLogsRep.InsertBatch(logs);
                }

                //var list = rabbitMQClient.ReceiveBatch<ServerRSLogs>(Config.TraceServerRSQueueName);

                //if (list != null && list.Count > 0)
                //{
                //    for (var i = 0; i < list.Count; i++)
                //    {
                //        var l = list[i];
                //        //List<string> jsonObjectKey = new List<string>();
                //        if (l.Extension != null)
                //        {
                //            foreach (var kv in l.Extension)
                //            {
                //                if (kv.Value.GetType().FullName == "Newtonsoft.Json.Linq.JObject")//"Jil.DeserializeDynamic.JsonObject")
                //                {
                //                    var str = Newtonsoft.Json.JsonConvert.SerializeObject(kv.Value);
                //                    l.Extensions.Add(kv.Key, MongoDB.Bson.BsonDocument.Parse(str));
                //                }
                //                else
                //                {
                //                    l.Extensions.Add(kv.Key, MongoDB.Bson.BsonValue.Create(kv.Value));
                //                }
                //            }
                //            l.Extension = null;
                //            //if (jsonObjectKey.Count > 0)
                //            //{
                //            //    foreach (var k in jsonObjectKey)
                //            //    {
                //            //        var value = l.Extension[k];
                //            //        var str = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                //            //        l.Extension[k] = MongoDB.Bson.BsonDocument.Parse(str);
                //            //    }
                //            //}
                //        }
                //    }
                //    serverRSlogRep.InsertBatch(list);
                //}

                //var list2 = rabbitMQClient.ReceiveBatch<ClientSRLogs>(Config.TraceClientSRQueueName);

                //if (list2 != null && list2.Count > 0)
                //{
                //    for (var i = 0; i < list2.Count; i++)
                //    {
                //        var l = list2[i];
                //        //List<string> jsonObjectKey = new List<string>();
                //        if (l.Extension != null)
                //        {
                //            foreach (var kv in l.Extension)
                //            {
                //                if (kv.Value.GetType().FullName == "Newtonsoft.Json.Linq.JObject")//"Jil.DeserializeDynamic.JsonObject")
                //                {
                //                    var str = Newtonsoft.Json.JsonConvert.SerializeObject(kv.Value);
                //                    l.Extensions.Add(kv.Key, MongoDB.Bson.BsonDocument.Parse(str));
                //                }
                //                else
                //                {
                //                    l.Extensions.Add(kv.Key, MongoDB.Bson.BsonValue.Create(kv.Value));
                //                }
                //            }
                //            l.Extension = null;
                //            //if (jsonObjectKey.Count > 0)
                //            //{
                //            //    foreach (var k in jsonObjectKey)
                //            //    {
                //            //        var value = l.Extension[k];
                //            //        var str = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                //            //        l.Extension[k] = MongoDB.Bson.BsonDocument.Parse(str);
                //            //    }
                //            //}
                //        }
                //    }
                //    clientSRlogRep.InsertBatch(list2);
                //}
            }
            catch (Exception ex)
            {
                Loger.GetInstance.LogError(ex, null);
            }
        }
    }
}
