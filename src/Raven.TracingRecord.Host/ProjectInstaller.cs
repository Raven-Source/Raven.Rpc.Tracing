using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Raven.TracingRecord.Host
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            this.serviceInstaller1.ServiceName = IniFileHelper.ReadFile("ServiceName");
            this.serviceInstaller1.DisplayName = IniFileHelper.ReadFile("DisplayName");
        }
    }
}
