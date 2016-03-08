using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    public abstract class Handle
    {
        Loger loger = new Loger();

        /// <summary>
        /// 同步线程通知事件
        /// </summary>
        public System.Threading.AutoResetEvent Reset { get; set; }
        /// <summary>
        /// 同步线程通知事件
        /// </summary>
        public System.Threading.AutoResetEvent Reset2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected Thread thread = null;
        protected bool isRun = false;
        protected string serverName;
        protected int intervalMilliseconds;

        private bool isFirst = true;

        protected abstract Action ProcessWorkAction { get; }

        public Handle(string serverName, int intervalMilliseconds = 1000)
        {
            Reset = new AutoResetEvent(false);
            Reset2 = new AutoResetEvent(false);
            this.serverName = serverName;
            this.intervalMilliseconds = intervalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual TimeSpan GetIntervalMilliseconds()
        {
            return TimeSpan.FromMilliseconds(this.intervalMilliseconds);
        }

        /// <summary>
        /// 运行
        /// </summary>
        public virtual void Run()
        {
            if (!isRun)
            {
                isRun = true;

                thread = new Thread(() => ProcessWork());
                thread.IsBackground = true;
                thread.Start();

                loger.LogInfo(serverName + "服务启动");
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {
            isRun = false;
            Reset.Set();
            Reset2.WaitOne();
        }

        /// <summary>
        /// 处理
        /// </summary>
        public void ProcessWork()
        {
            Reset2.Reset();
            while (isRun)
            {
                try
                {
                    if (!isFirst)
                    {
                        ProcessWorkAction();
                    }

                    if (isRun)
                    {
                        Reset.WaitOne(GetIntervalMilliseconds());
                    }
                }
                catch (Exception ex)
                {
                    loger.LogInfo("错误了");
                }

                isFirst = false;
            }

            loger.LogInfo(serverName + "服务停止");
            Reset2.Set();
        }
    }

}
