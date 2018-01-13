using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class Range2DTests
    {
        [TestMethod]
        public void Range2DConstructors()
        {
            Assert.AreEqual(Range2D.Empty.Width, 0);
            Assert.AreEqual(Range2D.Empty.Height, 0);

            var h = 20;
            var v = -100;
            var p = new Range2D(h, v);
            Assert.AreEqual(p.Width, h);
            Assert.AreEqual(p.Height, v);

            h = Int32.MinValue;
            v = Int32.MaxValue;
            p = new Range2D(h, v);
            Assert.AreEqual(h, p.Width);
            Assert.AreEqual(v, p.Height);
        }

        [TestMethod]
        public void Range2DOperators()
        {
            var p = new Range2D(0, 0);
            Assert.AreEqual(p, Range2D.Empty);
            Assert.AreEqual(0, p.Area);

            Assert.IsTrue(5 == new Range2D(5, 5));

            p = Range2D.Empty + new Range2D(3, 5);
            Assert.AreEqual(p.Width, 3);
            Assert.AreEqual(p.Height, 5);
            Assert.AreEqual(p.Area, 15);

            p *= new Range2D(-2, 3);
            Assert.AreEqual(p, new Range2D(-6, 15));
            Assert.AreEqual(p.Area, -90);
            Assert.IsTrue(p == (Range2D)new Position2D(-6, 15));

            p /= 3;
            Assert.AreEqual(p, new Range2D(-2, 5));

            p /= -1;
            Assert.AreEqual(p, new Range2D(2, -5));
            Assert.IsTrue(p.Area == -10);

            p = new Range2D(7, -11).Subtract(p);
            Assert.AreEqual(p, new Range2D(5, -6));

            Assert.AreNotEqual(new Range2D(0, 1), new Range2D(1, 0));

            Assert.IsTrue(p == new Range2D(5, -6));
            Assert.IsTrue(new Range2D(0, 1) != new Range2D(1, 0));
            Assert.IsFalse(new Range2D(0, 1) == new Range2D(1, 0));

            Assert.IsTrue(p.Equals(new Range2D(5, -6)));

            Assert.AreEqual(p - p, Range2D.Empty);

            Assert.IsTrue(p.Area == (-p).Area);

            var q = new Range2D(-7, 4);

            Assert.AreEqual(Range2D.TopLeft(p, q), new Range2D(-7, -6));
            Assert.AreEqual(Range2D.TopRight(p, q), new Range2D(5, -6));
            Assert.AreEqual(Range2D.BottomLeft(p, q), new Range2D(-7, 4));
            Assert.AreEqual(Range2D.BottomRight(p, q), new Range2D(5, 4));
        }
    }
}
