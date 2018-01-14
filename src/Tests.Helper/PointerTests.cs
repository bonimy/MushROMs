// <copyright file="PointerTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class PointerTests
    {
        [TestMethod]
        public void PointerConstructors()
        {
            var list = new int[0x1000];
            for (var i = 0; i < list.Length; i++)
            {
                list[i] = i;
            }

            var pointer = (Pointer<int>)list;
            pointer += 0x800;

            pointer[0] = 0;
            pointer[-0x800] = 0x800;
            pointer[0x7FF] = -0x1000;

            Assert.AreEqual(list[0], pointer[-0x800]);
            Assert.AreEqual(list[0x800], pointer[0]);
            Assert.AreEqual(list[0xFFF], pointer[0x7FF]);

            // We're allowed to have null pointers.
            list = null;
            pointer = list;
            pointer = null;
        }

        [TestMethod]
        public void PointerUsage()
        {
            var list1 = new string[0x1000];
            for (var i = 0; i < 0x1000; i++)
            {
                list1[i] = "0x" + i.ToString("X4");
            }

            var list2 = new string[list1.Length];
            for (var i = 0; i < list2.Length; i++)
            {
                list2[i] = list1[list1.Length - 1 - i];
            }

            var shared = (Pointer<string>)list1;
            shared += 0x200;
            var dif = shared - list1;

            Assert.AreEqual(dif, 0x200);

            shared += 0x400;
            Assert.AreEqual(shared.Offset, 0x600);

            Assert.ThrowsException<ArgumentException>(() => shared - list2);

            var shared2 = (Pointer<string>)list2;

            Assert.ThrowsException<ArgumentException>(() => shared - shared2);
        }
    }
}
