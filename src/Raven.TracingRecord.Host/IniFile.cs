using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Raven.TracingRecord.Host
{
    public class IniFile
    {
        private static object lockObj = new object();
        public string path;　　 //INI文件路径

        private Dictionary<string, Dictionary<string, string>> InitIniFileContent(string filePath)
        {
            Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                string returnedString = null;

                if (File.Exists(filePath))
                {
                    returnedString = File.ReadAllText(filePath, Encoding.UTF8);
                }
                else
                {
                    return dict;
                }

                var items = returnedString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (items != null && items.Length > 0)
                {
                    //dict = new Dictionary<string, Dictionary<string, string>>();
                    string key = "";
                    for (var i = 0; i < items.Length; i++)
                    {
                        var item = items[i].Trim();
                        if (item.Length >= 3 && item.FirstOrDefault() == '[' && item.LastOrDefault() == ']')
                        {
                            key = item.Substring(1, item.Length - 2);
                        }
                        else
                        {
                            if (!dict.ContainsKey(key))
                            {
                                dict[key] = new Dictionary<string, string>();
                            }

                            var index = item.IndexOf('=');
                            if (index > -1)
                            {
                                dict[key].Add(item.Substring(0, index), item.Substring(index + 1));
                            }
                        }

                    }
                }

                return dict;
            }
            catch
            {
                return dict;
            }

        }

        /// <summary>写入ini
        /// 写入ini
        /// </summary>
        /// <param name="section">section名</param>
        /// <param name="key">key值</param>
        /// <param name="val">value值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        private void WritePrivateProfileString(string section, string key, string val, string filePath)
        {
			val = val ?? "";
            if (!string.IsNullOrWhiteSpace(key))
            {
                Dictionary<string, Dictionary<string, string>> dict = InitIniFileContent(filePath);

                if (!dict.ContainsKey(section))
                {
                    dict[section] = new Dictionary<string, string>();
                }

                dict[section][key] = val;

                StringBuilder sb = new StringBuilder();
                foreach (var d in dict)
                {
                    sb.Append(string.Concat("[", d.Key, "]", "\r\n"));
                    foreach (var kv in d.Value)
                    {
                        sb.Append(string.Concat(kv.Key, "=", kv.Value, "\r\n"));
                    }
                    sb.Append("\r\n");
                }

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            }
        }

        /// <summary>将ini数据一一对应
        /// 将ini数据一一对应
        /// </summary>
        /// <param name="listKeys">键名列表</param>
        /// <param name="listValues">键值列表</param>
        /// <returns></returns>
        private List<IniKey> GetListIniKey(List<string> listKeys, List<string> listValues)
        {
            List<IniKey> listIniKey = new List<IniKey>();
            IniKey modelKey;
            for (int i = 0; i < listKeys.Count; i++)
            {
                modelKey = new IniKey();
                modelKey.Key = listKeys[i];
                modelKey.Value = listValues[i];
                listIniKey.Add(modelKey);
            }
            return listIniKey;
        }

        /// <summary>类的构造函数，传递INI文件名
        /// 类的构造函数，传递INI文件名
        /// </summary>
        /// <param name="INIPath">ini文件路径</param>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        /// <summary> 写INI文件
        ///  写INI文件
        /// </summary>
        /// <param name="Section">键位</param>
        /// <param name="Key">键名</param>
        /// <param name="Value">键值</param>
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>读取INI文件指定
        /// 读取INI文件指定
        /// </summary>
        /// <param name="Section">键位</param>
        /// <param name="Key">键名</param>
        /// <returns>键值</returns>
        public string IniReadValue(string section, string key)
        {
            string val = null;
            if (!string.IsNullOrWhiteSpace(key))
            {
                Dictionary<string, Dictionary<string, string>> dict = InitIniFileContent(path);

                if (dict.ContainsKey(section))
                {
                    if (dict[section].ContainsKey(key))
                    {
                        val = dict[section][key];
                    }
                }
            }
            return val;
        }

        /// <summary>根据section取所有keyList
        /// 根据section取所有keyList
        /// </summary>
        /// <param name="section">section</param>
        /// <returns>keyList</returns>
        public List<IniKey> GetSectionIniList(string section)
        {
            List<IniKey> listIniKey = new List<IniKey>();

            Dictionary<string, Dictionary<string, string>> dict = InitIniFileContent(path);

            if (dict.ContainsKey(section))
            {
                foreach (var d in dict[section])
                {
                    listIniKey.Add(new IniKey(d.Key, d.Value));
                }

            }

            return listIniKey;
        }

    }

    /// <summary>
    /// 字符串键值对
    /// </summary>
    public class IniKey : KeyValue<string, string>
    {
        public IniKey()
        { }

        public IniKey(string key, string val)
        {
            base.Key = key;
            base.Value = val;
        }
    }


}
