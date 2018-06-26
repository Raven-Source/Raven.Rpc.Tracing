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
    public class SerialVersion
    {
        private List<int> _nums;

        private SerialVersion()
        {
            _nums = new List<int>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SerialVersion GetDefalut()
        {
            SerialVersion serialVersion = new SerialVersion();
            serialVersion.AugmentSerialNum(0);
            return serialVersion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SerialVersion Parse(string value)
        {
            SerialVersion serialVersion = GetDefalut();
            if (!string.IsNullOrWhiteSpace(value))
            {
                foreach (var str in value.Trim().Split('.'))
                {
                    int id = 0;
                    if (int.TryParse(str, out id))
                    {
                        serialVersion.AugmentSerialNum(id);
                    }
                }
            }

            return serialVersion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public SerialVersion IncreaseLeastNum(int count)
        {
            int index = _nums.Count - 1;
            _nums[index] = _nums[index] + count;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public SerialVersion IncreaseLeastNum()
        {
            IncreaseLeastNum(1);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        public SerialVersion AugmentSerialNum(int num)
        {
            _nums.Add(num);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public SerialVersion AugmentSerialNum()
        {
            return AugmentSerialNum(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(".", _nums);
        }

    }
}
