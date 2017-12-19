using Raven.Rpc.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    public interface ITracingController
    {
        ITracingContextHelper TracingContextHelper { get; set; }

        
    }
}
