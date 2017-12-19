using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    public interface ITracingContextHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Rpc.IContractModel.Header GetRequestHeader();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        void SetRequestHeader(Rpc.IContractModel.Header header);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetSubRpcID();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        void SetSubRpcID(string val);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Rpc.IContractModel.Header GetDefaultRequestHeader();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trace"></param>
        void SetTraceLogs(TraceLogs trace);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        TraceLogs GetTraceLogs();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetServerAddress();
    }
}
