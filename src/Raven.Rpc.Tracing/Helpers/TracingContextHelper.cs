using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingContextHelper : ITracingContextHelper
    {
        public const string RequestHeaderKey = "__raven_RequestHeader";
        public const string SubRpcIDKey = "__raven_SubRpcID";
        public const string LocalAddressKey = "__localAddress";
        public const string TraceKey = "__raven_Trace";
        //private const string TraceIDKey = "__raven_TraceID";

        private IRequestContextHelper requestContextHelper;

        public TracingContextHelper(IRequestContextHelper requestContextHelper)
        {
            this.requestContextHelper = requestContextHelper;
        }

        /// <summary>
        ///                              
        /// </summary>
        /// <returns></returns>
        public Rpc.IContractModel.Header GetRequestHeader()
        {
            //var requestHeader = HttpContext.Current.Items[RequestHeaderKey];
            var requestHeader = this.requestContextHelper.GetContextItem<Rpc.IContractModel.Header>(RequestHeaderKey);
            if (requestHeader != null)
                return requestHeader as Rpc.IContractModel.Header;
            else return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        public void SetRequestHeader(Rpc.IContractModel.Header header)
        {
            this.requestContextHelper.SetContextItem(RequestHeaderKey, header);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSubRpcID()
        {
            var subRpcID = this.requestContextHelper.GetContextItem<string>(SubRpcIDKey);
            if (subRpcID == null)
            {
                subRpcID = "0";
                this.requestContextHelper.SetContextItem(SubRpcIDKey, subRpcID);
                //HttpContext.Current.Items[SubRpcIDKey] = subRpcID;
            }
            return subRpcID.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void SetSubRpcID(string val)
        {
            this.requestContextHelper.SetContextItem(SubRpcIDKey, val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rpc.IContractModel.Header GetDefaultRequestHeader()
        {
            return new Rpc.IContractModel.Header()
            {
                RpcID = "0",
                TraceID = Generate.GenerateId() //Util.GetUniqueCode32()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trace"></param>
        public void SetTraceLogs(TraceLogs trace)
        {
            this.requestContextHelper.SetContextItem(TraceKey, trace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TraceLogs GetTraceLogs()
        {
            return this.requestContextHelper.GetContextItem<TraceLogs>(TraceKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetServerAddress()
        {
            return this.requestContextHelper.GetContextItem<string>(LocalAddressKey);

        }
    }
}
