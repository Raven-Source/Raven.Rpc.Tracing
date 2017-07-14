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
        private const string RpcIDKey = "RpcID";
        private const string TraceIDKey = "TraceID";
        private static ITracingRecord record = ServiceContainer.Resolve<ITracingRecord>();
        //private static Dictionary<int, Tuple<string, string>> dict = new Dictionary<int, Tuple<string, string>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="systemID"></param>
        /// <param name="systemName"></param>
        /// <param name="environment">环境</param>
        /// <param name="TraceExtensionAct">TraceExtensionAct</param>
        public static void RegistTracing(this RpcHttpClient client, string systemID = null, string systemName = null, string environment = null, Action<HttpResponseMessage, RpcContext, TraceLogs> TraceExtensionAct = null)
        {
            RpcHttpClient.OnResponseDelegate onResponse = (response, rpcContext) =>
            {
                TraceLogs trace = new TraceLogs();
                trace.IsSuccess = true;
                trace.IsException = false;
                trace.SystemID = systemID ?? EnvironmentConfig.SystemID;
                trace.SystemName = systemName ?? EnvironmentConfig.SystemName;
                trace.Environment = environment ?? EnvironmentConfig.Environment;
                TraceExtensionAct?.Invoke(response, rpcContext, trace);

                FillClientSR(trace, response.RequestMessage, response, rpcContext);

                Record(trace);
            };
            RpcHttpClient.OnErrorDelegate onError = (ex, request, rpcContext) =>
            {
                TraceLogs trace = new TraceLogs();
                trace.IsSuccess = false;
                trace.IsException = true;
                trace.SystemID = systemID ?? EnvironmentConfig.SystemID;
                trace.SystemName = systemName ?? EnvironmentConfig.SystemName;
                trace.Environment = environment ?? EnvironmentConfig.Environment;

                FillClientSR(trace, request, null, rpcContext);

                trace.Extensions.Add("Exception", Util.GetFullExceptionMessage(ex));

                Record(trace);
            };


            client.RequestContentDataHandler -= Client_RequestContentDataHandler;
            client.OnRequest -= Client_OnRequest;
            client.OnResponse -= onResponse;
            client.OnError -= onError;

            client.RequestContentDataHandler += Client_RequestContentDataHandler;
            client.OnRequest += Client_OnRequest;
            client.OnResponse += onResponse;
            client.OnError += onError;

            //dict[client.GetHashCode()] = new Tuple<string, string>(systemID, systemName);
        }

        private static void Client_OnRequest(HttpRequestMessage request, RpcContext rpcContext)
        {
            var rpcId = string.Empty;
            var requestHeader = TracingContextData.GetRequestHeader();
            if (requestHeader == null)
            {
                requestHeader = TracingContextData.GetDefaultRequestHeader();
                TracingContextData.SetRequestHeader(requestHeader);
                //HttpContentData.SetSubRpcID(modelHeader.RpcID + ".0");
                //rpcId = requestHeader.RpcID + ".0";
            }
            //else
            //{
            //    rpcId = Util.VersionIncr(TracingContextData.GetSubRpcID());
            //}
            rpcId = Util.VersionIncr(TracingContextData.GetSubRpcID());
            TracingContextData.SetSubRpcID(rpcId);

            rpcContext.Items[TraceIDKey] = requestHeader.TraceID;
            rpcContext.Items[RpcIDKey] = rpcId;

            //post/put content is ObjectContent
            var oc = request.Content as ObjectContent;
            if (oc != null)
            {
                var reqModel = oc.Value as IRequestModel<Header>;
                if (reqModel != null)
                {
                    if (reqModel.Header == null)
                    {
                        reqModel.Header = new Header();
                    }

                    reqModel.Header.TraceID = requestHeader.TraceID;
                    reqModel.Header.RpcID = rpcId;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private static void Client_RequestContentDataHandler(ref object data)
        {
            //var rpcId = string.Empty;
            //var requestHeader = TracingContextData.GetRequestHeader();
            //if (requestHeader == null)
            //{
            //    requestHeader = TracingContextData.GetDefaultRequestHeader();
            //    TracingContextData.SetRequestHeader(requestHeader);
            //    //HttpContentData.SetSubRpcID(modelHeader.RpcID + ".0");
            //    //rpcId = requestHeader.RpcID + ".0";
            //}
            ////else
            ////{
            ////    rpcId = Util.VersionIncr(TracingContextData.GetSubRpcID());
            ////}
            //rpcId = Util.VersionIncr(TracingContextData.GetSubRpcID());
            //TracingContextData.SetSubRpcID(rpcId);

            if (data == null)
            {
                data = new RequestModel();
            }

            //var reqModel = data as IRequestModel<Header>;
            //if (reqModel != null)
            //{
            //    if (reqModel.Header == null)
            //    {
            //        reqModel.Header = new Header();
            //    }

            //    reqModel.Header.TraceID = requestHeader.TraceID;
            //    reqModel.Header.TraceID = rpcId;
            //}
        }

        //private static void Client_OnResponse(HttpResponseMessage response, RpcContext rpcContext)
        //{
        //    TraceLogs trace = new TraceLogs();
        //    trace.IsSuccess = true;
        //    trace.IsException = false;
        //    FillClientSR(trace, response.RequestMessage, rpcContext);

        //    Record(trace);
        //}

        //private static void Client_OnError(Exception ex, HttpRequestMessage request, RpcContext rpcContext)
        //{
        //    TraceLogs trace = new TraceLogs();
        //    trace.IsSuccess = false;
        //    trace.IsException = true;
        //    FillClientSR(trace, request, rpcContext);

        //    trace.Extension.Add("Exception", Util.GetFullExceptionMessage(ex));

        //    Record(trace);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="rpcContext"></param>
        private static void FillClientSR(TraceLogs trace, HttpRequestMessage request, HttpResponseMessage response, RpcContext rpcContext)
        {
            trace.ContextType = ContextType.Client.ToString();
            //var requestHeader = TracingContextData.GetRequestHeader();  //raven Request Header
            var uri = request.RequestUri;

            //int index = uri.AbsoluteUri.IndexOf("?");
            //if (index > 0)
            //{
            //    trace.ServiceMethod = uri.AbsoluteUri.Substring(0, index);
            //}
            //else
            //{
            //    trace.ServiceMethod = uri.AbsoluteUri;
            //}

            trace.MachineAddr = Util.TracingContextHelper.GetServerAddress();

            trace.InvokeID = uri.AbsolutePath;
            trace.ServerHost = uri.Host;

            //trace.Extension.Add(nameof(uri.AbsolutePath), uri.AbsolutePath);
            trace.Extensions.Add(nameof(uri.PathAndQuery), uri.PathAndQuery);
            //trace.Extension.Add(nameof(uri.Host), uri.Host);

            trace.Extensions.Add(nameof(rpcContext.RequestModel), Util.SerializerObjToString(rpcContext.RequestModel));
            trace.Extensions.Add(nameof(rpcContext.ResponseModel), Util.SerializerObjToString(rpcContext.ResponseModel));
            trace.ResponseSize = rpcContext.ResponseSize;

            trace.Protocol = uri.Scheme;

            trace.ProtocolHeader.Add("RequestHeaders", new Dictionary<string, string>
            {
                { "Accept", request.Headers.Accept.ToString() },
                { "Accept-Encoding", request.Headers.AcceptEncoding.ToString() },
                { "Content-Type", request.Content?.Headers?.ContentType?.ToString() },
            });

            if (response != null)
            {
                trace.ProtocolHeader.Add("ResponseHeaders", new Dictionary<string, string>
                {
                    { "Content-Encoding", response.Content?.Headers?.ContentEncoding?.ToString() },
                    { "Content-Type", response.Content?.Headers?.ContentType?.ToString() },
                });
            }

            //trace.SendSTime = rpcContext.SendStartTime;
            //trace.ReceiveETime = rpcContext.ReceiveEndTime;
            //trace.ExceptionTime = rpcContext.ExceptionTime;
            trace.StartTime = rpcContext.SendStartTime;

            if (rpcContext.ReceiveEndTime.HasValue)
            {
                trace.EndTime = rpcContext.ReceiveEndTime.Value;
                trace.TimeLength = (trace.EndTime - trace.StartTime).TotalMilliseconds;
            }
            else if (rpcContext.ExceptionTime.HasValue)
            {
                trace.EndTime = rpcContext.ExceptionTime.Value;
                trace.TimeLength = (trace.EndTime - trace.StartTime).TotalMilliseconds;
            }

            //trace.TimeLength = trace.ReceiveETime.HasValue ? (trace.ReceiveETime.Value - trace.SendSTime).TotalMilliseconds : 0D;
            //trace.RpcId = modelHeader.RpcID;



            //var reqModel = rpcContext.RequestModel as IRequestModel<Header>;
            //if (reqModel != null && reqModel.Header != null)
            //{
            //    trace.RpcId = rpcContext.Items[RpcIDKey].ToString();
            //}

            //if modelHeader is null, create new traceID
            trace.RpcId = rpcContext.Items[RpcIDKey].ToString();
            trace.TraceId = rpcContext.Items[TraceIDKey].ToString();

            if (rpcContext.ResponseModel != null)
            {
                var resModel = rpcContext.ResponseModel as IResponseModel;
                if (resModel != null)
                {
                    trace.Code = resModel.GetCode();
                }

                //SearchKey
                var searchKey = rpcContext.ResponseModel as ISearchKey;
                if (searchKey != null)
                {
                    trace.SearchKey = searchKey.GetSearchKey();
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trace"></param>
        private static void Record(TraceLogs trace)
        {
            record.RecordTraceLog(trace);
        }

    }
}
