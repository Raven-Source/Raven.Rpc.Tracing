using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="dataObj">1.Dequeue(出队时为BasicGetResult的Body,byte[]类型 或 null);2.Enqueue(入队时为传的dataObj)</param>
        void LogError(Exception ex, object dataObj);
    }
}
