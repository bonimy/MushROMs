using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class Int24Tests
    {
        [TestMethod]
        public void TestUsage()
        {
            Assert.AreEqual(new Int24(), (Int24)0);

            var x = (Int24)2;
            Assert.AreEqual(x, (Int24)2);

            Assert.AreEqual(x - 1, (Int24)1);

            x++;
            Assert.AreEqual(x, (Int24)3);

            Assert.AreEqual(++x, (Int24)4);
            Assert.AreEqual(x--, (Int24)4);

            Assert.AreEqual(x *= 2, (Int24)6);

            var y = x / 2;
            Assert.IsTrue(x / 2 == y);
            Assert.IsFalse(x != y * 2);
            Assert.IsTrue(x != y);

            Assert.AreNotEqual(x, y);

            var z = Int24.MaxValue;
            Assert.AreEqual(z, Int24.MaxValue);

            z++;
            Assert.IsTrue(z == Int24.MinValue);
            Assert.AreEqual(z, Int24.MinValue);

            z = 0;
            z--;
            int a = z;
            Assert.IsTrue(a == -1);
            z--;
            Assert.IsTrue((a = z) == -2);
        }
    }
}
