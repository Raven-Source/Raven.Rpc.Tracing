using System;
using System.Threading;

namespace Raven.Rpc.Tracing.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                Console.WriteLine("Raven.Rpc.Tracing.TestConsole [kafka broker]");
                Console.ReadLine();
                return;
            }
            string kafkaBroker = args[0];
            Console.WriteLine("kafka broker:{0}", kafkaBroker);
            //TracingRecordKafka record = new TracingRecordKafka(kafkaBroker, "Raven.Rpc.Tracing.TestConsole.Log,Raven.Rpc.Tracing.TestConsole");

            //while (true)
            //{
            //    int count = 0;
            //    string lastTraceId = null;
            //    for (int i = 0; i < 10; i++)
            //    {
            //        SystemLogs log = new SystemLogs()
            //        {
            //            TraceId = Generate.GenerateId(),
            //            Environment = "0",
            //            SystemID = "tracingtest",
            //            SystemName = "tracingtest",
            //            Level = LogsLevel.L4,
            //            Content = "test log",
            //            CreateTime = DateTime.Now
            //        };
            //        record.RecordSystemLogs(log);
            //        lastTraceId = log.TraceId;
            //        count++;
            //    }
            //    Thread.Sleep(1000);
            //    Console.WriteLine("write {0} logs, last traceid {1}", count, lastTraceId);
            //}
        }
    }

    //class Log : ILog
    //{
    //    public void Debug(string format, params object[] pars)
    //    {
            
    //    }

    //    public void Error(Exception ex)
    //    {
    //        Console.WriteLine(ex.ToString());
    //    }

    //    public void Error(string format, params object[] pars)
    //    {
    //        Console.WriteLine(string.Format(format, pars));
    //    }

    //    public void Info(string format, params object[] pars)
    //    {
            
    //    }
    //}
}
