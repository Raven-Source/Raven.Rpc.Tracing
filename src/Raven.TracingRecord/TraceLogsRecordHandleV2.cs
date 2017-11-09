using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Raven.Rpc.Tracing;
using Raven.TracingRecord.Models;
using Raven.TracingRecord.V2;

namespace Raven.TracingRecord
{
    public class TraceLogsRecordHandleV2
    {
        #region GetInstance

        private static Lazy<TraceLogsRecordHandleV2> _instance = new Lazy<TraceLogsRecordHandleV2>(() => new TraceLogsRecordHandleV2());

        public static TraceLogsRecordHandleV2 GetInstance => _instance.Value;

        #endregion

        readonly TraceLogsRepAsync _traceLogsRep;
        readonly CacheQueue<TraceLos_Temp> _queue;
        readonly RabbitTaskChannel _channel;
        public TraceLogsRecordHandleV2() 
        {
            _traceLogsRep=new TraceLogsRepAsync();
            _queue=new CacheQueue<TraceLos_Temp>(Config.Interval,Config.BatchSaveCount,Save);
            _channel=new RabbitTaskChannel();
        }

        public void Run()
        {
            _channel.OnReceive<TraceLos_Temp>(Config.TraceLogsQueueName, l=>
            {
                _queue.Enqueue(l);
                return true;
            });
        }

        Task Save(List<TraceLos_Temp> list)
        {
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
                    var json = JsonConvert.SerializeObject(l, Newtonsoft.Json.Formatting.None,
                        new JavaScriptDateTimeConverter()); // l.ToString(Newtonsoft.Json.Formatting.None);

                    try
                    {
                        var log = Raven.TracingRecord.TraceLogs.Parse(json);

                        if (log.Contains("Extension"))
                        {
                            log.Remove("Extension");
                        }

                        logs.Add(log);
                    }
                    catch
                    {
                        //
                    }
                }
            }
            return _traceLogsRep.InsertBatchAsync(logs);
        }

        public void Stop()
        {
            _channel.Dispose();
            _queue.Stop();
        }
    }
}
