using Raven.Rpc.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    public interface ITracingApiController
    {
        ITracingContextHelper TracingContextHelper { get; set; }
    }
}
