using Raven.Rpc.Tracing.ContextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    public interface ITracingController
    {
        ITraceContext TraceContext { get; }
    }
}
