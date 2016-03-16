using Raven.Rpc.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.HttpProtocol.Tracing.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string res = null;
            res = Util.GetUniqueCode22();

            res = new Guid(Util.GetGuidArray()).ToString();
            Console.WriteLine(res);
            res = new Guid(Util.GetGuidArray()).ToString();
            Console.WriteLine(res);
            res = new Guid(Util.GetGuidArray()).ToString();
            Console.WriteLine(res);
            res = new Guid(Util.GetGuidArray()).ToString();
            Console.WriteLine(res);
            res = new Guid(Util.GetGuidArray()).ToString();
            Console.WriteLine(res);
        }
    }
}
