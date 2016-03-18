using Raven.Rpc.IContractModel;
using Raven.Rpc.Tracing;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HasMarkerAttribute<NonTracingAttribute>())
            {
                var request = filterContext.HttpContext.Request;
                var response = filterContext.HttpContext.Response;

                Header reqHeader = HttpContentData.GetDefaultRequestHeader();

                HttpContentData.SetSubRpcID(reqHeader.RpcID + ".0");
                HttpContentData.SetRequestHeader(reqHeader);

                if (!filterContext.HasMarkerAttribute<NotToLogAttribute>())
                {
                    ServerRS srs = new ServerRS();
                    srs.StartTime = DateTime.Now;
                    srs.MachineAddr = Util.HttpHelper.GetServerAddress();
                    srs.TraceId = reqHeader.TraceID;
                    srs.RpcId = reqHeader.RpcID;
                    srs.ServerHost = request.Url.Authority;
                    srs.Protocol = request.Url.Scheme;

                    srs.SystemID = this.systemID;
                    srs.SystemName = this.systemName;

                    //srs.InvokeID = string.Format("{0}_{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower(), filterContext.ActionDescriptor.ActionName.ToLower());

                    srs.InvokeID = request.Url.AbsolutePath;
                    srs.Extension.Add(nameof(request.Url.PathAndQuery), request.Url.PathAndQuery);

                    if (filterContext.ActionParameters != null && filterContext.ActionParameters.Count > 0)
                    {
                        srs.Extension.Add(Config.ParamsKey, filterContext.ActionParameters);
                    }

                    Util.HttpHelper.SetHttpContextItem(Config.ServerRSKey, srs);
                }
            }


            base.OnActionExecuting(filterContext);
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
                    var srs = Util.HttpHelper.GetHttpContextItem<ServerRS>(Config.ServerRSKey);

                    srs.EndTime = DateTime.Now;
                    srs.TimeLength = (srs.EndTime - srs.StartTime).TotalMilliseconds;
                    srs.IsException = false;
                    srs.IsSuccess = true;

                    var jResult = filterContext.Result as JsonResult;
                    if (jResult != null)
                    {
                        var responseModel = jResult.Data as IResponseModel;
                        if (responseModel != null)
                        {
                            srs.Code = responseModel.GetCode();
                            srs.Extension.Add(Config.ResultKey, jResult.Data);

                            //if (responseModel.Extension == null)
                            //{
                            //    responseModel.Extension = new List<Rpc.IContractModel.KeyValue<string, string>>();
                            //}
                            //responseModel.Extension.Add(new Rpc.IContractModel.KeyValue<string, string>(nameof(Raven.Rpc.IContractModel.Header.TraceID), HttpContentData.GetRequestHeader().TraceID));
                        }
                    }

                    if (filterContext.HttpContext.Response != null)
                    {
                        filterContext.HttpContext.Response.Headers.Add(Config.ResponseHeaderTraceKey, HttpContentData.GetRequestHeader().TraceID);
                    }

                    //Exception
                    if (filterContext.Exception != null)
                    {
                        srs.IsException = true;
                        srs.IsSuccess = false;
                        srs.Extension.Add(Config.ExceptionKey, Util.GetFullExceptionMessage(filterContext.Exception));
                    }

                    Record(srs);
                }
            }

            base.OnActionExecuted(filterContext);
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
