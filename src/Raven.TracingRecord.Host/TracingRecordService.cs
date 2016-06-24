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
    public partial class TracingRecordService : ServiceBase
    {
        public TracingRecordService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            RecordHandle.GetInstance.Run();
        }

        protected override void OnStop()
        {
            RecordHandle.GetInstance.Stop();
        }
    }
}
