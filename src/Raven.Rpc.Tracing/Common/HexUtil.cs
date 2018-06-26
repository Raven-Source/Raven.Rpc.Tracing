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
    internal static class HexUtil
    {
        private static readonly DateTime __unixEpoch;

        static HexUtil()
        {
            __unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public static DateTime UnixEpoch { get { return __unixEpoch; } }

        /// <summary>
        /// Converts a value to a hex character.
        /// </summary>
        /// <param name="value">The value (assumed to be between 0 and 15).</param>
        /// <returns>The hex character.</returns>
        public static char ToHexChar(int value)
        {
            return (char)(value + (value < 10 ? '0' : 'a' - 10));
        }

        /// <summary>
        /// Converts a byte array to a hex string.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <returns>A hex string.</returns>
        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            var length = bytes.Length;
            var c = new char[length * 2];

            for (int i = 0, j = 0; i < length; i++)
            {
                var b = bytes[i];
                c[j++] = ToHexChar(b >> 4);
                c[j++] = ToHexChar(b & 0x0f);
            }

            return new string(c);
        }

        /// <summary>
        /// Converts a DateTime to local time (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>The DateTime in local time.</returns>
        public static DateTime ToLocalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Local);
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Local);
            }
            else
            {
                return dateTime.ToLocalTime();
            }
        }

        /// <summary>
        /// Converts a DateTime to number of milliseconds since Unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>Number of seconds since Unix epoch.</returns>
        public static long ToMillisecondsSinceEpoch(DateTime dateTime)
        {
            var utcDateTime = ToUniversalTime(dateTime);
            return (utcDateTime - HexUtil.UnixEpoch).Ticks / 10000;
        }

        /// <summary>
        /// Converts a DateTime to UTC (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>The DateTime in UTC.</returns>
        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
            else
            {
                return dateTime.ToUniversalTime();
            }
        }

        /// <summary>
        /// Tries to parse a hex string to a byte array.
        /// </summary>
        /// <param name="s">The hex string.</param>
        /// <param name="bytes">A byte array.</param>
        /// <returns>True if the hex string was successfully parsed.</returns>
        public static bool TryParseHexString(string s, out byte[] bytes)
        {
            bytes = null;

            if (s == null)
            {
                return false;
            }

            var buffer = new byte[(s.Length + 1) / 2];

            var i = 0;
            var j = 0;

            if ((s.Length % 2) == 1)
            {
                // if s has an odd length assume an implied leading "0"
                int y;
                if (!TryParseHexChar(s[i++], out y))
                {
                    return false;
                }
                buffer[j++] = (byte)y;
            }

            while (i < s.Length)
            {
                int x, y;
                if (!TryParseHexChar(s[i++], out x))
                {
                    return false;
                }
                if (!TryParseHexChar(s[i++], out y))
                {
                    return false;
                }
                buffer[j++] = (byte)((x << 4) | y);
            }

            bytes = buffer;
            return true;
        }

        // private static methods
        private static bool TryParseHexChar(char c, out int value)
        {
            if (c >= '0' && c <= '9')
            {
                value = c - '0';
                return true;
            }

            if (c >= 'a' && c <= 'f')
            {
                value = 10 + (c - 'a');
                return true;
            }

            if (c >= 'A' && c <= 'F')
            {
                value = 10 + (c - 'A');
                return true;
            }

            value = 0;
            return false;
        }
        
        /// <summary>
        /// Parses a hex string into its equivalent byte array.
        /// </summary>
        /// <param name="s">The hex string to parse.</param>
        /// <returns>The byte equivalent of the hex string.</returns>
        public static byte[] ParseHexString(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            byte[] bytes;
            if (!TryParseHexString(s, out bytes))
            {
                throw new FormatException("String should contain only hexadecimal digits.");
            }

            return bytes;
        }
    }
}
