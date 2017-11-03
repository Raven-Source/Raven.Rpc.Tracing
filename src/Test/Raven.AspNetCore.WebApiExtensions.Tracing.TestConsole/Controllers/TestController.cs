using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raven.AspNetCore.WebApiExtensions.Tracing.TestConsole.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [CustemFilter]
        public User Get()
        {
            return new Controllers.User() {  Name = "sdggas"};
        }

    }

    public class User
    {
        public long ID
        {
            get; set;
        }

        public string Name { get; set; }

        public static int a;

        public string Desc { get; set; }

        public virtual void Say()
        {
        }
    }

    public class GoodUser
    {
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustemFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }


    }

}
