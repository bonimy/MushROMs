// <copyright file="SuffixTreeTests.cs" company="Public Domain">
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
    public class SuffixTreeTests
    {
        [TestMethod]
        public void SuffixTreeUsage()
        {
            // suffix tree has just a default constructor.
            var tree = new SuffixTree();

            var data = new byte[]
            {
                // 00: A string of numbers from 0-9
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,

                // 01: All 0
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

                // 02: 0-5, then random
                0, 1, 2, 3, 4, 5, 1, 7, 1, 4,

                // 03: 2-9 then random
                2, 3, 4, 5, 6, 7, 8, 9, 5, 5,

                // 04: 0-7 (one off of row three)
                0, 1, 2, 3, 4, 5, 6, 7, 1, 4,

                // 05: 0-7 again
                0, 1, 2, 3, 4, 5, 6, 7, 5, 5,

                // 06: 0-7, then 5 and 6
                0, 1, 2, 3, 4, 5, 6, 7, 5, 7,

                // 07: 0-8
                0, 1, 2, 3, 4, 5, 6, 7, 8, 1,

                // 08: some reverse
                7, 6, 5, 4, 3, 2, 8, 8, 8, 8,

                // 09: 6-3 reverse
                6, 5, 4, 3, 7, 7, 7, 7, 7, 7,

                // 10: 5-1 reverse
                5, 4, 3, 2, 1, 9, 9, 9, 9, 9,

                // 11: 9-3 reverse
                9, 8, 7, 6, 5, 4, 3, 7, 7, 7,

                // 12: new number
                10, 0, 0, 0, 0, 0, 0, 0, 0, 0,

                // 13: repeating
                0, 0, 1, 1, 2, 2, 4, 2, 5, 7,

                // 14: longer repeating
                0, 0, 1, 1, 2, 2, 3, 3, 4, 2,

                // 15: short repeat again
                0, 0, 1, 1, 2, 2, 7, 8, 1, 2,

                // 16: long repeat again
                0, 0, 1, 1, 2, 2, 3, 3, 4, 4,

                // 17: full reverse
                9, 8, 7, 6, 5, 4, 3, 2, 1, 0,

                // 18: repeat full reverse
                9, 8, 7, 6, 5, 4, 3, 2, 1, 0,

                // 19: repeat again
                9, 8, 7, 6, 5, 4, 3, 2, 1, 0,
            };

            // Create tree after constructed.
            tree.CreateTree(data);

            // Tree size should be 1 extra due to termination value.
            Assert.AreEqual(tree.Size, data.Length + 1);

            // All data in tree should match its array.
            for (var i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], tree[i]);
            }

            // Tree should end with termination value.
            Assert.AreEqual(tree[data.Length], SuffixTree.TerminationValue);

            // Tree should accept range within array.
            var start = 20;
            var size = 50;

            // Constructed tree should be reassignable (this practice is preferred).
            tree.CreateTree(data, start, size);
            Assert.AreEqual(tree.Size, size + 1);
            for (var i = 0; i < size; i++)
            {
                Assert.AreEqual(data[start + i], tree[i]);
            }

            Assert.AreEqual(tree[size], SuffixTree.TerminationValue);

            // Use full data for testing.
            tree.CreateTree(data);

            // Index of 0 should always return empty substring.
            var index = tree.GetLongestInternalSubstring(0);
            Assert.AreEqual(index, SubstringInfo.Empty);

            // Just do some simple checks with expected longest substrings.
            index = tree.GetLongestInternalSubstring(20);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(0, 6));

            index = tree.GetLongestInternalSubstring(30);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(2, 8));

            index = tree.GetLongestInternalSubstring(40);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(0, 8));

            // Find longest match with lowest index
            index = tree.GetLongestInternalSubstring(50);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(0, 8));

            index = tree.GetLongestInternalSubstring(60);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(50, 9));

            index = tree.GetLongestInternalSubstring(70);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(0, 9));

            // Look for reversed matches now.
            index = tree.GetLongestInternalSubstring(90);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(81, 4));

            index = tree.GetLongestInternalSubstring(100);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(82, 4));

            // No part of the array has started with 9, 8 yet.
            index = tree.GetLongestInternalSubstring(110);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(9, 1));

            // If we haven't encountered first number, return empty.
            index = tree.GetLongestInternalSubstring(120);
            Assert.AreEqual(index, SubstringInfo.Empty);

            // Find longest substring even if a shorter match occurred earlier.
            index = tree.GetLongestInternalSubstring(160);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(140, 9));

            // Allow overlap of substring if match continues to end of data.
            index = tree.GetLongestInternalSubstring(180);
            Assert.AreEqual(index, SubstringInfo.FromStartAndLength(170, 20));

            // Throw on out of range.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                tree.GetLongestInternalSubstring(-1);
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                tree.GetLongestInternalSubstring(data.Length);
            });
        }
    }
}
