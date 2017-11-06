using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord.HostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TraceLogsRecordHandle.GetInstance.Run();
            //TraceLogsRecordHandleV1.GetInstance.Run();
            SystemLogRecordHandle.GetInstance.Run();

            Console.WriteLine("start...");
            Console.ReadLine();
            TraceLogsRecordHandle.GetInstance.Stop();
            //TraceLogsRecordHandleV1.GetInstance.Stop();
            SystemLogRecordHandle.GetInstance.Stop();
            Console.WriteLine("stop...");

            Console.ReadLine();
        }
    }
}
