using System;
using System.Collections.Generic;
using System.IO;
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
        string root = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private static Dictionary<string, object> _obj = new Dictionary<string, object>();

        public DefaultLog()
        {
            root = root.Remove(root.LastIndexOf('\\') + 1);

            var date = DateTime.Now.Date.AddDays(-7);
            string path = root + "Log/";
            DeleteOldLog(path, date, 1);
        }

        /// <summary>
        /// 清除旧日志
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folder"></param>
        private void DeleteOldLog(string path, DateTime date, int folderIndex)
        {
            int folder;
            switch (folderIndex)
            {
                case 1:
                    folder = date.Year;
                    break;
                case 2:
                    folder = date.Month;
                    break;
                case 3:
                    folder = date.Day;
                    break;
                default:
                    return;
            }

            var dire = new DirectoryInfo(path);
            if (dire == null || !dire.Exists)
            {
                return;
            }

            foreach (var d in dire.GetDirectories())
            {
                int temp;
                if (int.TryParse(d.Name, out temp))
                {
                    if (temp < folder)
                    {
                        Directory.Delete(d.FullName, true);
                    }
                    else if (temp == folder)
                    {
                        DeleteOldLog(d.FullName, date, folderIndex + 1);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        public void Debug(string format, params object[] pars)
        {
#if DEBUG
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(string.Format(format, pars));
#endif

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception ex)
        {
#if DEBUG
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(ex.Message);
            if (ex != null)
            {
                Console.WriteLine(ex.StackTrace);
            }
#else
            var fileName = "error.txt";
            if (ex is System.Net.Sockets.SocketException)
            {
                fileName = "socket.txt";
            }
            else if (ex is System.IO.IOException)
            {
                fileName = "io.txt";
            }

            string message = "";
            if (ex != null)
            {
                message = ex.Message;
                message += "\r\n" + ex.StackTrace;
                if (ex.InnerException != null)
                {
                    message += "\r\n内部错误：" + ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace;
                }
            }

            SaveLog(fileName, message);
#endif

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        public void Error(string format, params object[] pars)
        {
#if DEBUG
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(string.Format(format, pars));
#else
            var fileName = "error.txt";

            string message = "";
            if (format != null)
            {
                message = format;
            }

            SaveLog(fileName, message);
#endif

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        public void Info(string format, params object[] pars)
        {
#if DEBUG
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(string.Format(format, pars));
#endif
        }


        private void SaveLog(string fileName, string message)
        {
            DateTime nowTime = DateTime.Now;
            message = "\r\n" + nowTime.ToString("yyyy-MM-dd HH:mm:ss") + "-----------------------------------\r\n" + message;

            string path = root + "Log\\" + string.Format("{0}\\{1}\\{2}\\", nowTime.Year, nowTime.Month, nowTime.Day);// + DateTime.Now.ToString("yyyy/MM/dd/");

            path = System.IO.Path.GetFullPath(path);

            Action<object> act = (o) =>
            {
                if (!_obj.ContainsKey(fileName))
                {
                    _obj[fileName] = new object();
                }

                lock (_obj[fileName])
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = path + fileName;
                    try
                    {
                        File.AppendAllText(path, message);
                    }
                    catch { }
                }
            };

            if (!System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(act)))
            {
                act(null);
            }
        }
    }
}
