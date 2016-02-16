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
    public static class HttpContentData
    {
        private const string RequestHeaderKey = "__raven_RequestHeader";
        private const string SubRpcIDKey = "__raven_SubRpcID";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Rpc.IContractModel.Header GetRequestHeader(HttpRequestMessage request)
        {
            //var requestHeader = HttpContext.Current.Items[RequestHeaderKey];
            var requestHeader = Util.HttpHelper.GetHttpContextItem<Rpc.IContractModel.Header>(request, RequestHeaderKey);
            if (requestHeader != null)
                return requestHeader as Rpc.IContractModel.Header;
            else return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="header"></param>
        public static void SetRequestHeader(HttpRequestMessage request, Rpc.IContractModel.Header header)
        {
            Util.HttpHelper.SetHttpContextItem(request, RequestHeaderKey, header);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetSubRpcID(HttpRequestMessage request)
        {
            var subRpcID = Util.HttpHelper.GetHttpContextItem<string>(request, SubRpcIDKey);
            if (subRpcID == null)
            {
                subRpcID = "0";
                Util.HttpHelper.SetHttpContextItem(request, SubRpcIDKey, subRpcID);
                //HttpContext.Current.Items[SubRpcIDKey] = subRpcID;
            }
            return subRpcID.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="val"></param>
        public static void SetSubRpcID(HttpRequestMessage request, string val)
        {
            Util.HttpHelper.SetHttpContextItem(request, SubRpcIDKey, val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Rpc.IContractModel.Header GetDefaultRequestHeader()
        {
            return new Rpc.IContractModel.Header()
            {
                //RpcID = "0",
                TrackID = Util.GetUniqueCode32()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Rpc.IContractModel.Header CloneRequestHeader(Rpc.IContractModel.Header header)
        {
            return new Rpc.IContractModel.Header()
            {
                RpcID = header.RpcID,
                TrackID = header.TrackID,
                Token = header.Token,
                UUID = header.UUID,
            };
        }
        
    }
}
