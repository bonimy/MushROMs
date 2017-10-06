using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper;

namespace Tests.Helper
{
    [TestClass]
    public class SharedArrayTests
    {
        [TestMethod]
        public void ConstructorTests()
        {
            var list = new int[0x1000];
            for (int i = 0; i < list.Length; i++)
                list[i] = i;

            var shared = new SharedArray<int>(list, 0x800);

            shared[0] = 0;
            shared[-0x800] = 0x800;
            shared[0x7FF] = -0x1000;

            Assert.AreEqual(list[0], shared[-0x800]);
            Assert.AreEqual(list[0x800], shared[0]);
            Assert.AreEqual(list[0xFFF], shared[0x7FF]);

            list = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
                new SharedArray<int>(list, 0));

            shared = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
                new SharedArray<int>(shared, 0));
        }

        [TestMethod]
        public void UsageTests()
        {
            var list1 = new string[0x1000];
            for (int i = 0; i < 0x1000; i++)
                list1[i] = "0x" + i.ToString("X4");

            var list2 = new string[list1.Length];
            for (int i = 0; i < list2.Length; i++)
                list2[i] = list1[list1.Length - 1 - i];

            var shared = new SharedArray<string>(list1, 0x200);
            int dif = shared - list1;

            Assert.AreEqual(dif, 0x200);

            shared += 0x400;
            Assert.AreEqual(shared.Offset, 0x600);

            Assert.ThrowsException<ArgumentException>(() => shared - list2);

            var shared2 = new SharedArray<string>(list2, 0);

            Assert.ThrowsException<ArgumentException>(() => shared - shared2);
        }
    }
}
