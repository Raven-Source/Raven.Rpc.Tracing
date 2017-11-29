using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class Generate
    {
        const long MaxCounter = 0xFFFFF;//每秒最多创建1048575个id
        static long _lastTimestamp = 0;//最后生成id时间戳
        static long _counter = 0;//每秒创建id数
        static long _timestampAndCounter = 0;//时间戳与创建id数
        static readonly DateTime _beginTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        static string _machineHash = "";
        static string _processId = "";
        static string _machineHashAndProcessId = "";
        //

        static Generate()
        {
            //初始化机器hash和进程id
            _machineHash = GetMachineHash();
            _processId = GetProcessId();
            _machineHashAndProcessId = _machineHash + _processId;
        }


        /// <summary>
        /// 生成Id
        /// </summary>
        /// <returns></returns>
        public static string GenerateId()
        {
            long timestamp = 0;
            long c = GetCounter(out timestamp);
            return string.Format("{0:x}{1}{2}", timestamp, _machineHashAndProcessId, c.ToString("x").PadLeft(5, '0'));
        }
        /// <summary>
        /// 获取id计数和时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private static long GetCounter(out long timestamp)
        {
            while (true)
            {
                long lastTimestampAndCounter = 0;
                long curTimestampAndCounter = 0;
                long lastTimestamp = 0;
                GetCounters(out lastTimestampAndCounter, out curTimestampAndCounter, out lastTimestamp, out timestamp);
                if (Interlocked.CompareExchange(ref _timestampAndCounter, curTimestampAndCounter, lastTimestampAndCounter) == lastTimestampAndCounter)
                {
                    if (timestamp > lastTimestamp)
                    {
                        Interlocked.Exchange(ref _counter, 1);
                        Interlocked.Exchange(ref _lastTimestamp, timestamp);
                        return 1;
                    }
                    else
                    {
                        return Interlocked.Increment(ref _counter);
                    }
                }
            }
        }

        private static void GetCounters(out long oldCounter, out long newCounter, out long lastTimestamp, out long now)
        {
            now = GetNowTimestamp();
            lastTimestamp = Interlocked.Read(ref _lastTimestamp);
            long counter = Interlocked.Read(ref _counter);
            oldCounter = (lastTimestamp << 20) | counter;

            long newc = 0;
            if (now > lastTimestamp)
                newc = 1;
            else
                newc = counter + 1;
            if (newc >= MaxCounter)
                throw new InvalidOperationException("execeed 1048575 id per second");
            newCounter = (now << 20) | newc;
        }

        private static long GetNowTimestamp()
        {
            return (long)(DateTime.UtcNow - _beginTime).TotalSeconds;
        }

                /// <summary>
        /// 获取服务器hash
        /// </summary>
        /// <remarks>
        /// md5({name}{mac}{ip}{machineIdentity})取后4个字节
        /// </remarks>
        /// <returns></returns>
        private static string GetMachineHash()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            string name = GetMachineName() ?? rand.Next().ToString("x");
            string mac = GetMacAddressByNetworkInformation() ?? rand.Next().ToString("x");
            string ip = GetIPAddress() ?? rand.Next().ToString("x");
            string machineIdentity = ConfigurationManager.AppSettings["machineIdentity"] ?? string.Empty;//若发生服务器hash冲突，可以通过配置调整
            string rowString = name + mac + ip + machineIdentity;
            //int c = Interlocked.Increment(ref testcounter);
            //string mac = "0011223344" + (c % 255).ToString("x").PadLeft(2, '0');
            //string ip = "0.0.0." + c % 255;
            //string rowString = string.Format("test-machine-{0}{1}{2}", c, mac, ip);
            var bytes = Md5(rowString);
            StringBuilder sb = new StringBuilder((bytes[12] & 0xF).ToString("x"));
            for (int i = 13; i < 16; i++)
            {
                byte b = bytes[i];
                sb.Append(b.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取进程Id
        /// </summary>
        /// <returns></returns>
        private static string GetProcessId()
        {
            int id = Process.GetCurrentProcess().Id;
            id = id & 0xFFFF;
            return id.ToString("x").PadLeft(4, '0');
        }

        private static byte[] Md5(string input)
        {
            byte[] result = Encoding.Default.GetBytes(input);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return output;
        }

        /// <summary>
        /// 获取机器名
        /// </summary>
        /// <returns></returns>
        private static string GetMachineName()
        {
            try
            {
                string strHostName = Dns.GetHostName();
                return strHostName;
            }
            catch { return null; }
        }

        /// <summary>  
        /// 通过网络适配器获取MAC地址  
        /// </summary>  
        /// <returns></returns>  
        private static string GetMacAddressByNetworkInformation()
        {
            string macAddress = null;
            try
            {
                NetworkInterface ni = null;
                List<NetworkInterface> nics = NetworkInterface.GetAllNetworkInterfaces().Where(i => i.OperationalStatus == OperationalStatus.Up && i.NetworkInterfaceType != NetworkInterfaceType.Loopback).ToList();
                if (nics.Count == 1)
                    ni = nics[0];
                else if (nics.Count > 1)
                    ni = nics.OrderByDescending(i => i.Speed).First();

                byte[] macByte = ni?.GetPhysicalAddress()?.GetAddressBytes();
                macAddress = ni?.GetPhysicalAddress()?.ToString();
            }
            catch
            {
            }
            return macAddress;
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        private static string GetIPAddress()
        {
            string strAddr = null;
            try
            {
                string strHostName = Dns.GetHostName(); //得到本机的主机名
                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP
                strAddr = ipEntry.AddressList.Where(ip => !IPAddress.IsLoopback(ip)).First().ToString();
            }
            catch { }
            return strAddr;
        }

    }


}
