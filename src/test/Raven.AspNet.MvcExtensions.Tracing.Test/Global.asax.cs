using Raven.MessageQueue;
using Raven.Rpc.Tracing.Record;
using Raven.Rpc.Tracing.WebHost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Raven.AspNet.MvcExtensions.Tracing.Test
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly string hostName = ConfigurationManager.AppSettings["RabbitMQHost"];
        private static readonly string username = "liangyi";
        private static readonly string password = "123456";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            TracingConfig.UseTracingContext(new TracingRecordRabbitmq(hostName, username, password, new Loger()));
        }
    }

    public class Loger : ILoger
    {
        public void LogError(Exception ex, object dataObj)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
}
