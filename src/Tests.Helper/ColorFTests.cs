// <copyright file="ColorFTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Helper.MathHelper;

namespace Tests.Helper
{
    [TestClass]
    public class ColorFTests
    {
        [TestMethod]
        public void LumaTotal()
        {
            var total =
                ColorF.LumaRedWeight +
                ColorF.LumaGreenWeight +
                ColorF.LumaBlueWeight;

            // Luma weights should sum to unity.
            Assert.AreEqual(total, 1f);
        }

        [TestMethod]
        public void LumaWeight()
        {
            // Humans see green the best and blue the worst.
            Assert.IsTrue(ColorF.LumaGreenWeight > ColorF.LumaRedWeight);
            Assert.IsTrue(ColorF.LumaRedWeight > ColorF.LumaBlueWeight);
        }

        [TestMethod]
        public void EmptyColor()
        {
            // Test empty color.
            var color = ColorF.Empty;
            AssertArgb(color, 0, 0, 0, 0);
        }

        [TestMethod]
        public void AlphaWithEmpty()
        {
            var color = ColorF.FromArgb(1, ColorF.Empty);
            AssertArgb(color, 1, 0, 0, 0);
        }

        [TestMethod]
        public void RedProperties()
        {
            var color = ColorF.FromArgb(1, 0, 0);
            AssertArgb(color, 1, 1, 0, 0);
            Assert.AreEqual(color.Hue, 0);
            Assert.AreEqual(color.HueDegrees, 0);
            Assert.AreEqual(color.Saturation, 1);
        }

        [TestMethod]
        public void YellowProperties()
        {
            var color = ColorF.FromArgb(0.75f, 1, 1, 0);
            AssertArgb(color, 0.75f, 1, 1, 0);
            Assert.AreEqual(color.Hue, 1 / 6f);
            Assert.AreEqual(color.HueDegrees, 60f);
            Assert.AreEqual(color.Saturation, 1);
        }

        [TestMethod]
        public void GreenProperties()
        {
            var color = ColorF.FromArgb(0.25f, 0, 1, 0);
            AssertArgb(color, 0.25f, 0, 1, 0);
            Assert.AreEqual(color.Hue, 2 / 6f);
            Assert.AreEqual(color.HueDegrees, 120f);
            Assert.AreEqual(color.Saturation, 1);
        }

        [TestMethod]
        public void CyanProperties()
        {
            var color = ColorF.FromArgb(1, 0, 0.5f, 0.5f);
            AssertArgb(color, 1, 0, 0.5f, 0.5f);
            Assert.AreEqual(color.Hue, 3 / 6f);
            Assert.AreEqual(color.HueDegrees, 180f);
            Assert.AreEqual(color.Saturation, 1);
        }

        [TestMethod]
        public void BlueProperties()
        {
            var color = ColorF.FromArgb(0.5f, 0, 0, 1);
            AssertArgb(color, 0.5f, 0, 0, 1);
            Assert.AreEqual(color.Hue, 4 / 6f);
            Assert.AreEqual(color.HueDegrees, 240f);
            Assert.AreEqual(color.Saturation, 1);
        }

        [TestMethod]
        public void MagentaProperties()
        {
            var color = ColorF.FromArgb(0.5f, 0.75f, 0.25f, 0.75f);
            AssertArgb(color, 0.5f, 0.75f, 0.25f, 0.75f);
            Assert.AreEqual(color.Hue, 5 / 6f);
            Assert.AreEqual(color.HueDegrees, 300f);
            Assert.AreEqual(color.Saturation, 0.5f);
        }

        [TestMethod]
        public void AlphaNaN()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(Single.NaN, ColorF.Empty);
            });
        }

        [TestMethod]
        public void RedNaN()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(Single.NaN, 0, 0);
            });
        }

        [TestMethod]
        public void GreenNaN()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(0, 0, Single.NaN, 0);
            });
        }

        [TestMethod]
        public void BlueNaN()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(1, 1, 1, Single.NaN);
            });
        }

        [TestMethod]
        public void CyanNaN()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(Single.NaN, 0, 0);
            });
        }

        [TestMethod]
        public void MagentaNaN()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(1, 0, Single.NaN, 1);
            });
        }

        [TestMethod]
        public void YellowNaN()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(Single.PositiveInfinity, 0, 1, Single.NaN);
            });
        }

        private static void AssertArgb(
            ColorF color,
            float alpha,
            float red,
            float green,
            float blue)
        {
            Assert.AreEqual(color.Alpha, alpha);
            Assert.AreEqual(color.Red, red);
            Assert.AreEqual(color.Green, green);
            Assert.AreEqual(color.Blue, blue);

            Assert.AreEqual(color.Cyan, 1 - red);
            Assert.AreEqual(color.Magenta, 1 - green);
            Assert.AreEqual(color.Yellow, 1 - blue);

            var max = Max(red, green, blue);
            var min = Min(red, green, blue);
            Assert.AreEqual(color.Max, max);
            Assert.AreEqual(color.Min, min);

            Assert.AreEqual(color.Lightness, (max + min) / 2);
            Assert.AreEqual(color.Chroma, max - min);

            var luma =
                (ColorF.LumaRedWeight * red) +
                (ColorF.LumaGreenWeight * green) +
                (ColorF.LumaBlueWeight * blue);

            Assert.AreEqual(color.Luma, luma);
        }
    }
}
