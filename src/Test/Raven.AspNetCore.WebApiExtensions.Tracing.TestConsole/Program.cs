using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Raven.AspNetCore.WebApiExtensions.Tracing.TestConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
        }

    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            // 注入MVC框架
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            // 添加MVC中间件
            app.UseMvc();

            //app.Run(context =>
            //{
            //    return context.Response.WriteAsync("Hello from ASP.NET Core!");
            //});
        }
    }

}
