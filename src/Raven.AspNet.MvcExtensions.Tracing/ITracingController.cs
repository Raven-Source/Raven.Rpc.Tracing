using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITracingController
    {
        /// <summary>
        /// 
        /// </summary>
        ITracingContext TracingContext { get; set; }

        
    }
}
