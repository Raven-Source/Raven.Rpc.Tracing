using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Message.RabbitMQ.Abstract;

namespace Raven.TracingRecord.V2
{
    public class Log : ILog
    {
        public void LogError(string errorMessage, Exception ex, object dataObj)
        {
            //Console.WriteLine(ex);
        }

        public void LogDebug(string info, object dataObj)
        {
            //Console.WriteLine(info);
        }
    }
}
