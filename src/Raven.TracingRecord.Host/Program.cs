using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord.Host
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            string p1 = (args != null && args.Length > 0) ? args[0] : null;
            if (p1 == "c")
            {
                TraceLogsRecordHandleV1.GetInstance.Run();
                //TraceLogsRecordHandle.GetInstance.Run();
                SystemLogRecordHandle.GetInstance.Run();
                TraceLogsRecordHandleV2.GetInstance.Run();
                Console.ReadLine();
                TraceLogsRecordHandleV1.GetInstance.Stop();
                //TraceLogsRecordHandle.GetInstance.Stop();
                SystemLogRecordHandle.GetInstance.Stop();
                TraceLogsRecordHandleV2.GetInstance.Stop();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new TracingRecordService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
