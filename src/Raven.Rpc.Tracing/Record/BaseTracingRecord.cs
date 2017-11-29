using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record
{
    public abstract class BaseTracingRecord : ITracingRecord
    {
        public virtual void RecordSystemLogs(SystemLogs data)
        {

        }

        public virtual void RecordTraceLog(TraceLogs data)
        {

        }
    }
}
