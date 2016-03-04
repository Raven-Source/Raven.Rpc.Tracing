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
        private const string ServerRSKey = "__raven_ServerRS";
        private ITracingRecord record = ServiceContainer.Resolve<ITracingRecord>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var attrs = actionContext.ActionDescriptor.GetCustomAttributes<NonTracingAttribute>();
            if (attrs.Count == 0)
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
                }

                if (reqModel != null && reqModel.Header != null)
                {
                    reqHeader = reqModel.Header;

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
                if (actionContext.ActionDescriptor.GetCustomAttributes<NotToLogAttribute>().Count == 0)
                {
                    ServerRS srs = new ServerRS();
                    srs.StartTime = DateTime.Now;
                    srs.MachineAddr = Util.HttpHelper.GetServerAddress(actionContext.Request);
                    srs.TraceId = reqHeader.TrackID;
                    srs.RpcId = reqHeader.RpcID;
                    srs.ServerHost = actionContext.Request.RequestUri.Host;

                    srs.InvokeID = string.Format("{0}_{1}", actionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower(), actionContext.ActionDescriptor.ActionName.ToLower());
                    if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
                    {
                        srs.Extension.Add(Util.ParamsKey, JsonConvert.SerializeObject(actionContext.ActionArguments));
                    }

                    //ServerRS Log Data TODO

                    Util.HttpHelper.SetHttpContextItem(ServerRSKey, srs);
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
            var attrs = actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<NonTracingAttribute>();
            if (attrs.Count == 0)
            {
                if (actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<NotToLogAttribute>().Count == 0)
                {
                    var request = actionExecutedContext.Request;
                    var srs = Util.HttpHelper.GetHttpContextItem<ServerRS>(ServerRSKey);

                    srs.EndTime = DateTime.Now;
                    srs.TimeLength = (srs.EndTime - srs.StartTime).TotalMilliseconds;
                    srs.IsException = false;
                    srs.IsSuccess = true;

                    IResponseModel value = null;
                    if (actionExecutedContext.Response != null && actionExecutedContext.Response.TryGetContentValue<IResponseModel>(out value))
                    {
                        srs.Code = value.GetCode();
                        srs.Extension.Add(Util.ResultKey, value);
                    }

                    //Exception
                    if (actionExecutedContext.Exception != null)
                    {
                        srs.IsException = true;
                        srs.IsSuccess = false;
                        srs.Extension.Add(Util.ExceptionKey, Util.GetFullExceptionMessage(actionExecutedContext.Exception));
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
