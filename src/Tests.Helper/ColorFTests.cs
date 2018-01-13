using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class ColorFTests
    {
        [TestMethod]
        public void ColorFConsts()
        {
            // We should have four channels, three non-alpha (RGB)
            Assert.AreEqual(ColorF.NumberOfChannels, 4);
            Assert.AreEqual(ColorF.NumberOfColorChannels, 3);

            // Luma weights should sum to unity.
            Assert.AreEqual(ColorF.LumaRed + ColorF.LumaGreen + ColorF.LumaBlue, 1f);

            // Humans see green the best and blue the worst.
            Assert.IsTrue(
                ColorF.LumaGreen > ColorF.LumaRed &&
                ColorF.LumaRed > ColorF.LumaBlue);
        }

        [TestMethod]
        public void FromArgb()
        {
            // Test empty color.
            var color = ColorF.Empty;
            AssertArgb(color, 0, 0, 0, 0);

            // Test alpha reassignment
            color = ColorF.FromArgb(1, color);
            AssertArgb(color, 1, 0, 0, 0);

            // red
            color = ColorF.FromArgb(1, 0, 0);
            AssertArgb(color, 1, 1, 0, 0);
            Assert.AreEqual(color.Hue, 0);
            Assert.AreEqual(color.HueDegrees, 0);
            Assert.AreEqual(color.Saturation, 1);

            // yellow
            color = ColorF.FromArgb(0.75f, 1, 1, 0);
            AssertArgb(color, 0.75f, 1, 1, 0);
            Assert.AreEqual(color.Hue, 1 / 6f);
            Assert.AreEqual(color.HueDegrees, 60f);
            Assert.AreEqual(color.Saturation, 1);

            // green
            color = ColorF.FromArgb(0.25f, 0, 1, 0);
            AssertArgb(color, 0.25f, 0, 1, 0);
            Assert.AreEqual(color.Hue, 2 / 6f);
            Assert.AreEqual(color.HueDegrees, 120f);
            Assert.AreEqual(color.Saturation, 1);

            // cyan
            color = ColorF.FromArgb(1, 0, 0.5f, 0.5f);
            AssertArgb(color, 1, 0, 0.5f, 0.5f);
            Assert.AreEqual(color.Hue, 3 / 6f);
            Assert.AreEqual(color.HueDegrees, 180f);
            Assert.AreEqual(color.Saturation, 1);

            // blue
            color = ColorF.FromArgb(0.5f, 0, 0, 1);
            AssertArgb(color, 0.5f, 0, 0, 1);
            Assert.AreEqual(color.Hue, (4 / 6f));
            Assert.AreEqual(color.HueDegrees, 240f);
            Assert.AreEqual(color.Saturation, 1);

            // magenta
            color = ColorF.FromArgb(0.5f, 0.75f, 0.25f, 0.75f);
            AssertArgb(color, 0.5f, 0.75f, 0.25f, 0.75f);
            Assert.AreEqual(color.Hue, (5 / 6f));
            Assert.AreEqual(color.HueDegrees, 300f);
            Assert.AreEqual(color.Saturation, 0.5f);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(Single.NaN, ColorF.Empty);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(Single.NaN, 0, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(0, Single.NaN, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(0, 0, Single.NaN);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(Single.NaN, 0, 0, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(0, Single.NaN, 0, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(0, 0, Single.NaN, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromArgb(0, 0, 0, Single.NaN);
            });
        }

        private void AssertArgb(ColorF color, float alpha, float red, float green, float blue)
        {
            Assert.AreEqual(color.Alpha, alpha);
            Assert.AreEqual(color.Red, red);
            Assert.AreEqual(color.Green, green);
            Assert.AreEqual(color.Blue, blue);

            Assert.AreEqual(color[ColorF.AlphaIndex], alpha);
            Assert.AreEqual(color[ColorF.RedIndex], red);
            Assert.AreEqual(color[ColorF.GreenIndex], green);
            Assert.AreEqual(color[ColorF.BlueIndex], blue);

            Assert.AreEqual(color.Cyan, 1 - red);
            Assert.AreEqual(color.Magenta, 1 - green);
            Assert.AreEqual(color.Yellow, 1 - blue);

            var max = red > green ? red : (green > blue ? green : blue);
            var min = red < green ? red : (green < blue ? green : blue);
            Assert.AreEqual(color.Max, max);
            Assert.AreEqual(color.Min, min);

            Assert.AreEqual(color.Lightness, (max + min) / 2);
            Assert.AreEqual(color.Chroma, max - min);
            Assert.AreEqual(color.Luma,
                ColorF.LumaRed * red + ColorF.LumaGreen * green + ColorF.LumaBlue * blue);
        }

        [TestMethod]
        public void FromAcmy()
        {
            // Test empty color.
            var color = ColorF.Empty;
            assertAcmy(0, 1, 1, 1);

            // Test some colors
            color = ColorF.FromCmy(1, 0, 0);
            assertAcmy(1, 1, 0, 0);
            Assert.AreEqual(color.Hue, 0.5f);
            Assert.AreEqual(color.HueDegrees, 180);
            Assert.AreEqual(color.Saturation, 1);

            // Test FromArgb with defined alpha
            color = ColorF.FromCmy(0.75f, 0, 1, 0);
            assertAcmy(0.75f, 0, 1, 0);
            Assert.AreEqual(color.Hue, (5 / 6f));
            Assert.AreEqual(color.HueDegrees, 300f);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(Single.NaN, 0, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(0, Single.NaN, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(0, 0, Single.NaN);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(Single.NaN, 0, 0, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(0, Single.NaN, 0, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(0, 0, Single.NaN, 0);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ColorF.FromCmy(0, 0, 0, Single.NaN);
            });

            void assertAcmy(float alpha, float cyan, float magenta, float yellow)
            {
                AssertArgb(color, alpha, 1 - cyan, 1 - magenta, 1 - yellow);
            }
        }
    }
}
