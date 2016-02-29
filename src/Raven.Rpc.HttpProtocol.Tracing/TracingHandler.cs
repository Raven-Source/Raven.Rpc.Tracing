using Raven.Rpc.IContractModel;
using Raven.Rpc.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.HttpProtocol.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class TracingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public static void RegistTracing(ref object data)
        {
            var model = (data as RequestModel);
            if (model != null && model.Header != null)
            {
                var header = model.Header;
                header.RpcID = Util.VersionIncr(HttpContentData.GetSubRpcID());
            }
        }
    }
}
