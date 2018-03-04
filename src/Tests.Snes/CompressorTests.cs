// <copyright file="CompressorTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snes;
using Tests.Snes.Properties;

namespace Tests.Snes
{
    [TestClass]
    public class CompressorTests
    {
        private static readonly Compressor Compressor = Compressor.Default;

        [TestMethod]
        public void SmallCompressorTests()
        {
            /*
            To maximize compressed data space and decompression time, we need to examine several edge cases. For example, the minimum number of bytes to reduce space for a repeating byte sequence is three bytes.

            data:   00 00 00
            best:   22 00 - Using repeating byte compression
            alt:    02 00 00 00 - Direct copy costs space
            */
            var data = new byte[] { 0x00, 0x00, 0x00 };
            var best = new byte[] { 0x22, 0x00, 0xFF };
            AssertData();

            /*
             However, if there is an uncompressable byte before and after the data, we have two options that take the same amount of space. The single direct copy is preferred since it reduces the total number of commands for the decompressor.

             data:  01 00 00 00 10
             best:  04 01 00 00 00 10 - One continuous direct copy
             alt:   00 01|22 00|00 10 - Direct copy, repeating, and direct copy
            */
            data = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x10 };
            best = new byte[] { 0x04, 0x01, 0x00, 0x00, 0x00, 0x10, 0xFF };
            AssertData();

            /*
            If just one side is uncompressable, then doing the repeated bytes saves space.

            data:   00 00 00 10
            best:   22 00|00 10
            alt:    03 00 00 00 10

            data:   01 00 00 00
            best:   00 01|22 00
            alt:    03 01 00 00 00

            data:   00 00 00 01 00 00 00 02
            best:   22 00|04 01 00 00 00 02
            alt:    22 00|00 01|22 00|00 01
            */
            data = new byte[] { 0x00, 0x00, 0x00, 0x10 };
            best = new byte[] { 0x22, 0x00, 0x00, 0x10, 0xFF };
            AssertData();

            data = new byte[] { 0x01, 0x00, 0x00, 0x00 };
            best = new byte[] { 0x00, 0x01, 0x22, 0x00, 0xFF };
            AssertData();

            /*
            The "one side uncompressable" and "both sides uncompressable" rules apply even when next to each other.

            data:   00 00 00 01 00 00 00 02
            best:   12 00|04 01 00 00 00 02
            alt:    12 00|00 01|12 00|00 01
            */
            data = new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02 };
            best = new byte[] { 0x22, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00, 0x02, 0xFF };
            AssertData();

            // 4096 bytes of data that has no patterns that can be compressed.
            var uncompressable = CreateUncompressedData();

            best = Compressor.Compress(uncompressable);
            AssertArrayEquality(uncompressable, Compressor.Decompress(best));

