// <copyright file="SubstringPointerTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
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
            var endOfString = SubstringInfo.EndOfString;
            var negative = endOfString == -1 ? -2 : -1;

            Assert.AreEqual(SubstringInfo.Empty.Start, 0);
            Assert.AreEqual(SubstringInfo.Empty.End, 0);
            Assert.AreEqual(SubstringInfo.Empty.Length, 0);

            var sp = new SubstringInfo(3, 11);
            Assert.AreEqual(sp.Start, 3);
            Assert.AreEqual(sp.End, 11);
            Assert.AreEqual(sp.Length, 8);

            sp = new SubstringInfo(2, endOfString);
            Assert.AreEqual(sp.Start, 2);
            Assert.AreEqual(sp.End, endOfString);
            Assert.AreEqual(sp.Length, endOfString);

            // Negative start length. (Although shared arrays could complicate this).
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new SubstringInfo(negative, 10);
            });

            // End before length.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringInfo(5, 4);
            });

            // Negative end values not EndOfString designator.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringInfo(0, negative);
            });

            sp = SubstringInfo.FromStartAndLength(6, 11);
            Assert.AreEqual(sp.Start, 6);
            Assert.AreEqual(sp.Length, 11);
            Assert.AreEqual(sp.End, 17);

            sp = SubstringInfo.FromStartAndLength(8, endOfString);
            Assert.AreEqual(sp.Start, 8);
            Assert.AreEqual(sp.End, endOfString);
            Assert.AreEqual(sp.Length, endOfString);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringInfo(0, negative);
            });

            sp = SubstringInfo.FromLengthAndEnd(5, 6);
            Assert.AreEqual(sp.Length, 5);
            Assert.AreEqual(sp.End, 6);
            Assert.AreEqual(sp.Start, 1);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new SubstringInfo(negative, 5);
            });

            sp = SubstringInfo.FromLengthAndEnd(endOfString, 8);
            Assert.IsTrue(sp == new SubstringInfo(0, 8));

            sp = SubstringInfo.FromLengthAndEnd(6, endOfString);
            Assert.IsTrue(sp.Start == endOfString);
            Assert.IsTrue(sp.Length == 6);
            Assert.IsTrue(sp.End == endOfString);
        }

        [TestMethod]
        public void SubstringPointerEquality()
        {
            Assert.AreEqual(new SubstringInfo(), SubstringInfo.Empty);

            var sp = new SubstringInfo(0, 11);
            Assert.IsTrue(sp == new SubstringInfo(0, 11));
            Assert.IsTrue(sp != SubstringInfo.Empty);

            Assert.IsTrue(sp.Equals(SubstringInfo.FromLengthAndEnd(SubstringInfo.EndOfString, 11)));

            sp = new SubstringInfo(5, 9);
            Assert.AreEqual(sp, SubstringInfo.FromStartAndLength(5, 4));
        }

        [TestMethod]
        public void SubstringTests()
        {
            var text = "Hello, I am a text string!";

            Assert.AreEqual(SubstringInfo.Empty.GetSubstring(text), String.Empty);

            var sp = new SubstringInfo(7, 18);
            Assert.IsTrue(sp.GetSubstring(text) == text.Substring(7, 11));

            sp = new SubstringInfo(19, SubstringInfo.EndOfString);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(19));

            sp = new SubstringInfo(0, SubstringInfo.EndOfString);
            Assert.IsTrue(sp.GetSubstring(text) == text);

            sp = SubstringInfo.FromStartAndLength(5, 13);
            Assert.IsTrue(sp.GetSubstring(text) == text.Substring(5, 13));

            sp = SubstringInfo.FromStartAndLength(9, SubstringInfo.EndOfString);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(9));

            sp = SubstringInfo.FromLengthAndEnd(8, 11);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(3, 8));

            sp = SubstringInfo.FromLengthAndEnd(SubstringInfo.EndOfString, SubstringInfo.EndOfString);
            Assert.AreEqual(sp.GetSubstring(text), text);

            sp = SubstringInfo.FromLengthAndEnd(SubstringInfo.EndOfString, 9);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(0, 9));

            sp = SubstringInfo.FromLengthAndEnd(9, SubstringInfo.EndOfString);
            Assert.AreEqual(sp.GetSubstring(text), text.Substring(text.Length - 9));

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                sp.GetSubstring(null);
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringInfo(0, text.Length + 1).GetSubstring(text);
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new SubstringInfo(0, Int32.MaxValue).GetSubstring(text);
            });
        }
    }
}
