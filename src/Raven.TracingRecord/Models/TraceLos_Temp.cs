using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord.Models
{
    public class TraceLos_Temp : Raven.Rpc.Tracing.TraceLogs
    {
        /// <summary>
        /// 扩展
        /// </summary>
        [Obsolete]
        public virtual Dictionary<string, object> Extension
        { get; set; }
    }
}
