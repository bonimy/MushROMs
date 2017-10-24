using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Snes.Properties;
using Snes;

namespace Tests.Snes
{
    [TestClass]
    public class CompressorTests
    {
        private static Compressor Compressor = Compressor.Default;

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
