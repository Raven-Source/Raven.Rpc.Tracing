using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Rpc.Tracing.Owin;
using Raven.Rpc.IContractModel;
using Newtonsoft.Json;
using Raven.Rpc.Tracing.Record;
using Raven.Serializer;
using System.Configuration;
using Raven.MessageQueue;

namespace Raven.AspNet.WebApiExtensions.Tracing.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "http://127.0.0.1:9001/";
            Console.WriteLine("host:" + host);
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

            appBuilder.UseTracingContext(new TracingRecordRabbitmq(hostName, username, password, new Loger()));
            appBuilder.UseWebApi(config);

            //CallContext
            //appBuilder.Use(
        }
    }
}
