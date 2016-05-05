using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITracingRecord
    {
        //void RecordClientSR(ClientSR data);

        //void RecordServerRS(ServerRS data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        void RecordTraceLog(TraceLogs data);
    }
}