            data = new byte[0x1000];
            best = Compressor.Compress(data);
            AssertArrayEquality(data, Compressor.Decompress(best));

            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }

            best = Compressor.Compress(data);
            AssertArrayEquality(data, Compressor.Decompress(best));

            data = new byte[0x1000];
            for (var i = 1; i < data.Length; i += 2)
            {
                data[i] = 0xFF;
            }

            best = Compressor.Compress(data);
            AssertArrayEquality(data, Compressor.Decompress(best));

            for (var i = 0; i < 0x800; i++)
            {
                data[i] = data[i + 0x800] = uncompressable[i];
            }

            best = Compressor.Compress(data);
            AssertArrayEquality(data, Compressor.Decompress(best));

            /*
            We run into more edge case pain when one or both sides of uncompressable data is over 32 bytes, when we get into long commands.

            When both sides are uncompressable, each side is less than 32 bytes, but the total section size is greater than 32 bytes, inserting the repeated byte copy saves space.

            data:   [n <= 0x20 bytes] 00 00 00 [m <= 0x20 bytes] : n + m + 3 > 0x20
            best:   0n [n bytes]|22 00|0m [m bytes] = (1 + n) + 2 + (1 + m) bytes
            alt:    xx xx [n + m + 3 bytes]         = (2 + n + m + 3) bytes
            */

            int n = 0x10, m = 0x10;
            data = new byte[n + 3 + m];
            var expected = new byte[(1 + n) + 2 + (1 + m) + 1];

            expected[0] = (byte)(n - 1);

            for (var i = 0; i < n; i++)
            {
                data[i] = uncompressable[i];
                expected[1 + i] = uncompressable[i];
            }

            expected[1 + n] = 0x22;

            expected[(1 + n) + 2] = (byte)(m - 1);

            for (var i = 0; i < m; i++)
            {
                data[i + 3 + n] = uncompressable[n + i];
                expected[(1 + n) + 2 + (1 + i)] = uncompressable[n + i];
            }

            expected[(1 + n) + 2 + (1 + m)] = 0xFF;

            AssertArrayEquality(expected, Compressor.Compress(data));

            /*
            When both sides are uncompressable, one side is less than 32 bytes, but the other size is greater than 32 bytes, the full direct copy uses as many bytes as inserting the repeating byte copy, but the full direct copy uses less commands, so it is preferred.

            data:   [n > 0x20] 00 00 00 [m <= 0x20]
            best:   xx xx [n + m + 3 bytes]             = (2 + n + m + 3) bytes
            alt:    xx xx [n bytes]|12 00|0m [m bytes]  = (2 + n) + 2 + (1 + m) bytes
            */

            n = 0x21;
            m = 0x1;
            data = new byte[n + 3 + m];
            expected = new byte[(2 + n + 3 + m) + 1];

            var total = n + m + 3 - 1;
            expected[0] = (byte)(0xE0 | (total >> 8));
            expected[1] = (byte)total;

            for (var i = 0; i < n; i++)
            {
                data[i] = uncompressable[i];
                expected[2 + i] = uncompressable[i];
            }

            for (var i = 0; i < m; i++)
            {
                data[i + 3 + n] = uncompressable[n + i];
                expected[2 + n + 3 + i] = uncompressable[n + i];
            }

            expected[2 + n + 3 + m] = 0xFF;

            AssertArrayEquality(expected, Compressor.Compress(data));

            /*
            When both sides are uncompressable and greater than 32 bytes, the repeating byte command must be at least four bytes

            data:   [n > 0x20] 00 00 00 [m > 0x20]
            best:   xx xx [n bytes]|12 00|xx xx [m bytes] - n + m + 6 bytes
            alt:    xx xx [n + m + 3 bytes] - n + m + 5 bytes
            */

            n = 0x21;
            m = 0x21;
            data = new byte[n + 4 + m];
            expected = new byte[(2 + n + 4 + m) + 1];

            total = n + m + 4 - 1;
            expected[0] = (byte)(0xE0 | (total >> 8));
            expected[1] = (byte)total;

            for (var i = 0; i < n; i++)
            {
                data[i] = uncompressable[i];
                expected[2 + i] = uncompressable[i];
            }

            for (var i = 0; i < m; i++)
            {
                data[i + 4 + n] = uncompressable[n + i];
                expected[2 + n + 4 + i] = uncompressable[n + i];
            }

            expected[2 + n + 4 + m] = 0xFF;

            AssertArrayEquality(expected, Compressor.Compress(data));

            void AssertData()
            {
                var compressed = Compressor.Compress(data);
                AssertArrayEquality(best, compressed);
            }
        }

        private static byte[] CreateUncompressedData()
        {
            var result = new byte[0x1000];

            for (var x = 0; x < 0x10; x++)
            {
                WriteValues(x * 0x100, x + 1, 0);
            }

            return result;

            void WriteValues(int offset, int step, int start)
            {
                for (var j = 0; j < step; j++)
                {
                    for (var i = 0; i < 0x100; i += step)
                    {
                        result[(i / step) + ((j * 0x100) / step) + offset] = (byte)((i + start + j) ^ 0xFF);
                    }
                }
            }
        }

        [TestMethod]
        public void TestAgainstKnownBestCompressor()
        {
            // Gets all the uncompressed data.
            var decompressedDataList = GetDataList(
                Resources.AllDecompressed,
                Resources.DecompressedIndexes);

            // Gets the same data, but compressed by the known best LZ2 algorithm.
            var compressedDataList = GetDataList(
                Resources.AllCompressedBest,
                Resources.CompressedIndexesBest);

            // Make sure we got the same number of tests.
            Assert.AreEqual(decompressedDataList.Count, compressedDataList.Count);

            for (var i = 0; i < decompressedDataList.Count; i++)
            {
                // Check for each file that we're matching the known best compressor.
                AssertAgainstKnownBestCompressor(
                    decompressedDataList[i],
                    compressedDataList[i]);
            }
        }

        private void AssertAgainstKnownBestCompressor(byte[] inUncompressed, byte[] inCompressed)
        {
            // Get our compressed data.
            var outCompressed = Compressor.Compress(inUncompressed);

            // Get the list of compress commands for each compress data.
            var inCommands = CompressInfo.GetCompressList(inCompressed, 0);
            var outCommands = CompressInfo.GetCompressList(outCompressed, 0);

            const string Winning = "It looks like we're beating the known best compressor by number of commands. Comment out this assert and see that all of our other tests are passing. If they are, then it seems we've improved the compressor. We'll have to update the test data to reflect this change.";

            Assert.IsFalse(outCommands.Count < inCommands.Count, Winning);

            Assert.IsFalse(outCompressed.Length < inCompressed.Length, Winning);

            assertArrayMatch(inCompressed, outCompressed);

            // Make sure we correctly decompress our data.
            var outDecompressed = Compressor.Decompress(inCompressed);
            assertArrayMatch(inUncompressed, outDecompressed);

            // Make sure we correctly decompress our own data.
            outDecompressed = Compressor.Decompress(outCompressed);
            assertArrayMatch(inUncompressed, outDecompressed);

            void assertArrayMatch(byte[] left, byte[] right)
            {
                // Check that the data we decompressed matches the actual decompressed data.
                Assert.AreEqual(left.Length, right.Length);
                for (var i = 0; i < left.Length; i++)
                {
                    Assert.AreEqual(left[i], right[i]);
                }
            }
        }

        [TestMethod]
        public void TestCompressorAgainstLunarCompress()
        {
            // Gets all the uncompressed data.
            var decompressedDataList = GetDataList(
                Resources.AllDecompressed,
                Resources.DecompressedIndexes);

            // Gets the same data, but compressed by Lunar Compress's LZ2 routine.
            var compressedDataList = GetDataList(
                Resources.AllCompressedLunar,
                Resources.CompressedIndexesLunar);

            // Make sure we got the same number of tests.
            Assert.AreEqual(decompressedDataList.Count, compressedDataList.Count);

            for (var i = 0; i < decompressedDataList.Count; i++)
            {
                // Check for each file that we're beating Lunar Compress.
                AssertAgainstLunarCompress(
                    decompressedDataList[i],
                    compressedDataList[i]);
            }
        }

        private void AssertArrayEquality(byte[] left, byte[] right)
        {
            if (left == right)
            {
                return;
            }

            Assert.IsTrue(left != null && right != null);
            Assert.AreEqual(left.Length, right.Length);
            for (var i = 0; i < left.Length; i++)
            {
                Assert.AreEqual(left[i], right[i]);
            }
        }

        private void AssertAgainstLunarCompress(byte[] inUncompressed, byte[] inCompressed)
        {
            // Get our compressed data.
            var outCompressed = Compressor.Compress(inUncompressed);

            // Get the list of compress commands for each compress data.
            var inCommands = CompressInfo.GetCompressList(inCompressed, 0);
            var outCommands = CompressInfo.GetCompressList(outCompressed, 0);

            // We want to have a compressor that uses less commands that Lunar Compress
            Assert.IsTrue(outCommands.Count <= inCommands.Count);

            // We also want the compressed data size to be better.
            Assert.IsTrue(outCompressed.Length <= inCompressed.Length);

            // Make sure we correctly decompress Lunar Compress data.
            var outDecompressed = Compressor.Decompress(inCompressed);
            assertDecompressed();

            // Make sure we correctly decompress our own data.
            outDecompressed = Compressor.Decompress(outCompressed);
            assertDecompressed();

            void assertDecompressed()
            {
                // Check that the data we decompressed matches the actual decompressed data.
                Assert.AreEqual(inUncompressed.Length, outDecompressed.Length);
                for (var i = 0; i < inUncompressed.Length; i++)
                {
                    Assert.AreEqual(inUncompressed[i], outDecompressed[i]);
                }
            }
        }

        internal static List<byte[]> GetDataList(byte[] src, byte[] indexes)
        {
            var result = new List<byte[]>();

            var lastIndex = 0;
            for (var i = 0; i < indexes.Length; i += sizeof(int))
            {
                var index = BitConverter.ToInt32(indexes, i);

                var compressed = new byte[index - lastIndex];
                Array.Copy(src, lastIndex, compressed, 0, compressed.Length);
                result.Add(compressed);

                lastIndex = index;
            }

            return result;
        }
    }
}
