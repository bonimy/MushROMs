using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper;

namespace Tests.Helper
{
    [TestClass]
    public class Range2DTests
    {
        [TestMethod]
        public void ConstructorTests()
        {
            Assert.AreEqual(Range2D.Empty.Horizontal, 0);
            Assert.AreEqual(Range2D.Empty.Vertical, 0);

            var h = 20;
            var v = -100;
            var p = new Range2D(h, v);
            Assert.AreEqual(p.Horizontal, h);
            Assert.AreEqual(p.Vertical, v);

            h = Int32.MinValue;
            v = Int32.MaxValue;
            p = new Range2D(h, v);
            Assert.AreEqual(h, p.Horizontal);
            Assert.AreEqual(v, p.Vertical);
        }

        [TestMethod]
        public void OperatorTests()
        {
            var p = new Range2D(0, 0);
            Assert.AreEqual(p, Range2D.Empty);
            Assert.AreEqual(0, p.Area);

            Assert.IsTrue(5 == new Range2D(5, 5));

            p = Range2D.Empty + new Range2D(3, 5);
            Assert.AreEqual(p.Horizontal, 3);
            Assert.AreEqual(p.Vertical, 5);
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
