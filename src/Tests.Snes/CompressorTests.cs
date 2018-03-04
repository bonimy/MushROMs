// <copyright file="CompressorTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snes;
using Tests.Snes.Properties;

namespace Tests.Snes
{
    [TestClass]
    public class CompressorTests
    {
        private static readonly Compressor Compressor = Compressor.Default;

        private static readonly byte[] Uncompressable = CreateUncompressedData();

        [TestMethod]
        public void TrivialRepeatedBytes()
        {
            /*
            To maximize compressed data space and decompression time, we need to examine several edge cases. For example, the minimum number of bytes to reduce space for a repeating byte sequence is three bytes.

            data:   00 00 00
            best:   22 00 - Using repeating byte compression
            alt:    02 00 00 00 - Direct copy costs space
            */
            AssertCompressionQuality(
                new byte[] { 0x00, 0x00, 0x00 },
                new byte[] { 0x22, 0x00, 0xFF });
        }

        [TestMethod]
        public void RepeatedBytesBetweenUncompressable()
        {
            /*
             However, if there is an uncompressable byte before and after the data, we have two options that take the same amount of space. The single direct copy is preferred since it reduces the total number of commands for the decompressor.

             data:  01 00 00 00 10
             best:  04 01 00 00 00 10 - One continuous direct copy
             alt:   00 01|22 00|00 10 - Direct copy, repeating, and direct copy
            */
            AssertCompressionQuality(
                new byte[] { 0x01, 0x00, 0x00, 0x00, 0x10 },
                new byte[] { 0x04, 0x01, 0x00, 0x00, 0x00, 0x10, 0xFF });
        }

        [TestMethod]
        public void RepeatedBytesBeforeUncompressable()
        {
            /*
            If just the right side is uncompressable, then doing the repeated bytes saves space.

            data:   00 00 00 10
            best:   22 00|00 10
            alt:    03 00 00 00 10
            */
            AssertCompressionQuality(
                new byte[] { 0x00, 0x00, 0x00, 0x10 },
                new byte[] { 0x22, 0x00, 0x00, 0x10, 0xFF });
        }

        [TestMethod]
        public void RepeatedBytesAfterUncompressable()
        {
            /*
            If just the left side is uncompressable, then doing the repeated bytes saves space.

            data:   01 00 00 00
            best:   00 01|22 00
            alt:    03 01 00 00 00
            */
            AssertCompressionQuality(
                new byte[] { 0x01, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x01, 0x22, 0x00, 0xFF });
        }

        [TestMethod]
        public void RepeatedBytesAfterUncompressableRepeated()
        {
            /*
            The "one side uncompressable" and "both sides uncompressable" rules apply even when next to each other.

            data:   00 00 00 01 00 00 00 02
            best:   22 00|04 01 00 00 00 02
            alt:    22 00|00 01|22 00|00 01
            */
            AssertCompressionQuality(
                new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02 },
                new byte[] { 0x22, 0x00, 0x04, 0x01, 0x00, 0x00, 0x00, 0x02, 0xFF });
        }

        [TestMethod]
        public void RepeatedBytesBeforeUncompressableRepeated()
        {
            /*
            The "one side uncompressable" and "both sides uncompressable" rules apply even when next to each other.

            data:   01 00 00 00 02 00 00 00
            best:   04 01 00 00 00 02|22 00
            alt:    00 01|22 00|00 02|22 00
            */
            AssertCompressionQuality(
                new byte[] { 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00 },
                new byte[] { 0x04, 0x01, 0x00, 0x00, 0x00, 0x02, 0x22, 0x00, 0xFF });
        }

        [TestMethod]
        public void RepeatedBetweenUncomppressableLongTotal()
        {
            /*
            We run into more edge case pain when one or both sides of uncompressable data is over 32 bytes, when we get into long commands.

            When both sides are uncompressable, each side is less than 32 bytes, but the total section size is greater than 32 bytes, inserting the repeated byte copy saves space.

            data:   [n <= 0x20 bytes] 00 00 00 [m <= 0x20 bytes] : n + m + 3 > 0x20
            best:   0n [n bytes]|22 00|0m [m bytes] = (1 + n) + 2 +(1 + m) bytes
            alt:    xx xx [n + m + 3 bytes]         =  2 +(n + m + 3) bytes
            */

            // Specify the sizes of the uncompressed data's left and right uncompressable sequences.
            var leftUncompressedSize = 0x10;
            var repeatedByteSize = 3;
            var rightUncompressedSize = 0x10;

            var uncompressedData = CreateRepeatedBetweenUncompressable(
                leftUncompressedSize,
                repeatedByteSize,
                rightUncompressedSize);

            var leftCompressedSize = leftUncompressedSize + 1;
            var rightCompressedSize = rightUncompressedSize + 1;

            var compressedSize = leftCompressedSize
                + 2
                + rightCompressedSize
                + 1;

            var expectedCompressedData = new byte[compressedSize];
            using (var stream = new MemoryStream(expectedCompressedData))
            {
                stream.WriteByte(
                    (byte)(leftUncompressedSize - 1));

                stream.Write(
                    Uncompressable,
                    0,
                    leftUncompressedSize);

                stream.WriteByte(
                    (byte)(0x20 | (repeatedByteSize - 1)));

                stream.Seek(1, SeekOrigin.Current);

                stream.WriteByte(
                    (byte)(rightUncompressedSize - 1));

                stream.Write(
                    Uncompressable,
                    leftUncompressedSize,
                    rightUncompressedSize);

                stream.WriteByte(0xFF);
            }

            AssertCompressionQuality(
                uncompressedData,
                expectedCompressedData);
        }

