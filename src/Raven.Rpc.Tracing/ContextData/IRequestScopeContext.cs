using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.ContextData
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestScopeContext
    {
        /// <summary>
        /// Raw Owin Environment dictionary.
        /// </summary>
        object Environment { get; }

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
    public class RequestScopeContext : IRequestScopeContext
    {
        const string CallContextKey = "raven_request_context";

        /// <summary>
        /// 
        /// </summary>
        public static IRequestScopeContext Current
        {
            get
            {
                return (IRequestScopeContext)CallContext.LogicalGetData(CallContextKey);
            }
            set
            {
                CallContext.LogicalSetData(CallContextKey, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void FreeContextSlot()
        {
            CallContext.FreeNamedDataSlot(CallContextKey);
        }
        
        /// <summary>
        /// 
        /// </summary>
        readonly DateTime utcTimestamp = DateTime.UtcNow;
        //readonly List<UnsubscribeDisposable> disposables;
        //readonly ConcurrentQueue<UnsubscribeDisposable> disposablesThreadsafeQueue;

        /// <summary>
        /// 
        /// </summary>
        public object Environment { get; private set; }

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
        public RequestScopeContext(object environment)
        {
            this.utcTimestamp = DateTime.UtcNow;
            this.Environment = environment;
            this.Items = new Dictionary<string, object>();
        }

        //public IDisposable DisposeOnPipelineCompleted(IDisposable target)
        //{
        //    if (target == null) throw new ArgumentNullException("target");

        //    var token = new UnsubscribeDisposable(target);
        //    if (disposables != null)
        //    {
        //        disposables.Add(token);
        //    }
        //    else
        //    {
        //        disposablesThreadsafeQueue.Enqueue(token);
        //    }
        //    return token;
        //}

        //internal void Complete()
        //{
        //    var exceptions = new List<Exception>();
        //    try
        //    {
        //        if (disposables != null)
        //        {
        //            foreach (var item in disposables)
        //            {
        //                item.CallTargetDispose();
        //            }
        //        }
        //        else
        //        {
        //            UnsubscribeDisposable target;
        //            while (disposablesThreadsafeQueue.TryDequeue(out target))
        //            {
        //                target.CallTargetDispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        exceptions.Add(ex);
        //    }
        //    finally
        //    {
        //        if (exceptions.Any())
        //        {
        //            throw new AggregateException("failed on disposing", exceptions);
        //        }
        //    }
        //}

    }
}
