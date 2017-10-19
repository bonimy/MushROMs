﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper;
using MushROMs.SMB1;
using Tests.SMB1.Properties;

namespace Tests.SMB1
{
    [TestClass]
    public class AreaDataTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var rom = Resources.SuperMarioBrosPrg0Rom;

            Assert.AreEqual(0x25, AreaData.GetAreaNumber(rom, 0, 0));
            Assert.AreEqual(0x29, AreaData.GetAreaNumber(rom, 0, 1));
            Assert.AreEqual(0x40, AreaData.GetAreaNumber(rom, 0, 2));
            Assert.AreEqual(0x26, AreaData.GetAreaNumber(rom, 0, 3));
            Assert.AreEqual(0x60, AreaData.GetAreaNumber(rom, 0, 4));
            Assert.AreEqual(0x28, AreaData.GetAreaNumber(rom, 1, 0));
            Assert.AreEqual(0x28, AreaData.GetAreaNumber(rom, 0, 5));
            Assert.AreEqual(0x29, AreaData.GetAreaNumber(rom, 1, 1));
            Assert.AreEqual(0x01, AreaData.GetAreaNumber(rom, 1, 2));
            Assert.AreEqual(0x01, AreaData.GetAreaNumber(rom, 0, 7));
            Assert.AreEqual(0x65, AreaData.GetAreaNumber(rom, 7, 3));

            var result = AreaData.GetAllAreas(rom);
        }
    }
}