        [TestMethod]
        public void RepeatedBetweenLeftLongUncompressable()
        {
            /*
            When both sides are uncompressable, right side is less than 32 bytes, but left side is greater than 32 bytes, the full direct copy uses as many bytes as inserting the repeating byte copy, but the full direct copy uses less commands, so it is preferred.

            data:   [n > 0x20] 00 00 00 [m <= 0x20]
            best:   xx xx [n + m + 3 bytes]             = (2 + n + m + 3) bytes
            alt:    xx xx [n bytes]|12 00|0m [m bytes]  = (2 + n)+ 2 +(1 + m) bytes
            */
            AssertRepeatedBetweenLongUncompressable(0x21, 3, 0x04);
        }

        [TestMethod]
        public void RepeatedBetweenRightLongUncompressable()
        {
            /*
            When both sides are uncompressable, left side is less than 32 bytes, but right side is greater than 32 bytes, the full direct copy uses as many bytes as inserting the repeating byte copy, but the full direct copy uses less commands, so it is preferred.

            data:   [n <= 0x20] 00 00 00 [m > 0x20]
            best:   xx xx [n + m + 3 bytes]             = (2 + n + m + 3) bytes
            alt:    xx xx [n bytes]|12 00|0m [m bytes]  = (2 + n)+ 2 +(1 + m) bytes
            */
            AssertRepeatedBetweenLongUncompressable(0x04, 3, 0x21);
        }

        [TestMethod]
        public void RepeatedBetweenBothLongUncompressable()
        {
            /*
            When both sides are uncompressable and greater than 32 bytes, the repeating byte command must be at least four bytes

            data:   [n > 0x20] 00 00 00 [m > 0x20]
            best:   xx xx [n bytes]|12 00|xx xx [m bytes] - n + m + 6 bytes
            alt:    xx xx [n + m + 3 bytes] - n + m + 5 bytes
            */
            AssertRepeatedBetweenLongUncompressable(0x24, 4, 0x21);
        }

        private static byte[] CreateRepeatedBetweenUncompressable(
            int leftUncompressedSize,
            int repeatedByteSize,
            int rightUncompressedSize)
        {
            var totalSize = leftUncompressedSize
                + repeatedByteSize
                + rightUncompressedSize;

            var result = new byte[totalSize];
            using (var stream = new MemoryStream(result))
            {
                stream.Write(
                    Uncompressable,
                    0,
                    leftUncompressedSize);

                stream.Seek(
                    repeatedByteSize,
                    SeekOrigin.Current);

                stream.Write(
                    Uncompressable,
                    leftUncompressedSize,
                    rightUncompressedSize);
            }

            return result;
        }

        private static void AssertRepeatedBetweenLongUncompressable(
            int leftUncompressedSize,
            int repeatedByteSize,
            int rightUncompressedSize)
        {
            var uncompressedData = CreateRepeatedBetweenUncompressable(
                leftUncompressedSize,
                repeatedByteSize,
                rightUncompressedSize);

            var compressedSize = 2
                + leftUncompressedSize
                + repeatedByteSize
                + rightUncompressedSize
                + 1;

            var expectedCompressedData = new byte[compressedSize];
            using (var stream = new MemoryStream(expectedCompressedData))
            {
                var writeSize = uncompressedData.Length - 1;
                stream.WriteByte(
                    (byte)(0xE0 | (writeSize >> 8)));

                stream.WriteByte(
                    (byte)writeSize);

                stream.Write(
                    Uncompressable,
                    0,
                    leftUncompressedSize);

                stream.Seek(
                    repeatedByteSize,
                    SeekOrigin.Current);

                stream.Write(
                    Uncompressable,
                    leftUncompressedSize,
                    rightUncompressedSize);

                stream.WriteByte(0xFF);
            }

            AssertCompressionQuality(
                uncompressedData,
                expectedCompressedData);
        }

        private static void AssertCompressionQuality(
            byte[] uncompressedData,
            byte[] expectedCompressedData)
        {
            var compressedData = Compressor.Compress(
                uncompressedData);

            CollectionAssert.AreEqual(
                compressedData,
                expectedCompressedData);
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
