using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class UInt24Tests
    {
        [TestMethod]
        public void TestUsage()
        {
            Assert.AreEqual(new UInt24(), (UInt24)0);

            var x = (UInt24)2;
            Assert.AreEqual(x, (UInt24)2);

            Assert.AreEqual(x - 1, (UInt24)1);

            x++;
            Assert.AreEqual(x, (UInt24)3);

            Assert.AreEqual(++x, (UInt24)4);
            Assert.AreEqual(x--, (UInt24)4);

            Assert.AreEqual(x *= 2, (UInt24)6);

            var y = x / 2;
            Assert.IsTrue(x / 2 == y);
            Assert.IsFalse(x != y * 2);
            Assert.IsTrue(x != y);

            Assert.AreNotEqual(x, y);

            var z = (UInt24)Int32.MaxValue;
            Assert.AreNotEqual(z, UInt32.MaxValue);

            z++;
            Assert.IsTrue(z == UInt32.MinValue);
            Assert.AreEqual(z, UInt24.MinValue);
        }
    }
}
