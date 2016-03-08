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
            RecordHandle.GetInstance.Run();

            Console.WriteLine("start...");
            RecordHandle.GetInstance.Stop();
            Console.WriteLine("stop...");

            Console.ReadLine();
        }
    }
}
