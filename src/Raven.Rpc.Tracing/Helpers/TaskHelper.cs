using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Helpers
{
    public static class TaskHelper
    {
        private static readonly Task<object> _completedTaskReturningNull = Task.FromResult<object>(null);
        private static readonly Task _defaultCompleted = Task.FromResult<AsyncVoid>(new AsyncVoid());

        public static Task Completed()
        {
            return _defaultCompleted;
        }

        public static Task FromError(Exception exception)
        {
            return FromError<AsyncVoid>(exception);
        }

        public static Task<TResult> FromError<TResult>(Exception exception)
        {
            TaskCompletionSource<TResult> source = new TaskCompletionSource<TResult>();
            source.SetException(exception);
            return source.Task;
        }

        public static Task<object> NullResult()
        {
            return _completedTaskReturningNull;
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        private struct AsyncVoid
        {
        }        

    }
}
