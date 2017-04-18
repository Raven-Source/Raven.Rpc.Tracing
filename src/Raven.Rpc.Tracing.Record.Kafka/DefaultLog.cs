using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record
{
    
    /// <summary>
    /// 
    /// </summary>
    public class DefaultLog : Message.Kafka.Abstract.ILog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        public void Debug(string format, params object[] pars)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception ex)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        public void Error(string format, params object[] pars)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        public void Info(string format, params object[] pars)
        {
            
        }
    }
}
