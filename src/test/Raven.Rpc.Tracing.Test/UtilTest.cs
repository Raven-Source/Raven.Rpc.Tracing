using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var res = Util.GetUniqueCode22();
        }
    }
}
