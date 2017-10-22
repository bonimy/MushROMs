using System;
<<<<<<< HEAD
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper;
=======
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
>>>>>>> refs/remotes/origin/master

namespace Tests.Helper
{
    [TestClass]
    public class Position2DTests
    {
        [TestMethod]
        public void ConstructorTests()
        {
            Assert.AreEqual(Position2D.Empty.X, 0);
            Assert.AreEqual(Position2D.Empty.Y, 0);

            var x = 20;
            var y = -100;
            var p = new Position2D(x, y);
            Assert.AreEqual(p.X, x);
            Assert.AreEqual(p.Y, y);

            x = Int32.MinValue;
            y = Int32.MaxValue;
            p = new Position2D(x, y);
            Assert.AreEqual(x, p.X);
            Assert.AreEqual(y, p.Y);
        }

        [TestMethod]
        public void OperatorTests()
        {
            var p = new Position2D(0, 0);
            Assert.AreEqual(p, Position2D.Empty);

            p = Position2D.Empty + new Position2D(3, 5);
            Assert.AreEqual(p.X, 3);
            Assert.AreEqual(p.Y, 5);

            p *= new Range2D(-2, 3);
            Assert.AreEqual(p, new Position2D(-6, 15));

            p /= 3;
            Assert.AreEqual(p, new Position2D(-2, 5));

            p /= -1;
            Assert.AreEqual(p, new Position2D(2, -5));

            p = new Position2D(7, -11).Subtract(p);
            Assert.AreEqual(p, new Position2D(5, -6));

            Assert.AreNotEqual(new Position2D(0, 1), new Position2D(1, 0));

            Assert.IsTrue(p == new Position2D(5, -6));
            Assert.IsTrue(new Position2D(0, 1) != new Position2D(1, 0));
            Assert.IsFalse(new Position2D(0, 1) == new Position2D(1, 0));

            Assert.IsTrue(p.Equals(new Position2D(5, -6)));

            Assert.AreEqual(p - p, Position2D.Empty);

            var q = new Position2D(-7, 4);

            Assert.AreEqual(Position2D.TopLeft(p, q), new Position2D(-7, -6));
            Assert.AreEqual(Position2D.TopRight(p, q), new Position2D(5, -6));
            Assert.AreEqual(Position2D.BottomLeft(p, q), new Position2D(-7, 4));
            Assert.AreEqual(Position2D.BottomRight(p, q), new Position2D(5, 4));
        }
    }
}
