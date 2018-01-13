// <copyright file="SubstringPointerTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class SubstringPointerTests
    {
        [TestMethod]
        public void SubstringPointerConstructors()
        {
            var endOfString = SubstringPointer.EndOfString;
            var negative = endOfString == -1 ? -2 : -1;

            Assert.AreEqual(SubstringPointer.Empty.Start, 0);
            Assert.AreEqual(SubstringPointer.Empty.End, 0);
            Assert.AreEqual(SubstringPointer.Empty.Length, 0);

            var sp = new SubstringPointer(3, 11);
            Assert.AreEqual(sp.Start, 3);
            Assert.AreEqual(sp.End, 11);
            Assert.AreEqual(sp.Length, 8);

            sp = new SubstringPointer(2, endOfString);
            Assert.AreEqual(sp.Start, 2);
            Assert.AreEqual(sp.End, endOfString);
            Assert.AreEqual(sp.Length, endOfString);

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

            sp = SubstringPointer.FromStartAndLength(8, endOfString);
            Assert.AreEqual(sp.Start, 8);
            Assert.AreEqual(sp.End, endOfString);
            Assert.AreEqual(sp.Length, endOfString);

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

            sp = SubstringPointer.FromLengthAndEnd(endOfString, 8);
            Assert.IsTrue(sp == new SubstringPointer(0, 8));

            sp = SubstringPointer.FromLengthAndEnd(6, endOfString);
            Assert.IsTrue(sp.Start == endOfString);
            Assert.IsTrue(sp.Length == 6);
            Assert.IsTrue(sp.End == endOfString);
        }

        [TestMethod]
        public void SubstringPointerEquality()
        {
            Assert.AreEqual(new SubstringPointer(), SubstringPointer.Empty);

            var sp = new SubstringPointer(0, 11);
            Assert.IsTrue(sp == new SubstringPointer(0, 11));
            Assert.IsTrue(sp != SubstringPointer.Empty);

            Assert.IsTrue(sp.Equals(SubstringPointer.FromLengthAndEnd(SubstringPointer.EndOfString, 11)));

            sp = new SubstringPointer(5, 9);
            Assert.AreEqual(sp, SubstringPointer.FromStartAndLength(5, 4));
        }

        [TestMethod]
        public void SubstringTests()
        {
            var text = "Hello, I am a text string!";

            Assert.AreEqual(SubstringPointer.Empty.GetSubstring(text), String.Empty);

            var sp = new SubstringPointer(7, 18);
            Assert.IsTrue(sp.GetSubstring(text) == text.Substring(7, 11));

            sp = new SubstringPointer(19, SubstringPointer.EndOfString);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(19));

            sp = new SubstringPointer(0, SubstringPointer.EndOfString);
            Assert.IsTrue(sp.GetSubstring(text) == text);

            sp = SubstringPointer.FromStartAndLength(5, 13);
            Assert.IsTrue(sp.GetSubstring(text) == text.Substring(5, 13));

            sp = SubstringPointer.FromStartAndLength(9, SubstringPointer.EndOfString);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(9));

            sp = SubstringPointer.FromLengthAndEnd(8, 11);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(3, 8));

            sp = SubstringPointer.FromLengthAndEnd(SubstringPointer.EndOfString, SubstringPointer.EndOfString);
            Assert.AreEqual(sp.GetSubstring(text), text);

            sp = SubstringPointer.FromLengthAndEnd(SubstringPointer.EndOfString, 9);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(0, 9));

            sp = SubstringPointer.FromLengthAndEnd(9, SubstringPointer.EndOfString);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(text.Length - 9));

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                sp.GetSubstring(null);
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringPointer(0, text.Length + 1).GetSubstring(text);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringPointer(0, Int32.MaxValue).GetSubstring(text);
            });
        }
    }
}
