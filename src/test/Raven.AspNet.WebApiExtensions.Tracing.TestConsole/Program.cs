using Owin;
using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Record;
using Raven.Rpc.Tracing.Record.Mongo;
using Raven.Rpc.Tracing.Record.RabbitMQ;
using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Raven.AspNet.WebApiExtensions.Tracing.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "http://*:9001/";
            Console.WriteLine("host: " + host);
            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>(host))
            {
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }

    public enum E : int
    { }

    public interface IA<T>
    {
        //T Code { get; set; }
    }

    public class A : IA<int>
    {
        //public int Code { get; set; }
    }

    public class B : IA<long>
    {
        //public long Code { get; set; }
    }

    public class Loger : ILoger
    {
        public void LogError(Exception ex, object dataObj)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    public class Startup
    {
        private static readonly string hostName = ConfigurationManager.AppSettings["RabbitMQHost"];
        private static readonly string username = "liangyi";
        private static readonly string password = "123456";
        private static readonly string systemID = "TracingHost.Test", systemName = "Tracing测试";

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // 默认返回Json数据
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //config.UseTracing(new TracingRecordMongo());
            config.UseTracing(new NoneTracingRecord());
            //config.UseTracing(new TracingRecordRabbitmq(hostName, username, password, new Loger()), systemID, systemName, "0");
            //appBuilder.UseTracingContext(new Raven.Rpc.Tracing.Record.TracingRecordKafka("121.43.149.229:9092,115.29.199.22:9092,115.29.204.19:9092"), systemID, systemName, "0");

            appBuilder.UseWebApi(config);

            //CallContext
            //appBuilder.Use(
        }
    }

    public class NoneTracingRecord : ITracingRecord
    {
        public void RecordSystemLogs(SystemLogs data)
        {
        }

        public void RecordTraceLog(TraceLogs data)
        {
        }
    }

}
