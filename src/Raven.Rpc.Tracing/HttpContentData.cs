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
        //private const string TrackIDKey = "__raven_TrackID";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Rpc.IContractModel.Header GetRequestHeader()
        {
            //var requestHeader = HttpContext.Current.Items[RequestHeaderKey];
            var requestHeader = Util.HttpHelper.GetHttpContextItem<Rpc.IContractModel.Header>(RequestHeaderKey);
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
            Util.HttpHelper.SetHttpContextItem(RequestHeaderKey, header);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSubRpcID()
        {
            var subRpcID = Util.HttpHelper.GetHttpContextItem<string>(SubRpcIDKey);
            if (subRpcID == null)
            {
                subRpcID = "0";
                Util.HttpHelper.SetHttpContextItem(SubRpcIDKey, subRpcID);
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
            Util.HttpHelper.SetHttpContextItem(SubRpcIDKey, val);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static string GetTrackID()
        //{
        //    var trackId = Util.HttpHelper.GetHttpContextItem<string>(TrackIDKey);
        //    return trackId;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="val"></param>
        //public static void SetTrackID(string val)
        //{
        //    Util.HttpHelper.SetHttpContextItem(TrackIDKey, val);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Rpc.IContractModel.Header GetDefaultRequestHeader()
        {
            return new Rpc.IContractModel.Header()
            {
                RpcID = "0",
                TrackID = Util.GetUniqueCode32()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
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
