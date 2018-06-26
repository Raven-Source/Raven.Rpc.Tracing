using Raven.Rpc.Tracing.Context;
using Raven.Rpc.Tracing.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class TracingContextHelper
    {
        public const string RequestHeaderKey = "__raven_RequestHeader";
        public const string SubRpcIDKey = "__raven_SubRpcID";
        public const string LocalAddressKey = "__localAddress";
        public const string TraceKey = "__raven_Trace";
        //private const string TraceIDKey = "__raven_TraceID";

        /// <summary>
        ///                              
        /// </summary>
        /// <returns></returns>
        public static Rpc.IContractModel.Header GetRequestHeader(this ITracingContext context)
        {
            //var requestHeader = HttpContext.Current.Items[RequestHeaderKey];
            var requestHeader = context.GetContextItem<Rpc.IContractModel.Header>(RequestHeaderKey);
            if (requestHeader != null)
                return requestHeader as Rpc.IContractModel.Header;
            else return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        public static void SetRequestHeader(this ITracingContext context, Rpc.IContractModel.Header header)
        {
            context.SetContextItem(RequestHeaderKey, header);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SerialVersion GetSubRpcID(this ITracingContext context)
        {
            var subRpcID = context.GetContextItem<SerialVersion>(SubRpcIDKey);
            if (subRpcID == null)
            {
                subRpcID = SerialVersion.GetDefalut();
                context.SetContextItem(SubRpcIDKey, subRpcID);
                //HttpContext.Current.Items[SubRpcIDKey] = subRpcID;
            }
            return subRpcID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public static void SetSubRpcID(this ITracingContext context, SerialVersion val)
        {
            context.SetContextItem(SubRpcIDKey, val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Rpc.IContractModel.Header GetDefaultRequestHeader(this ITracingContext context)
        {
            return new Rpc.IContractModel.Header()
            {
                RpcID = "0",
                TraceID = Util.GetGenerateId()// Generate.GenerateId() //Util.GetUniqueCode32()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trace"></param>
        public static void SetTraceLogs(this ITracingContext context, TraceLogs trace)
        {
            context.SetContextItem(TraceKey, trace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TraceLogs GetTraceLogs(this ITracingContext context)
        {
            return context.GetContextItem<TraceLogs>(TraceKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetTraceId(this ITracingContext context)
        {
            var trace = GetTraceLogs(context);
            if (trace != null)
            {
                return trace.TraceId;
            }
            return string.Empty;
        }

        /// <summary>
        /// AddTraceLogsExtensions
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool AddTraceLogsExtensions(this ITracingContext context, string key, object val)
        {
            var trace = GetTraceLogs(context);
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
        public static void RecordSystemLogs(this ITracingContext context, SystemLogs data)
        {
            data.SystemID = data.SystemID ?? EnvironmentConfig.SystemID;
            data.SystemName = data.SystemName ?? EnvironmentConfig.SystemName;
            data.Environment = data.Environment ?? EnvironmentConfig.Environment;
            data.TraceId = data.TraceId ?? GetTraceId(context);

            ServiceContainer.Resolve<ITracingRecord>().RecordSystemLogs(data);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static string GetServerAddress(this ITracingContext context)
        //{
        //    return this.requestContextHelper.GetContextItem<string>(LocalAddressKey);

        //}
    }
}
