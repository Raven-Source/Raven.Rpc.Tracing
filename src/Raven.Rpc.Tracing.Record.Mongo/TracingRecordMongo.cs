using MongoDB.Bson;
using Raven.TracingRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record.Mongo
{
    public class TracingRecordMongo : ITracingRecord
    {
        TraceLogsRep rep = new TraceLogsRep();

        public void RecordSystemLogs(SystemLogs data)
        {
        }

        public void RecordTraceLog(TraceLogs data)
        {
            var bs = data.ToJson();
            var entity = Raven.Rpc.Tracing.Record.Mongo.Entitys.TraceLogs.Parse(bs);
            rep.Insert(entity);
        }
    }
}
