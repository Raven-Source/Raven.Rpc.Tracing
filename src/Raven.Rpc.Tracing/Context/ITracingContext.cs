using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Context
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITracingContext
    {
        /// <summary>
        /// Raw Owin Environment dictionary.
        /// </summary>
        //object Environment { get; }

        /// <summary>
        /// Gets a key/value collection that can be used to organize and share data during an HTTP request.
        /// </summary>
        IDictionary<string, object> Items { get; }

        /// <summary>
        /// Gets the initial timestamp of the current HTTP request.
        /// </summary>
        DateTime Timestamp { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TracingContext : ITracingContext
    {
        //private static ConcurrentDictionary<long, IRequestScopeContext> contextDict = new ConcurrentDictionary<long, IRequestScopeContext>();

        /// <summary>
        /// 
        /// </summary>
        public const string CallContextKey = "raven_request_context";

        //internal static IRequestScopeContext GetCurrent()
        //{
        //    long key = (long)CallContext.LogicalGetData(CallContextKey);
        //    IRequestScopeContext context = null;
        //    contextDict.TryGetValue(key, out context);

        //    return context;
        //}

        //internal static void InitCurrent(ITracingContext context)
        //{
        //    long key = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
        //    CallContext.LogicalSetData(CallContextKey, key);
        //    contextDict.AddOrUpdate(key, x => context, (x, y) => context);

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //internal static void FreeContextSlot()
        //{
        //    long key = (long)CallContext.LogicalGetData(CallContextKey);
        //    contextDict.TryRemove(key, out IRequestScopeContext _);
        //    CallContext.FreeNamedDataSlot(CallContextKey);
        //}

        internal static void InitCurrent(IDictionary<string, object> environment, ITracingContext context)
        {
            //long key = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
            //CallContext.LogicalSetData(CallContextKey, key);
            //contextDict.AddOrUpdate(key, x => context, (x, y) => context);
            if (!environment.ContainsKey(CallContextKey))
            {
                environment[CallContextKey] = context;
            }
            else
            {
                Debug.WriteLine("TracingContext is exists");
            }
        }

        internal static void InitCurrent(IDictionary environment, ITracingContext context)
        {
            //long key = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
            //CallContext.LogicalSetData(CallContextKey, key);
            //contextDict.AddOrUpdate(key, x => context, (x, y) => context);

            if (!environment.Contains(CallContextKey))
            {
                environment[CallContextKey] = context;
            }
            else
            {
                Debug.WriteLine("TracingContext is exists");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void FreeContext(IDictionary<string, object> environment)
        {
            environment.Remove(CallContextKey);
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void FreeContext(IDictionary environment)
        {
            environment.Remove(CallContextKey);
        }

        /// <summary>
        /// 
        /// </summary>
        internal static ITracingContext GetContext(IDictionary<string, object> environment)
        {
            return (ITracingContext)environment[CallContextKey];
        }

        /// <summary>
        /// 
        /// </summary>
        internal static ITracingContext GetContext(IDictionary environment)
        {
            return (ITracingContext)environment[CallContextKey];
        }

        /// <summary>
        /// 
        /// </summary>
        readonly DateTime utcTimestamp = DateTime.UtcNow;
        //readonly List<UnsubscribeDisposable> disposables;
        //readonly ConcurrentQueue<UnsubscribeDisposable> disposablesThreadsafeQueue;

        ///// <summary>
        ///// 
        ///// </summary>
        //public object Environment { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Items { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Timestamp { get { return utcTimestamp.ToLocalTime(); } }

        /// <summary>
        /// 
        /// </summary>
        public TracingContext()
        {
            this.utcTimestamp = DateTime.UtcNow;
            this.Items = new Dictionary<string, object>();
        }

    }
}
