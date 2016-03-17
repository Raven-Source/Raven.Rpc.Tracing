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

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    /// <summary>
    /// Tracing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TracingAttribute : ActionFilterAttribute
    {
        private ITracingRecord record = ServiceContainer.Resolve<ITracingRecord>();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
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
                    reqModel = await actionContext.Request.Content.ReadAsAsync<RequestModel>();
                    actionContext.ActionArguments.Add(Guid.NewGuid().ToString("N"), reqModel);
                }

                if (reqModel != null && reqModel.Header != null)
                {
                    reqHeader = reqModel.Header;
                    if (string.IsNullOrWhiteSpace(reqHeader.TrackID))
                    {
                        reqHeader.TrackID = Util.GetUniqueCode32();
                    }
                    if (string.IsNullOrWhiteSpace(reqHeader.RpcID))
                    {
                        reqHeader.RpcID = "0";
                    }

                    //HttpContentData.SetTrackID(reqHeader.TrackID);
                    //HttpContentData.SubRpcID = reqHeader.RpcID + ".0";
                    //var header = HttpContentData.CloneRequestHeader(reqModel.Header);
                    //header.RpcID = header.RpcID + ".0";
                }
                else
                {
                    reqHeader = HttpContentData.GetDefaultRequestHeader();
                    //HttpContentData.SetTrackID(reqHeader.TrackID);
                }
                HttpContentData.SetSubRpcID(reqHeader.RpcID + ".0");
                HttpContentData.SetRequestHeader(reqHeader);
                //HttpContentData.RequestHeader = reqHeader;

                //Not To Log
                if (!actionContext.HasMarkerAttribute<NotToLogAttribute>())
                {
                    ServerRS srs = new ServerRS();
                    srs.StartTime = DateTime.Now;
                    srs.MachineAddr = Util.HttpHelper.GetServerAddress();
                    srs.TraceId = reqHeader.TrackID;
                    srs.RpcId = reqHeader.RpcID;
                    srs.ServerHost = actionContext.Request.RequestUri.Host;

                    srs.SystemID = this.systemID;
                    srs.SystemName = this.systemName;

                    srs.InvokeID = request.RequestUri.AbsolutePath;
                    srs.Extension.Add(nameof(request.RequestUri.PathAndQuery), request.RequestUri.PathAndQuery);

                    //srs.InvokeID = string.Format("{0}_{1}", actionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower(), actionContext.ActionDescriptor.ActionName.ToLower());

                    //if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
                    //{
                    //    srs.Extension.Add(Config.ParamsKey, actionContext.ActionArguments);
                    //}

                    //ServerRS Log Data TODO

                    Util.HttpHelper.SetHttpContextItem(Config.ServerRSKey, srs);
                }
            }
            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
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

                    string trackId = HttpContentData.GetRequestHeader().TrackID;
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
                            responseModel.Extension.Add(new Rpc.IContractModel.KeyValue<string, string>(nameof(Raven.Rpc.IContractModel.Header.TrackID), trackId));
                        }
                        actionExecutedContext.Response.Headers.Add(Config.ResponseHeaderTrackKey, trackId);
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



            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        private void Record(ServerRS srs)
        {
            record.RecordServerRS(srs);
        }

    }
}
