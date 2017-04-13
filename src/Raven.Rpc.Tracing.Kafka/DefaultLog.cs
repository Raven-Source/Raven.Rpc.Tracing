using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Kafka
{
    public class DefaultLog : Message.Kafka.Abstract.ILog
    {
        public void Debug(string format, params object[] pars)
        {
            
        }

        public void Error(Exception ex)
        {
            
        }

        public void Error(string format, params object[] pars)
        {
            
        }

        public void Info(string format, params object[] pars)
        {
            
        }
    }
}
