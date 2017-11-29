using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record
{
    public class TracingRecordFile : ITracingRecord
    {
        public void RecordSystemLogs(SystemLogs data)
        {
        }

        public void RecordTraceLog(TraceLogs data)
        {
            string log = string.Format($"TraceId={data.TraceId}, RpcId={data.RpcId}, SystemID={data.SystemID}, SystemName={data.SystemName}, ContextType={data.ContextType}, InvokeID={data.InvokeID}");
            //Console.WriteLine(log);
            File.AppendAllText("raven.log", log);
        }
    }
}
