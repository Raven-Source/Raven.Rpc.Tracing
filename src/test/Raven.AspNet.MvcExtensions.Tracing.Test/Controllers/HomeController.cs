using Newtonsoft.Json;
using Raven.Rpc.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Raven.AspNet.MvcExtensions.Tracing.Test.Controllers
{
    [Tracing]
    public class HomeController : Controller, Raven.AspNet.MvcExtensions.Tracing.ITracingController
    {
        public ITracingContextHelper TracingContextHelper { get; set; }

        public class A
        {
            public int ID;
        }

        //[Tracing]
        // GET: Home
        public ActionResult Index(string json)
        {
            return View();
        }

        // GET: Home
        public JsonResult Get()
        {
            return new JsonResult() { Data = new { id = 1324, name = "ggg" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [NotToRecord]
        // GET: Home
        public JsonResult Get2()
        {
            //throw new Exception();
            return new JsonResult() { Data = new { id = 1324, name = "ggg" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}