using Raven.Rpc.IContractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.Record;
using Newtonsoft.Json;
using Raven.Rpc.Tracing.ContextData;

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
        public string systemID;

        /// <summary>
        /// 系统名称
        /// </summary>
        public string systemName;

        /// <summary>
        /// 环境类型
        /// </summary>
        public string environment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            ServiceContainer.Resolve<IInitRequestScopeContext>().BeginRequest(actionContext.Request);

            if (!actionContext.HasMarkerAttribute<NonTracingAttribute>())
            {
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
                        reqHeader.TraceID = Util.GetUniqueCode32();
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
                    reqHeader = TracingContextData.GetDefaultRequestHeader();
                    //HttpContentData.SetTrackID(reqHeader.TraceID);
                }
                TracingContextData.SetSubRpcID(reqHeader.RpcID + ".0");
                TracingContextData.SetRequestHeader(reqHeader);
                //HttpContentData.RequestHeader = reqHeader;

                //Not To Log
                if (!actionContext.HasMarkerAttribute<NotToLogAttribute>())
                {
                    TraceLogs trace = new TraceLogs();
                    trace.ContextType = ContextType.Server.ToString();
                    trace.StartTime = DateTime.Now;
                    trace.MachineAddr = Util.TracingContextHelper.GetServerAddress();
                    trace.TraceId = reqHeader.TraceID;
                    trace.RpcId = reqHeader.RpcID;
                    trace.Protocol = string.Format("{0}/{1}", actionContext.Request.RequestUri.Scheme, actionContext.Request.Version);

                    trace.Environment = this.environment ?? EnvironmentConfig.Environment;
                    trace.SystemID = this.systemID ?? EnvironmentConfig.SystemID;
                    trace.SystemName = this.systemName ?? EnvironmentConfig.SystemName;

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

                    //srs.InvokeID = string.Format("{0}_{1}", actionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower(), actionContext.ActionDescriptor.ActionName.ToLower());

                    //if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
                    //{
                    //    srs.Extension.Add(Config.ParamsKey, actionContext.ActionArguments);
                    //}

                    //ServerRS Log Data TODO

                    Util.TracingContextHelper.SetContextItem(Config.ServerRSKey, trace);
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
                trace.Extensions.Add(Config.ParamsKey, actionContext.ActionArguments);
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
                if (!actionContext.HasMarkerAttribute<NotToLogAttribute>())
                {
                    var request = actionExecutedContext.Request;
                    var trace = Util.TracingContextHelper.GetContextItem<TraceLogs>(Config.ServerRSKey);

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
                    Record(trace);
                }

            }

            ServiceContainer.Resolve<IInitRequestScopeContext>().EndRequest(actionContext.Request);
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
                    trace.Extensions.Add(Config.ResultKey, responseModel);

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
