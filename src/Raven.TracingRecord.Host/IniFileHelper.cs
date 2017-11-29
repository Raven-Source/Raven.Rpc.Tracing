using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven.TracingRecord.Host
{
    public static class IniFileHelper
    {
        static string root = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string path = root.Remove(root.LastIndexOf('\\') + 1) + "config.ini";
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public static string ReadFile(string key)
        {
            IniFile ini = new IniFile(path);
            return ini.IniReadValue("setting", key);
        }
        /// <summary>
        /// 更改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void UpdateFile(string key, string value)
        {
            IniFile ini = new IniFile(path);
            ini.IniWriteValue("setting", key, value);
        }

    }
}
