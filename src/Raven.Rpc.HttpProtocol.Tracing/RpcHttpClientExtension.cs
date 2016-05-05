using Raven.Rpc.IContractModel;
using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.HttpProtocol.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class RpcHttpClientExtension
    {
        private static ITracingRecord record = ServiceContainer.Resolve<ITracingRecord>();
        //private static Dictionary<int, Tuple<string, string>> dict = new Dictionary<int, Tuple<string, string>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="systemID"></param>
        /// <param name="systemName"></param>
        /// <param name="environment">环境</param>
        public static void RegistTracing(this RpcHttpClient client, string systemID = null, string systemName = null, string environment = null)
        {
            RpcHttpClient.OnResponseDelegate onResponse = (response, rpcContext) =>
            {
                TraceLogs sr = new TraceLogs();
                sr.IsSuccess = true;
                sr.IsException = false;
                sr.SystemID = systemID;
                sr.SystemName = systemName;
                sr.Environment = environment;

                FillClientSR(sr, response.RequestMessage, rpcContext);

                Record(sr);
            };
            RpcHttpClient.OnErrorDelegate onError = (ex, request, rpcContext) =>
            {
                TraceLogs sr = new TraceLogs();
                sr.IsSuccess = false;
                sr.IsException = true;
                sr.SystemID = systemID;
                sr.SystemName = systemName;
                FillClientSR(sr, request, rpcContext);

                sr.Extension.Add("Exception", Util.GetFullExceptionMessage(ex));

                Record(sr);
            };


            client.RequestContentDataHandler -= Client_RequestContentDataHandler;
            //client.OnRequest -= Client_OnRequest;
            client.OnResponse -= onResponse;
            client.OnError -= onError;

            client.RequestContentDataHandler += Client_RequestContentDataHandler;
            //client.OnRequest += Client_OnRequest;
            client.OnResponse += onResponse;
            client.OnError += onError;

            //dict[client.GetHashCode()] = new Tuple<string, string>(systemID, systemName);
        }

        private static void Client_RequestContentDataHandler(ref object data)
        {
            if (data == null)
            {
                data = new RequestModel();
            }

            var reqModel = data as IRequestModel<Header>;
            if (reqModel != null)
            {
                if (reqModel.Header == null)
                {
                    reqModel.Header = new Header();
                }

                var modelHeader = HttpContextData.GetRequestHeader();
                if (modelHeader == null)
                {
                    modelHeader = HttpContextData.GetDefaultRequestHeader();
                    HttpContextData.SetRequestHeader(modelHeader);
                    //HttpContentData.SetSubRpcID(modelHeader.RpcID + ".0");
                    reqModel.Header.RpcID = modelHeader.RpcID + ".0";
                }
                else
                {
                    reqModel.Header.RpcID = Util.VersionIncr(HttpContextData.GetSubRpcID());
                }
                HttpContextData.SetSubRpcID(reqModel.Header.RpcID);
                reqModel.Header.TraceID = modelHeader.TraceID;
                reqModel.Header.UUID = modelHeader.UUID;
            }
        }

        //private static void Client_OnResponse(HttpResponseMessage response, RpcContext rpcContext)
        //{
        //    TraceLogs sr = new TraceLogs();
        //    sr.IsSuccess = true;
        //    sr.IsException = false;
        //    FillClientSR(sr, response.RequestMessage, rpcContext);

        //    Record(sr);
        //}

        //private static void Client_OnError(Exception ex, HttpRequestMessage request, RpcContext rpcContext)
        //{
        //    TraceLogs sr = new TraceLogs();
        //    sr.IsSuccess = false;
        //    sr.IsException = true;
        //    FillClientSR(sr, request, rpcContext);

        //    sr.Extension.Add("Exception", Util.GetFullExceptionMessage(ex));

        //    Record(sr);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="request"></param>
        /// <param name="rpcContext"></param>
        private static void FillClientSR(TraceLogs sr, HttpRequestMessage request, RpcContext rpcContext)
        {
            sr.ContextType = ContextType.Client.ToString();
            var modelHeader = HttpContextData.GetRequestHeader();
            var uri = request.RequestUri;

            //int index = uri.AbsoluteUri.IndexOf("?");
            //if (index > 0)
            //{
            //    sr.ServiceMethod = uri.AbsoluteUri.Substring(0, index);
            //}
            //else
            //{
            //    sr.ServiceMethod = uri.AbsoluteUri;
            //}

            sr.MachineAddr = Util.HttpHelper.GetServerAddress();

            sr.InvokeID = uri.AbsolutePath;
            sr.ServerHost = uri.Host;

            //sr.Extension.Add(nameof(uri.AbsolutePath), uri.AbsolutePath);
            sr.Extension.Add(nameof(uri.PathAndQuery), uri.PathAndQuery);
            //sr.Extension.Add(nameof(uri.Host), uri.Host);

            sr.Extension.Add(nameof(rpcContext.RequestModel), rpcContext.RequestModel);
            sr.Extension.Add(nameof(rpcContext.ResponseModel), rpcContext.ResponseModel);
            sr.ResponseSize = rpcContext.ResponseSize;

            sr.Protocol = uri.Scheme;

            //sr.SendSTime = rpcContext.SendStartTime;
            //sr.ReceiveETime = rpcContext.ReceiveEndTime;
            //sr.ExceptionTime = rpcContext.ExceptionTime;
            sr.StartTime = rpcContext.SendStartTime;

            if (rpcContext.ReceiveEndTime.HasValue)
            {
                sr.EndTime = rpcContext.ReceiveEndTime.Value;
                sr.TimeLength = (sr.EndTime - sr.StartTime).TotalMilliseconds;
            }
            else if (rpcContext.ExceptionTime.HasValue)
            {
                sr.EndTime = rpcContext.ExceptionTime.Value;
                sr.TimeLength = (sr.EndTime - sr.StartTime).TotalMilliseconds;
            }

            //sr.TimeLength = sr.ReceiveETime.HasValue ? (sr.ReceiveETime.Value - sr.SendSTime).TotalMilliseconds : 0D;
            //sr.RpcId = modelHeader.RpcID;
            var reqModel = rpcContext.RequestModel as IRequestModel<Header>;
            if (reqModel != null && reqModel.Header != null)
            {
                sr.RpcId = reqModel.Header.RpcID;
            }
            sr.TraceId = modelHeader.TraceID;

            if (rpcContext.ResponseModel != null)
            {
                var resModel = rpcContext.ResponseModel as IResponseModel;
                if (resModel != null)
                {
                    sr.Code = resModel.GetCode();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        private static void Record(TraceLogs sr)
        {
            record.RecordTraceLog(sr);
        }

    }
}
