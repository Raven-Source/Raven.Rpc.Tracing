using Raven.Rpc.Tracing.Record;
using Raven.Rpc.Tracing.Record.Mongo;
using Raven.Rpc.Tracing.Record.RabbitMQ;
using System;
using System.Configuration;
using System.Web.Http;

namespace Raven.AspNet.WebApiExtensions.Tracing.Test
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly string hostName = ConfigurationManager.AppSettings["RabbitMQHost"];
        private static readonly string username = "liangyi";
        private static readonly string password = "123456";
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);


            GlobalConfiguration.Configuration.UseTracing(new TracingRecordMongo());
            //GlobalConfiguration.Configuration.UseTracing(new TracingRecordRabbitmq(hostName, username, password, new Loger()));
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

    //public class CustomModule : IHttpModule
    //{
    //    public void Init(HttpApplication context)
    //    {
    //        context.BeginRequest += new EventHandler(context_BeginRequest);
    //        context.EndRequest += new EventHandler(context_EndRequest);

    //    }

    //    private void context_EndRequest(object sender, EventArgs e)
    //    {
    //        //TracingConfig.EndRequest();
    //    }

    //    private void context_BeginRequest(object sender, EventArgs e)
    //    {
    //        HttpApplication ap = sender as HttpApplication;
    //        if (ap != null)
    //        {
    //            //CallContext.LogicalSetData("x", ap.Request.Url.OriginalString);
    //            //TracingConfig.BeginRequest();
    //            //ap.Response.Write("汤姆大叔测试PreApplicationStartMethod通过！<br/>");
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        //nothing to do here
    //    }
    //}

    //public class PreApplicationStartCode
    //{
    //    private static bool hasLoaded;

    //    public static void PreStart()
    //    {
    //        if (!hasLoaded)
    //        {
    //            hasLoaded = true;
    //            //注意这里的动态注册，此静态方法在Microsoft.Web.Infrastructure.DynamicModuleHelper
    //            DynamicModuleUtility.RegisterModule(typeof(CustomModule));
    //        }
    //    }
    //}
}
