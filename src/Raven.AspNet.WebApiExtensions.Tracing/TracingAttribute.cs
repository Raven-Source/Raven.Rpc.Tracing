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
        
        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="systemID"></param>
        ///// <param name="systemName"></param>
        //public TracingAttribute(string systemID = "", string systemName = "")
        //{
        //    this.systemID = systemID;
        //    this.systemName = systemName;
        //}        


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
                if (reqModel == null)
                {
                    reqModel = actionContext.Request.Content.ReadAsAsync<RequestModel>().Result;
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
                    reqHeader = HttpContextData.GetDefaultRequestHeader();
                    //HttpContentData.SetTrackID(reqHeader.TraceID);
                }
                HttpContextData.SetSubRpcID(reqHeader.RpcID + ".0");
                HttpContextData.SetRequestHeader(reqHeader);
                //HttpContentData.RequestHeader = reqHeader;

                //Not To Log
                if (!actionContext.HasMarkerAttribute<NotToLogAttribute>())
                {
                    ServerRS srs = new ServerRS();
                    srs.StartTime = DateTime.Now;
                    srs.MachineAddr = Util.HttpHelper.GetServerAddress();
                    srs.TraceId = reqHeader.TraceID;
                    srs.RpcId = reqHeader.RpcID;
                    srs.ServerHost = actionContext.Request.RequestUri.Authority;
                    srs.Protocol = actionContext.Request.RequestUri.Scheme;

                    srs.SystemID = this.systemID;
                    srs.SystemName = this.systemName;

                    srs.InvokeID = request.RequestUri.AbsolutePath;
                    srs.Extension.Add(nameof(request.RequestUri.PathAndQuery), request.RequestUri.PathAndQuery);
                    if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
                    {
                        srs.Extension.Add(Config.ParamsKey, actionContext.ActionArguments);
                    }

                    //srs.InvokeID = string.Format("{0}_{1}", actionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower(), actionContext.ActionDescriptor.ActionName.ToLower());

                    //if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
                    //{
                    //    srs.Extension.Add(Config.ParamsKey, actionContext.ActionArguments);
                    //}

                    //ServerRS Log Data TODO

                    Util.HttpHelper.SetHttpContextItem(Config.ServerRSKey, srs);
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var actionContext = actionExecutedContext.ActionContext;
            if (!actionContext.HasMarkerAttribute<NonTracingAttribute>())
            {
                if (!actionContext.HasMarkerAttribute<NotToLogAttribute>())
                {
                    var request = actionExecutedContext.Request;
                    var srs = Util.HttpHelper.GetHttpContextItem<ServerRS>(Config.ServerRSKey);

                    srs.EndTime = DateTime.Now;
                    srs.TimeLength = (srs.EndTime - srs.StartTime).TotalMilliseconds;
                    srs.IsException = false;
                    srs.IsSuccess = true;

                    string traceId = HttpContextData.GetRequestHeader().TraceID;
                    if (actionExecutedContext.Response != null)
                    {
                        IResponseModel responseModel = null;
                        if (actionExecutedContext.Response.TryGetContentValue<IResponseModel>(out responseModel))
                        {
                            srs.Code = responseModel.GetCode();
                            srs.Extension.Add(Config.ResultKey, responseModel);

                            if (responseModel.Extension == null)
                            {
                                responseModel.Extension = new List<Rpc.IContractModel.KeyValue<string, string>>();
                            }
                            responseModel.Extension.Add(new Rpc.IContractModel.KeyValue<string, string>(nameof(Raven.Rpc.IContractModel.Header.TraceID), traceId));
                        }
                        actionExecutedContext.Response.Headers.Add(Config.ResponseHeaderTraceKey, traceId);
                    }

                    //Exception
                    if (actionExecutedContext.Exception != null)
                    {
                        srs.IsException = true;
                        srs.IsSuccess = false;
                        srs.Extension.Add(Config.ExceptionKey, Util.GetFullExceptionMessage(actionExecutedContext.Exception));
                    }

                    Record(srs);
                }

            }

            ServiceContainer.Resolve<IInitRequestScopeContext>().EndRequest(actionContext.Request);
            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srs"></param>
        private void Record(ServerRS srs)
        {
            ServiceContainer.Resolve<ITracingRecord>().RecordServerRS(srs);
        }

    }
}
