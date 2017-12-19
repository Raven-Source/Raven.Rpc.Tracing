using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Context
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IInitRequestTracingContext
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="context"></param>
        //void BeginRequest(ITracingContextHelper contextHelper, IDictionary<string, object> environment);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="context"></param>
        //void EndRequest(ITracingContextHelper contextHelper, IDictionary<string, object> environment);

        ITracingContext GetTracingContext(IDictionary<string, object> environment);
    }
}
