using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    internal class EnvironmentConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static string Environment
        {
            get;
            internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string SystemID
        {
            get;
            internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string SystemName
        {
            get;
            internal set;
        }
    }
}
