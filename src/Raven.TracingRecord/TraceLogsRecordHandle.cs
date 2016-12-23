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

    public class TraceLogsRecordHandle : Handle
    {
        #region GetInstance

        private static Lazy<TraceLogsRecordHandle> _instance = new Lazy<TraceLogsRecordHandle>(() => new TraceLogsRecordHandle("Raven.TracingRecord"));

        public static TraceLogsRecordHandle GetInstance
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

        //ServerRSLogsRep serverRSlogRep;
        //ClientSRLogsRep clientSRlogRep;
        TraceLogsRep traceLogsRep;

        public TraceLogsRecordHandle(string serverName)
            : base(serverName, 100)
        {
            traceLogsRep = new TraceLogsRep();
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        public void ProcessResetAwardData()
        {
            try
            {
                //json序列化后，日期为字符串，进mongodb数据库有问题

                Console.WriteLine("ReceiveBatch:{0}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"));
                var list = RabbitMQClientManager.GetInstance.rabbitMQClient.ReceiveBatch<Raven.TracingRecord.Models.TraceLos_Temp>(Config.TraceLogsQueueName, noAck: true);
                Console.WriteLine("ReceiveBatch End:{0}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"));
                var logs = new List<MongoDB.Bson.BsonDocument>();

                if (list != null && list.Count > 0)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        var l = list[i];
                        if (l.Extension != null && l.Extension.Count > 0)
                        {
                            l.Extensions = l.Extension;
                            l.Extension = null;
                        }

                        var json = JsonConvert.SerializeObject(l, Newtonsoft.Json.Formatting.None, new JavaScriptDateTimeConverter());// l.ToString(Newtonsoft.Json.Formatting.None);
                        try
                        {
                            var log = Raven.TracingRecord.TraceLogs.Parse(json);

                            if (log.Contains("Extension"))
                            {
                                //if (!log.Contains("Extensions"))
                                //{
                                //    var ext = log["Extension"];
                                //    log.Add("Extensions", ext);
                                //}
                                log.Remove("Extension");
                            }

                            logs.Add(log);
                        }
                        catch
                        {
                            continue;
                        }

                    }


                    Console.WriteLine("InsertBatch:{0}, Count:{1}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"), logs.Count);

                    int pi = 0;
                    int ps = 200;
                    while (pi * ps < logs.Count)
                    {
                        var temp = logs.Skip(pi * ps).Take(ps).ToList();
                        pi++;
                        traceLogsRep.InsertBatch(temp);
                    }

                    //traceLogsRep.InsertBatch(logs);
                    Console.WriteLine("InsertBatch End:{0}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"));
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
