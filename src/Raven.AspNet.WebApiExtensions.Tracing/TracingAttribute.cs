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

namespace Raven.AspNet.WebApiExtensions.Tracing
{
    /// <summary>
    /// Tracing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TracingAttribute : ActionFilterAttribute, IExceptionFilter
    {
        private const string ServerRSKey = "__raven_ServerRS";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var attrs = actionContext.ActionDescriptor.GetCustomAttributes<NonTracingAttribute>();
            if (attrs.Count > 0)
            {
                var request = actionContext.Request;
                RequestModel reqModel = null;
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
                        reqModel = dic.Value as RequestModel;
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

                    HttpContentData.SetSubRpcID(request, reqHeader.RpcID + ".0");
                    //HttpContentData.SubRpcID = reqHeader.RpcID + ".0";
                    //var header = HttpContentData.CloneRequestHeader(reqModel.Header);
                    //header.RpcID = header.RpcID + ".0";
                }
                else
                {
                    reqHeader = HttpContentData.GetDefaultRequestHeader();
                    HttpContentData.SetSubRpcID(request, "0");
                }
                HttpContentData.SetRequestHeader(request, reqHeader);
                //HttpContentData.RequestHeader = reqHeader;

                //Not To Log
                if (actionContext.ActionDescriptor.GetCustomAttributes<NotToLogAttribute>().Count > 0)
                {
                    ServerRS srs = new ServerRS();
                    srs.StartTime = DateTime.Now;
                    srs.MachineAddr = Util.HttpHelper.GetServerAddress(actionContext.Request);
                    srs.TraceId = reqHeader.TrackID;
                    srs.RpcId = reqHeader.RpcID;

                    srs.InvokeID = string.Format("{0}_{1}", actionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower(), actionContext.ActionDescriptor.ActionName);
                    
                    //ServerRS Log Data TODO

                    Util.HttpHelper.SetHttpContextItem(request, ServerRSKey, srs);
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
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<NotToLogAttribute>().Count > 0)
            {
                var request = actionExecutedContext.Request;
                var srs = Util.HttpHelper.GetHttpContextItem<ServerRS>(request, ServerRSKey);
            }

            await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<NotToLogAttribute>().Count > 0)
            {
                var request = actionExecutedContext.Request;
                var srs = Util.HttpHelper.GetHttpContextItem<ServerRS>(request, ServerRSKey);
            }
            throw new NotImplementedException();
        }
    }
}
