// <copyright file="SubstringInfoTests.cs" company="Public Domain">
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
    public class SubstringInfoTests
    {
        private const string Text = "I am a string. I love you.";

        [TestMethod]
        public void StartIndexOnly()

        {
            var start = 3;

            var info = new SubstringInfo(start);

            Assert.AreEqual(info.Start, start);
            Assert.AreEqual(info.End, SubstringInfo.EndOfString);
            Assert.AreEqual(info.Length, SubstringInfo.EndOfString);
        }

        [TestMethod]
        public void StartAndEndIndex()
        {
            var start = 3;
            var end = start + 8;

            var info = new SubstringInfo(start, end);
            Assert.AreEqual(info.Start, start);
            Assert.AreEqual(info.End, end);
            Assert.AreEqual(info.Length, end - start);
        }

        [TestMethod]
        public void InvalidStartIndex()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new SubstringInfo(-1);
            });
        }

        [TestMethod]
        public void StartAndInvalidEnd()
        {
            var start = 5;
            var end = start - 1;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new SubstringInfo(start, end);
            });
        }

        [TestMethod]
        public void StartAndLength()
        {
            var start = 6;
            var length = 11;

            var info = SubstringInfo.FromStartAndLength(
                start,
                length);

            Assert.AreEqual(info.Start, start);
            Assert.AreEqual(info.Length, length);
            Assert.AreEqual(info.End, start + length);
        }

        [TestMethod]
        public void StartAndLengthEndOfString()
        {
            var start = 6;

            var info = SubstringInfo.FromStartAndLength(
                start,
                SubstringInfo.EndOfString);

            Assert.AreEqual(info.Start, start);
            Assert.AreEqual(info.Length, SubstringInfo.EndOfString);
            Assert.AreEqual(info.End, SubstringInfo.EndOfString);
        }

        [TestMethod]
        public void StartAndInvalidLength()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new SubstringInfo(-1);
            });
        }

        [TestMethod]
        public void LengthAndEnd()
        {
            var length = 11;
            var end = length + 4;

            var info = SubstringInfo.FromLengthAndEnd(
                length,
                end);

            Assert.AreEqual(info.Start, end - length);
            Assert.AreEqual(info.Length, length);
            Assert.AreEqual(info.End, end);
        }

        [TestMethod]
        public void InvalidLengthAndEnd()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                SubstringInfo.FromLengthAndEnd(
                    -1,
                    SubstringInfo.EndOfString);
            });
        }

        [TestMethod]
        public void LengthAndInvalidEnd()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                SubstringInfo.FromLengthAndEnd(
                    SubstringInfo.EndOfString,
                    -1);
            });
        }

        [TestMethod]
        public void EmptySubstring()
        {
            Assert.AreEqual(
                SubstringInfo.Empty.GetSubstring(Text),
                String.Empty);
        }

        [TestMethod]
        public void NullSubstring()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                SubstringInfo.Empty.GetSubstring(null);
            });
        }

        [TestMethod]
        public void SubstringFromStart()
        {
            var start = 2;
            var info = new SubstringInfo(start);
            var substring = Text.Substring(start);
            TestSubstring(
                info,
                substring);
        }

        [TestMethod]
        public void SubstringFromStartAndEnd()
        {
            var start = 2;
            var length = 5;
            var end = start + length;
            var info = new SubstringInfo(start, end);
            var substring = Text.Substring(start, length);
            TestSubstring(
                info,
                substring);
        }

        [TestMethod]
        public void SubstringFromStartAndLength()
        {
            var start = 2;
            var length = 5;
            var end = start + length;
            var info = SubstringInfo.FromStartAndLength(
                start,
                length);

            var substring = Text.Substring(start, length);
            TestSubstring(
                info,
                substring);
        }

        [TestMethod]
        public void SubstringFromLengthAndEnd()
        {
            var start = 2;
            var length = 5;
            var end = start + length;
            var info = SubstringInfo.FromLengthAndEnd(length, end);
            var substring = Text.Substring(start, length);
            TestSubstring(
                info,
                substring);
        }

        private static void TestSubstring(
            SubstringInfo info,
            string substring)
        {
            Assert.AreEqual(
                info.GetSubstring(Text),
                substring);
        }
    }
}
