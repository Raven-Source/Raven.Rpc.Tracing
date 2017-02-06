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
    internal static class TracingContextData
    {
        private const string RequestHeaderKey = "__raven_RequestHeader";
        private const string SubRpcIDKey = "__raven_SubRpcID";
        //private const string TraceIDKey = "__raven_TraceID";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Rpc.IContractModel.Header GetRequestHeader()
        {
            //var requestHeader = HttpContext.Current.Items[RequestHeaderKey];
            var requestHeader = Util.TracingContextHelper.GetContextItem<Rpc.IContractModel.Header>(RequestHeaderKey);
            if (requestHeader != null)
                return requestHeader as Rpc.IContractModel.Header;
            else return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        public static void SetRequestHeader(Rpc.IContractModel.Header header)
        {
            Util.TracingContextHelper.SetContextItem(RequestHeaderKey, header);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSubRpcID()
        {
            var subRpcID = Util.TracingContextHelper.GetContextItem<string>(SubRpcIDKey);
            if (subRpcID == null)
            {
                subRpcID = "0";
                Util.TracingContextHelper.SetContextItem(SubRpcIDKey, subRpcID);
                //HttpContext.Current.Items[SubRpcIDKey] = subRpcID;
            }
            return subRpcID.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public static void SetSubRpcID(string val)
        {
            Util.TracingContextHelper.SetContextItem(SubRpcIDKey, val);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Rpc.IContractModel.Header GetDefaultRequestHeader()
        {
            return new Rpc.IContractModel.Header()
            {
                RpcID = "0",
                TraceID = Generate.GenerateId() //Util.GetUniqueCode32()
            };
        }

        //public static void SetTraceID(string val)
        //{
        //    Util.TracingContextHelper.SetContextItem(TraceIDKey, val);
        //}

        ///// <summary>
        ///// GetTraceID
        ///// </summary>
        ///// <returns></returns>
        //public static string GetTraceID()
        //{
        //    var traceID = Util.TracingContextHelper.GetContextItem<string>(TraceIDKey);
        //    if (traceID == null)
        //    {
        //        traceID = Util.GetUniqueCode32();
        //        Util.TracingContextHelper.SetContextItem(TraceIDKey, traceID);
        //        //HttpContext.Current.Items[SubRpcIDKey] = subRpcID;
        //    }
        //    return traceID;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="header"></param>
        ///// <returns></returns>
        //public static Rpc.IContractModel.Header CloneRequestHeader(Rpc.IContractModel.Header header)
        //{
        //    return new Rpc.IContractModel.Header()
        //    {
        //        RpcID = header.RpcID,
        //        TraceID = header.TraceID,
        //        Token = header.Token,
        //    };
        //}

    }
}
