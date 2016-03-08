using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    public static class RabbitMQConfig
    {
        public static readonly string hostName;
        public static readonly string username;
        public static readonly string password;

        static RabbitMQConfig()
        {
            string[] str = ConfigurationManager.AppSettings["RabbitMQ"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string b in str)
            {
                var temp = b.Split('=');
                if (temp[0] == "host")
                {
                    hostName = temp[1];
                }
                else if (temp[0] == "username")
                {
                    username = temp[1];
                }
                else if (temp[0] == "pwd")
                {
                    password = temp[1];
                }
            }
        }
    }
}
