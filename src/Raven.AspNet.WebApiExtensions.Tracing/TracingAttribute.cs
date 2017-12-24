using Raven.Rpc.IContractModel;
using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    /// <summary>
    /// Tracing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TracingAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemID { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 环境类型
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.HasMarkerAttribute<NonTracingAttribute>())
            {
                var tracingContext = actionContext.Request.GetTracingContext();
                //var tracingContextHelper = tracingController.TracingContextHelper;
                var request = actionContext.Request;
                IRequestModel<Header> reqModel = null;
                Header reqHeader = null;

                if (actionContext.ActionArguments.Count > 0)
                {
                    foreach (var dic in actionContext.ActionArguments)
                    {
                        //if (dic.Value is RequestModel)
                        //{
                        //    reqModel = dic.Value as RequestModel;
                        //    break;
                        //}
                        reqModel = dic.Value as IRequestModel<Header>;
                        if (reqModel != null)
                        {
                            break;
                        }
                    }
                }
                if (reqModel == null && actionContext.Request.Content != null && string.Equals(actionContext.Request.Method.Method, "post", StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        reqModel = actionContext.Request.Content.ReadAsAsync<RequestModel>().Result;
                    }
                    catch { }
                    if (reqModel != null)
                    {
                        actionContext.ActionArguments.Add(Guid.NewGuid().ToString("N"), reqModel);
                    }
                }

                if (reqModel != null && reqModel.Header != null)
                {
                    reqHeader = reqModel.Header;
                    if (string.IsNullOrWhiteSpace(reqHeader.TraceID))
                    {
                        reqHeader.TraceID = Generate.GenerateId();// Util.GetUniqueCode32();
                    }
                    if (string.IsNullOrWhiteSpace(reqHeader.RpcID))
                    {
                        reqHeader.RpcID = "0";
                    }

                    //HttpContentData.SetTrackID(reqHeader.TraceID);
                    //HttpContentData.SubRpcID = reqHeader.RpcID + ".0";
                    //var header = HttpContentData.CloneRequestHeader(reqModel.Header);
                    //header.RpcID = header.RpcID + ".0";
                }
                else
                {
                    reqHeader = tracingContext.GetDefaultRequestHeader();
                    //HttpContentData.SetTrackID(reqHeader.TraceID);
                }
                tracingContext.SetSubRpcID(reqHeader.RpcID + ".0");
                tracingContext.SetRequestHeader(reqHeader);
                //HttpContentData.RequestHeader = reqHeader;

                //Not To Log
                if (!actionContext.HasMarkerAttribute<NotToRecordAttribute>())
                {
                    TraceLogs trace = new TraceLogs();
                    trace.ContextType = ContextType.Server.ToString();
                    trace.StartTime = DateTime.Now;
                    //trace.MachineAddr = tracingContextHelper.GetServerAddress();
                    trace.TraceId = reqHeader.TraceID;
                    trace.RpcId = reqHeader.RpcID;
                    trace.Protocol = string.Format("{0}/{1}", actionContext.Request.RequestUri.Scheme, actionContext.Request.Version);

                    trace.Environment = this.Environment ?? EnvironmentConfig.Environment;
                    trace.SystemID = this.SystemID ?? EnvironmentConfig.SystemID;
                    trace.SystemName = this.SystemName ?? EnvironmentConfig.SystemName;

                    //InvokeID
                    trace.InvokeID = request.RequestUri.AbsolutePath;
                    IEnumerable<string> folder;
                    if (actionContext.Request.Headers.TryGetValues(Config.ResponseHeaderFolderKey, out folder))
                    {
                        trace.ServerHost = actionContext.Request.RequestUri.Host + folder.FirstOrDefault();
                    }
                    else
                    {
                        trace.ServerHost = actionContext.Request.RequestUri.Host;
                    }

                    //SearchKey
                    var searchKey = reqModel as ISearchKey;
                    if (searchKey != null)
                    {
                        trace.SearchKey = searchKey.GetSearchKey();
                    }

                    TraceExtensionOnActionExecuting(actionContext, trace);
                    tracingContext.SetTraceLogs(trace);
                }
            }

            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="trace"></param>
        protected virtual void TraceExtensionOnActionExecuting(HttpActionContext actionContext, TraceLogs trace)
        {
            trace.Extensions.Add(nameof(actionContext.Request.RequestUri.PathAndQuery), actionContext.Request.RequestUri.PathAndQuery);

            if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
            {
                trace.Extensions.Add(Config.ParamsKey, Util.SerializerObjToString(actionContext.ActionArguments));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var actionContext = actionExecutedContext.ActionContext;
            if (!actionContext.HasMarkerAttribute<NonTracingAttribute>())
            {
                //var tracingContextHelper = actionContext.Request.GetTracingContextHelper();
                var tracingContext = actionContext.Request.GetTracingContext();

                var request = actionExecutedContext.Request;
                //var trace = Util.TracingContextHelper.GetContextItem<TraceLogs>(Config.ServerRSKey);
                var trace = tracingContext.GetTraceLogs();

                trace.EndTime = DateTime.Now;
                trace.TimeLength = Math.Round((trace.EndTime - trace.StartTime).TotalMilliseconds, 4);
                trace.IsException = false;
                trace.IsSuccess = true;

                //string traceId = HttpContextData.GetRequestHeader().TraceID;

                if (actionExecutedContext.Response != null)
                {
                    actionExecutedContext.Response.Headers.Add(Config.ResponseHeaderTraceKey, trace.TraceId);
                }

                //Exception
                if (actionExecutedContext.Exception != null)
                {
                    trace.IsException = true;
                    trace.IsSuccess = false;
                    trace.Extensions.Add(Config.ExceptionKey, Util.GetFullExceptionMessage(actionExecutedContext.Exception));
                }

                TraceExtensionOnActionExecuted(actionExecutedContext, trace);

                if (!actionContext.HasMarkerAttribute<NotToRecordAttribute>())
                {
                    Record(trace);
                }

            }

            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="trace"></param>
        protected virtual void TraceExtensionOnActionExecuted(HttpActionExecutedContext actionExecutedContext, TraceLogs trace)
        {
            if (actionExecutedContext.Response != null)
            {
                IResponseModel responseModel = null;
                if (actionExecutedContext.Response.TryGetContentValue<IResponseModel>(out responseModel))
                {
                    trace.Code = responseModel.GetCode();
                    trace.Extensions.Add(Config.ResultKey, Util.SerializerObjToString(responseModel));

                    //if (responseModel.Extension == null)
                    //{
                    //    responseModel.Extension = new List<Rpc.IContractModel.KeyValue<string, string>>();
                    //}
                    //responseModel.Extension.Add(new Rpc.IContractModel.KeyValue<string, string>(nameof(Raven.Rpc.IContractModel.Header.TraceID), trace.TraceId));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srs"></param>
        private void Record(TraceLogs srs)
        {
            ServiceContainer.Resolve<ITracingRecord>().RecordTraceLog(srs);
        }

    }
}
