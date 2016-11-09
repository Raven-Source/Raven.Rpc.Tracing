using Raven.Rpc.IContractModel;
using Raven.Rpc.Tracing;
using Raven.Rpc.Tracing.ContextData;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Raven.AspNet.MvcExtensions.Tracing
{
    /// <summary>
    /// 
    /// </summary>
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
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ServiceContainer.Resolve<IInitRequestScopeContext>().BeginRequest(filterContext.HttpContext.Request);

            if (!filterContext.HasMarkerAttribute<NonTracingAttribute>())
            {
                var request = filterContext.HttpContext.Request;
                var response = filterContext.HttpContext.Response;

                Header reqHeader = HttpContextData.GetDefaultRequestHeader();

                HttpContextData.SetSubRpcID(reqHeader.RpcID + ".0");
                HttpContextData.SetRequestHeader(reqHeader);

                if (!filterContext.HasMarkerAttribute<NotToLogAttribute>())
                {
                    TraceLogs trace = new TraceLogs();
                    trace.ContextType = ContextType.Server.ToString();
                    trace.Environment = this.environment;
                    trace.StartTime = DateTime.Now;
                    trace.MachineAddr = Util.HttpHelper.GetServerAddress();
                    trace.TraceId = reqHeader.TraceID;
                    trace.RpcId = reqHeader.RpcID;
                    trace.ServerHost = request.Url.Host;
                    trace.Protocol = request.Url.Scheme;

                    trace.SystemID = this.systemID;
                    trace.SystemName = this.systemName;

                    //srs.InvokeID = string.Format("{0}_{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower(), filterContext.ActionDescriptor.ActionName.ToLower());

                    //InvokeID
                    trace.InvokeID = request.Url.AbsolutePath;
                    string val = filterContext.HttpContext.Request.Headers[Config.ResponseHeaderFolderKey];
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        trace.InvokeID = val.FirstOrDefault() + trace.InvokeID;
                    }
                    
                    TraceExtensionOnActionExecuting(filterContext, trace);

                    Util.HttpHelper.SetHttpContextItem(Config.ServerRSKey, trace);
                }
            }

            base.OnActionExecuting(filterContext);
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="trace"></param>
        protected virtual void TraceExtensionOnActionExecuting(ActionExecutingContext filterContext, TraceLogs trace)
        {
            trace.Extensions.Add(nameof(filterContext.HttpContext.Request.Url.PathAndQuery), filterContext.HttpContext.Request.Url.PathAndQuery);

            if (filterContext.ActionParameters != null && filterContext.ActionParameters.Count > 0)
            {
                trace.Extensions.Add(Config.ParamsKey, filterContext.ActionParameters);
            }

            var form = filterContext.HttpContext.Request.Form;
            if (form != null)
            {
                trace.Extensions.Add(Config.FormKey, form);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HasMarkerAttribute<NonTracingAttribute>())
            {
                if (!filterContext.HasMarkerAttribute<NotToLogAttribute>())
                {
                    var request = filterContext.HttpContext.Request;
                    var trace = Util.HttpHelper.GetHttpContextItem<TraceLogs>(Config.ServerRSKey);

                    trace.EndTime = DateTime.Now;
                    trace.TimeLength = (trace.EndTime - trace.StartTime).TotalMilliseconds;
                    trace.IsException = false;
                    trace.IsSuccess = true;

                    if (filterContext.HttpContext.Response != null)
                    {
                        filterContext.HttpContext.Response.Headers.Add(Config.ResponseHeaderTraceKey, trace.TraceId);
                    }

                    //Exception
                    if (filterContext.Exception != null)
                    {
                        trace.IsException = true;
                        trace.IsSuccess = false;
                        trace.Extensions.Add(Config.ExceptionKey, Util.GetFullExceptionMessage(filterContext.Exception));
                    }

                    TraceExtensionOnActionExecuted(filterContext, trace);

                    Record(trace);
                }
            }

            ServiceContainer.Resolve<IInitRequestScopeContext>().EndRequest(filterContext.HttpContext.Request);
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="trace"></param>
        protected virtual void TraceExtensionOnActionExecuted(ActionExecutedContext filterContext, TraceLogs trace)
        {
            var jResult = filterContext.Result as JsonResult;
            if (jResult != null)
            {
                var responseModel = jResult.Data as IResponseModel;
                if (responseModel != null)
                {
                    trace.Code = responseModel.GetCode();

                    //if (responseModel.Extension == null)
                    //{
                    //    responseModel.Extension = new List<Rpc.IContractModel.KeyValue<string, string>>();
                    //}
                    //responseModel.Extension.Add(new Rpc.IContractModel.KeyValue<string, string>(nameof(Raven.Rpc.IContractModel.Header.TraceID), HttpContentData.GetRequestHeader().TraceID));
                }
                trace.Extensions.Add(Config.ResultKey, jResult.Data);
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
