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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        void RecordTraceLog(TraceLogs data);

        /// <summary>
        /// 记录系统日志
        /// </summary>
        /// <param name="data"></param>
        void RecordSystemLogs(SystemLogs data);
    }
}
