using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Helpers
{
    /// <summary>
    /// TracingContextHelper
    /// </summary>
    public static class TracingHelper
    {
        /// <summary>
        /// TraceContext
        /// </summary>
        public static TraceLogs TraceLogs
        {
            get
            {
                return Util.TracingContextHelper.GetContextItem<TraceLogs>(Config.ServerRSKey);
            }
        }

        /// <summary>
        /// TraceId
        /// </summary>
        public static string TraceId
        {
            get
            {
                var trace = Util.TracingContextHelper.GetContextItem<TraceLogs>(Config.ServerRSKey);
                if (trace != null)
                {
                    return trace.TraceId;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// AddTraceLogsExtensions
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool AddTraceLogsExtensions(string key, object val)
        {
            var trace = Util.TracingContextHelper.GetContextItem<TraceLogs>(Config.ServerRSKey);
            if (trace != null)
            {
                //Extensions Max Count
                if (trace.Extensions.Count < 50 && !trace.Extensions.ContainsKey(key))
                {
                    trace.Extensions[key] = val;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// RecordSystemLogs
        /// </summary>
        /// <param name="data"></param>
        public static void RecordSystemLogs(SystemLogs data)
        {
            data.SystemID = data.SystemID ?? EnvironmentConfig.SystemID;
            data.SystemName = data.SystemName ?? EnvironmentConfig.SystemName;
            data.Environment = data.Environment ?? EnvironmentConfig.Environment;
            data.TraceId = data.TraceId ?? TraceId;

            ServiceContainer.Resolve<ITracingRecord>().RecordSystemLogs(data);
        }

        ///// <summary>
        ///// Invoke
        ///// </summary>
        ///// <param name="data"></param>
        //public static TResponse Invoke<IRequest, TResponse>(Func<IRequest, TResponse> func, IRequest req, TraceLogsOptions optons, Action<TraceLogs> onRecord = null)
        //{
        //    TraceLogs trace = new TraceLogs();
        //    trace.ContextType = ContextType.Server.ToString();
        //    trace.Environment = optons.Environment;
        //    trace.StartTime = DateTime.Now;
        //    trace.MachineAddr = Util.TracingContextHelper.GetServerAddress();
        //    trace.TraceId = TracingHelper.TraceId;

        //    var rpcId = string.Empty;
        //    var modelHeader = TracingContextData.GetRequestHeader();
        //    if (modelHeader == null)
        //    {
        //        modelHeader = TracingContextData.GetDefaultRequestHeader();
        //        TracingContextData.SetRequestHeader(modelHeader);
        //        //HttpContentData.SetSubRpcID(modelHeader.RpcID + ".0");
        //        rpcId = modelHeader.RpcID + ".0";
        //    }
        //    else
        //    {
        //        rpcId = Util.VersionIncr(TracingContextData.GetSubRpcID());
        //    }
        //    TracingContextData.SetSubRpcID(rpcId);
        //    //trace.RpcId = reqHeader.RpcID;
        //    //trace.ServerHost = request.Url.Host;
        //    //trace.Protocol = request.Url.Scheme;

        //    trace.SystemID = optons.SystemID;
        //    trace.SystemName = optons.SystemName;

        //    TResponse res = default(TResponse);
        //    trace.StartTime = DateTime.Now;
        //    try
        //    {
        //        res = func(req);
        //        trace.IsSuccess = true;
        //        trace.IsException = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        trace.IsException = true;
        //        trace.IsSuccess = false;

        //        trace.Extensions.Add("Exception", Util.GetFullExceptionMessage(ex));
        //    }
        //    finally
        //    {
        //    }

        //    trace.Extensions.Add("RequestModel", Util.Clone<IRequest>(req));
        //    trace.Extensions.Add("ResponseModel", Util.Clone<TResponse>(res));
        //    trace.EndTime = DateTime.Now;
        //    trace.TimeLength = Math.Round((trace.EndTime - trace.StartTime).TotalMilliseconds, 4);

        //    onRecord?.Invoke(trace);

        //    return res;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public class TraceLogsOptions
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string SystemID;
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string SystemName;
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string Environment;

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string ServerHost;
        //}
    }
}
