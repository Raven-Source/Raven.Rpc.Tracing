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

    public class TraceLogsRecordHandleV3 : Handle
    {
        #region GetInstance

        private static Lazy<TraceLogsRecordHandleV2> _instance = new Lazy<TraceLogsRecordHandleV2>(() => new TraceLogsRecordHandleV2("Raven.TracingRecord"));

        public static TraceLogsRecordHandleV2 GetInstance
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

        public TraceLogsRecordHandleV3(string serverName)
            : base(serverName, 1000)
        {
            traceLogsRep = new TraceLogsRep();
        }

        private const string raven_trace_logs = Config.TraceLogsQueueName;

        /// <summary>
        /// 调用方法
        /// </summary>
        public void ProcessResetAwardData()
        {
            try
            {
                //json序列化后，日期为字符串，进mongodb数据库有问题
                if (model != null)
                {
                    return;
                }

                Console.WriteLine("RegisterReceive:{0}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"));
                model = RabbitMQClientManager.GetInstance.rabbitMQClient.RegisterReceive<Raven.TracingRecord.Models.TraceLos_Temp>(raven_trace_logs
                      , (l) =>
                      {
                          if (!base.isRun)
                          {
                              return false;
                          }
                          
                          //if (l.Extension != null && l.Extension.Count > 0)
                          //{
                          //    l.Extensions = l.Extension;
                          //    l.Extension = null;
                          //}
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
                                  return false;
                              }

                          }
                          catch (Exception ex)
                          {
                              Loger.GetInstance.LogError(ex, null);
                              Loger.GetInstance.LogInfo(string.Format("parse error json:{0}", json));
                              return false;
                          }

                          Console.WriteLine("InsertBatch End:{0}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"));
                          return true;
                      }, noAck: true);

                if (model != null)
                {
                    model.Dispose();
                    model = null;
                }


            }
            catch (Exception ex)
            {
                if (model != null)
                {
                    model.Dispose();
                    model = null;
                }
                Loger.GetInstance.LogError(ex, null);
            }
        }



    }
}
