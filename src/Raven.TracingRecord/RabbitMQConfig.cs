using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.TracingRecord
{
    public class RabbitMQConfig
    {
        public readonly string hostName;
        public readonly string username;
        public readonly string password;

        public RabbitMQConfig(string appSettingsKey)
        {
            string[] str = ConfigurationManager.AppSettings[appSettingsKey].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
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
