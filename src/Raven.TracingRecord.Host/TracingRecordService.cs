using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord.Host
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TracingRecordService : ServiceBase
    {
        /// <summary>
        /// 
        /// </summary>
        public TracingRecordService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            TraceLogsRecordHandleV1.GetInstance.Run();
            //TraceLogsRecordHandle.GetInstance.Run();
            //SystemLogRecordHandle.GetInstance.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStop()
        {
            TraceLogsRecordHandleV1.GetInstance.Stop();
            //TraceLogsRecordHandle.GetInstance.Stop();
            //SystemLogRecordHandle.GetInstance.Stop();
        }
    }
}
