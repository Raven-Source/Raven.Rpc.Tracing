using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITracingApiController
    {
        /// <summary>
        /// 
        /// </summary>
        ITracingContext TracingContext { get; set; }
    }
}
