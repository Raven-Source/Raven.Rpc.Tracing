
using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {
        private static Lazy<ITracingContextHelper> _tracingContextHelper = new Lazy<ITracingContextHelper>(() => ServiceContainer.Resolve<ITracingContextHelper>());
        /// <summary>
        /// 
        /// </summary>
        public static ITracingContextHelper TracingContextHelper
        {
            get
            {
                return _tracingContextHelper.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static IDataSerializer serializer = Raven.Serializer.SerializerFactory.Create(SerializerType.NewtonsoftJson);

        ///// <summary>
        ///// 注册
        ///// </summary>
        ///// <param name="httpHelper"></param>
        //public static void Register(IHttpHelper httpHelper)
        //{
        //    ServiceContainer.Register(httpHelper);
        //}

        ///// <summary>
        ///// 获取32位唯一字符串
        ///// </summary>
        ///// <returns></returns>
        //public static string GetUniqueCode32()
        //{
        //    var guid1 = GetGuidArray();
        //    var guid2 = GetGuidArray();
        //    var token1 = Convert.ToBase64String(guid1).TrimEnd('=').Replace("/", "_").Replace("+", "-");
        //    var token2 = Convert.ToBase64String(guid2).TrimEnd('=').Replace("/", "_").Replace("+", "-");
        //    return (token1 + token2).Substring(0, 32);
        //}

        ///// <summary>
        ///// 获取32位唯一字符串
        ///// </summary>
        ///// <returns></returns>
        //public static string GetUniqueCode22()
        //{
        //    var guid1 = GetGuidArray();
        //    var token1 = Convert.ToBase64String(guid1).TrimEnd('=').Replace("/", "_").Replace("+", "-");
        //    return token1;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetGuidArray()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            var baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return guidArray;
        }

        /// <summary>
        /// 版本号自增（eg:1.4.3）
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string VersionIncr(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "1";

            var i = val.LastIndexOf('.');
            if (i >= 0)
            {
                var ver = val.Substring(i + 1);
                if (string.IsNullOrEmpty(ver))
                {
                    val = val.Substring(0, i + 1) + "1";
                }
                else
                {
                    val = val.Substring(0, i + 1) + (int.Parse(ver) + 1);
                }
                return val;
            }
            else
            {
                return (int.Parse(val) + 1).ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetFullExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            if (ex != null)
            {
                if (!string.IsNullOrWhiteSpace(ex.Message))
                {
                    sb.Append(ex.Message + "\r\n");
                }
                if (!string.IsNullOrWhiteSpace(ex.StackTrace))
                {
                    sb.Append(ex.StackTrace + "\r\n");
                }
            }

            if (ex is AggregateException)
            {
                var exList = ex as AggregateException;
                foreach (var e in exList.InnerExceptions)
                {
                    sb.Append(GetFullExceptionMessage(e));
                }
            }

            if (ex.InnerException != null)
            {
                sb.Append(GetFullExceptionMessage(ex.InnerException));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetThreadPool()
        {
            int workerThreads, completionPortThreads;
            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            if (workerThreads < defaultMinThreads)
            {
                workerThreads = defaultMinThreads;
            }
            if (completionPortThreads < defaultMinThreads)
            {
                completionPortThreads = defaultMinThreads;
            }
            ThreadPool.SetMinThreads(workerThreads, completionPortThreads);
        }

        private static int defaultMinThreads = Environment.ProcessorCount * 6 + 2;

        //public static string GetServerAddress(HttpRequestMessage request)
        //{
        //    //is owin
        //    if (System.Web.HttpContext.Current == null)
        //    {
        //        request.GetOwinContext()
        //    }
        //}

        const int MaxSerializerStringSize = 1024 * 512;   //char size, 1MB

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializerObjToString(object obj)
        {
            if (obj == null)
                return string.Empty;

            var res = Serializer(obj);
            if (res.Length > MaxSerializerStringSize)
            {
                res = string.Format("Size {0} is larger than MaxSize {1}", res.Length * 2, MaxSerializerStringSize);
            }
            return res;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string Serializer(object obj)
        {
            if (obj is string)
                return (string)obj;

            if (serializer is Raven.Serializer.IStringDataSerializer stringSerializer)
            {
                return stringSerializer.SerializeToString(obj);
            }
            else
            {
                throw new Exception("update packages: Raven.Serializer.WithNewtonsoft >= 1.1.0");
            }
            //var data = serializer.Serialize(obj);
            //Encoding encoding = Encoding.UTF8;
            //var res = encoding.GetString(data);
            //return res;
        }
        
    }
}
