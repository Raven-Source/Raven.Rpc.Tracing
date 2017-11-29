using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Raven.Rpc.Tracing.Test
{
    [TestClass]
    public class UtilTest
    {
        [TestMethod]
        public void VersionIncr()
        {
            string res = null;
            res = Util.VersionIncr("");

            StringAssert.Equals(res, "1");

            res = Util.VersionIncr("5");
            StringAssert.Equals(res, "6");

            res = Util.VersionIncr("1.2.5");
            StringAssert.Equals(res, "1.2.6");

            res = Util.VersionIncr("1.2.0");
            StringAssert.Equals(res, "1.2.1");

            res = Util.VersionIncr("1.2.");
            StringAssert.Equals(res, "1.2.1");
        }

        [TestMethod]
        public void GetUniqueCode16()
        {
            //string res = null;
            //res = Util.GetUniqueCode22();

            //res = new Guid(Util.GetGuidArray()).ToString();
            //res = new Guid(Util.GetGuidArray()).ToString();
            //res = new Guid(Util.GetGuidArray()).ToString();
            //res = new Guid(Util.GetGuidArray()).ToString();
            //res = new Guid(Util.GetGuidArray()).ToString();

            System.Collections.Concurrent.ConcurrentDictionary<string, string> dict = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();
            System.Collections.Concurrent.ConcurrentQueue<string> queue = new System.Collections.Concurrent.ConcurrentQueue<string>();
            //Dictionary<string, string> dict = new Dictionary<string, string>(10000000);
            //long ms = 0;
            int none = 0;
            Stopwatch w = new Stopwatch();
            Task[] tasks = new Task[1];
            w.Start();
            for (int c = 0; c < tasks.Length; c++)
            {
                var t = Task.Run(() =>
                {
                    //string id = Guid.NewGuid().ToString();
                    string id = Generate.GenerateId();
                    queue.Enqueue(id);
                    if (dict.TryAdd(id, id))
                    {

                    }
                    else
                    {
                        none++;
                    }
                });
                tasks[c] = t;
            }
            Task.WaitAll(tasks);
            w.Stop();

            if (none > 0 || dict.Count != tasks.Length)
            {
                throw new Exception("ID重复");
            }


            ;

        }

        [TestMethod]
        public void GuidToInt()
        {
            HashSet<long> hs = new HashSet<long>();
            for (var i = 0; i < 1000000; i++)
            {
                Guid guid = Guid.NewGuid();
                long id = BitConverter.ToInt64(guid.ToByteArray(), 0);
                if (hs.Contains(id))
                {
                    Debug.WriteLine($"在{i}次循环处重复，重复值guid {guid}:{id}");
                    throw new Exception();
                }
                hs.Add(id);
            }
        }





    }
}
