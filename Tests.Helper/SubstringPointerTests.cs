using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper;

namespace Tests.Helper
{
    [TestClass]
    public class SubstringPointerTests
    {
        [TestMethod]
        public void ConstructorTests()
        {

            var negative = SubstringPointer.EndOfString == -1 ? -2 : -1;

            Assert.AreEqual(SubstringPointer.Empty.Start, 0);
            Assert.AreEqual(SubstringPointer.Empty.End, 0);
            Assert.AreEqual(SubstringPointer.Empty.Length, 0);

            var sp = new SubstringPointer(3, 11);
            Assert.AreEqual(sp.Start, 3);
            Assert.AreEqual(sp.End, 11);
            Assert.AreEqual(sp.Length, 8);

            sp = new SubstringPointer(2, SubstringPointer.EndOfString);
            Assert.AreEqual(sp.Start, 2);
            Assert.AreEqual(sp.End, SubstringPointer.EndOfString);
            Assert.AreEqual(sp.Length, SubstringPointer.EndOfString);

            // Negative start length. (Although shared arrays could complicate this).
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new SubstringPointer(negative, 10);
            });

            // End before length.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringPointer(5, 4);
            });

            // Negative end values not EndOfString designator.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringPointer(0, negative);
            });

            sp = SubstringPointer.FromStartAndLength(6, 11);
            Assert.AreEqual(sp.Start, 6);
            Assert.AreEqual(sp.Length, 11);
            Assert.AreEqual(sp.End, 17);

            sp = SubstringPointer.FromStartAndLength(8, SubstringPointer.EndOfString);
            Assert.AreEqual(sp.Start, 8);
            Assert.AreEqual(sp.End, SubstringPointer.EndOfString);
            Assert.AreEqual(sp.Length, SubstringPointer.EndOfString);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringPointer(0, negative);
            });

            sp = SubstringPointer.FromLengthAndEnd(5, 6);
            Assert.AreEqual(sp.Length, 5);
            Assert.AreEqual(sp.End, 6);
            Assert.AreEqual(sp.Start, 1);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new SubstringPointer(negative, 5);
            });
        }
    }
}
