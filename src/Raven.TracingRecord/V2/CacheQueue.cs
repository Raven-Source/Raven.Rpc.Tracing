using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.TracingRecord.V2
{
    public class CacheQueue<T>
    {
        private int _currentCount = 0;
        readonly BlockingCollection<T> _firstCache;
        readonly List<T> _lastCache;
        readonly int _max;
        readonly object _lock;
        DateTime _lastTime;
        readonly CancellationTokenSource _token;
        readonly int _interval;
        readonly Func<List<T>,Task> _saveMsg;
        /// <summary>
        /// 缓冲队列
        /// </summary>
        /// <param name="interval">主动写入时间间隔（毫秒）</param>
        /// <param name="max">达到写入数量</param>
        /// <param name="saveMsg">保存消息委托</param>
        public CacheQueue(int interval=2000,int max=200, Func<List<T>, Task> saveMsg =null)
        {
            _max = max;
            _interval = interval;
            _saveMsg = saveMsg;
            _token = new CancellationTokenSource();
            _firstCache = new BlockingCollection<T>();
            _lastCache = new List<T>();
            Task.Factory.StartNew( ExchangeCache);
            Task.Factory.StartNew(async () =>
            {
                await TimeSave();
            });
            _lock = new object();
            _lastTime = DateTime.Now;
        }

        public void Enqueue(T msg)
        {
            _firstCache.Add(msg, _token.Token);
        }

         async Task ExchangeCache()
        {
            try
            {
                foreach (var msg in _firstCache.GetConsumingEnumerable(_token.Token))
                {
                    _lastCache.Add(msg);
                    Interlocked.Increment(ref _currentCount);
                    _lastTime = DateTime.Now;
                    if (_currentCount >= _max)
                    {
                         await SaveMsg();
                    }
                }
            }
            catch (Exception)
            {
                //
            }
            Console.WriteLine("Exchange Task Done!");
        }

        async Task SaveMsg()
        {
            var list = new List<T>(_lastCache.Count);
            lock (_lock)
            {
                if (_lastCache.Count > 0)
                {
                    _lastCache.ForEach(list.Add);
                    _lastCache.Clear();
                    Interlocked.Exchange(ref _currentCount, 0);
                }
            }
            if (list.Count > 0&&_saveMsg!=null)
                await _saveMsg(list);
        }

        async Task TimeSave()
        {
            try
            {
                while (!_token.IsCancellationRequested)
                {
                    if (_lastTime.AddMilliseconds(_interval) <= DateTime.Now)
                    {
                        await SaveMsg();
                    }
                    await Task.Delay(_interval, _token.Token);
                }
            }
            catch (Exception)
            {
                //
            }
            Console.WriteLine("TimeSave Task Done!");
        }

        public void Stop()
        {
            _token.Cancel();
        }

    }
}
