using Raven.Rpc.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Rpc.HttpProtocol.Tracing;
using Raven.Rpc.Tracing.NoContext;
using Raven.Rpc.Tracing.Record;
using System.Configuration;
using Raven.MessageQueue;

namespace Raven.Rpc.Tracing.TestConsole
{
    class Program
    {
        static Rpc.HttpProtocol.RpcHttpClient client = new Rpc.HttpProtocol.RpcHttpClient("http://127.0.0.1:9001/");
        private static readonly string hostName = ConfigurationManager.AppSettings["RabbitMQHost"];
        private static readonly string username = "liangyi";
        private static readonly string password = "123456";
        private static readonly string systemID = "TracingClient.Test", systemName = "Tracing测试";

        static void Main(string[] args)
        {
            TracingConfig.UseTracingContext(new TracingRecordRabbitmq(hostName, username, password, new Loger()), systemID, systemName, "0");

            client.RegistTracing();
            var res = client.Invoke<Raven.Rpc.IContractModel.RequestModel, Raven.Rpc.IContractModel.ResponseModel<string, int>>("api/test/get2", new Rpc.IContractModel.RequestModel());

            Raven.Rpc.Tracing.Helpers.TracingHelper.RecordSystemLogs(new SystemLogs()
            {
                Level = LogsLevel.L1
            });

            System.Console.ReadLine();
            //string res = null;
            //res = Util.GetUniqueCode22();

            //res = new Guid(Util.GetGuidArray()).ToString();
            //Console.WriteLine(res);
            //res = new Guid(Util.GetGuidArray()).ToString();
            //Console.WriteLine(res);
            //res = new Guid(Util.GetGuidArray()).ToString();
            //Console.WriteLine(res);
            //res = new Guid(Util.GetGuidArray()).ToString();
            //Console.WriteLine(res);
            //res = new Guid(Util.GetGuidArray()).ToString();
            //Console.WriteLine(res);
        }

    }


    public class Loger : ILoger
    {
        public void LogError(Exception ex, object dataObj)
        {
            System.Console.WriteLine(ex.Message);
            System.Console.WriteLine(ex.StackTrace);
        }
    }
}
