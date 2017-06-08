using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
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
        IDisposable model;

        public TraceLogsRecordHandle(string serverName)
            : base(serverName, 1000)
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

                Console.WriteLine("RegisterReceive:{0}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"));
                if (model != null)
                {
                    return;
                }
                model = RabbitMQClientManager.GetInstance.rabbitMQClient.RegisterReceive<Raven.TracingRecord.Models.TraceLos_Temp>(Config.TraceLogsQueueName
                      , (l) =>
                      {
                          if (!base.isRun)
                          {
                              return;
                          }

                          if (l.Extension != null && l.Extension.Count > 0)
                          {
                              l.Extensions = l.Extension;
                              l.Extension = null;
                          }
                          var json = JsonConvert.SerializeObject(l, Newtonsoft.Json.Formatting.None, new JavaScriptDateTimeConverter());
                          try
                          {
                              if (json.Length > 16000000)
                              {
                                  Loger.GetInstance.LogInfo(string.Format("larger json:{0}", json));
                              }

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

                            //
                            try
                              {
                                  traceLogsRep.Insert(log);

                              }
                              catch (Exception ex)
                              {
                                  Loger.GetInstance.LogError(ex, null);
                              }

                          }
                          catch (Exception ex)
                          {
                              Loger.GetInstance.LogError(ex, null);
                              Loger.GetInstance.LogInfo(string.Format("parse error json:{0}", json));
                          }

                          Console.WriteLine("InsertBatch End:{0}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"));
                      }
                      , noAck: true);

                model.Dispose();
                model = null;


            }
            catch (Exception ex)
            {
                Loger.GetInstance.LogError(ex, null);
            }
        }
    }
}
