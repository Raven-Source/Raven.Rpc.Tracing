using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record
{
    public interface ITracingRecord
    {
        //void RecordClientSR(ClientSR data);

        //void RecordServerRS(ServerRS data);

        void RecordTraceLog(TraceLogs data);
    }
}
