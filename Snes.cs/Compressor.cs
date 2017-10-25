// We make the Compressor class non-static so it can be multi-threaded (since we need a nonlocal
// copy of a SuffixTree to maintain optimization).

using System;
using System.Collections.Generic;
using Helper;

namespace Snes
{
    public class Compressor
    {
        private const int BitsPerByte = 8;

        // A static compressor class to use if operating on a single thread.
        public static readonly Compressor Default = new Compressor();

        private static readonly int[] CommandSizes = { -1, 1, 2, 1, 2 };

        private SuffixTree Tree
        {
            get;
            set;
        }

        private List<CompressInfo> Commands
        {
            get;
            set;
        }

        public Compressor()
        {
            Tree = new SuffixTree();
            Commands = new List<CompressInfo>(0x100);
        }

        public static unsafe List<CompressInfo> GetCompressList(byte* src, int slen)
        {
            var list = new List<CompressInfo>();

            int sindex = 0, dindex = 0, clen;

            while (sindex < slen)
            {
                if (src[sindex] == 0xFF)
                {
                    return list;
                }

                // Command is three most significant bits
                var command = (CompressCommand)(src[sindex] >> 5);

                // Signifies extended length copy.
                if (command == CompressCommand.LongCommand)
                {
                    // Get new command
                    command = (CompressCommand)((src[sindex] >> 2) & 0x07);
                    if (command == CompressCommand.LongCommand)
                    {
                        return null;
                    }

                    // Length is ten least significant bits.
                    clen = ((src[sindex] & 0x03) << BitsPerByte);
                    clen |= src[++sindex];
                }
                else
                {
                    clen = src[sindex] & 0x1F;
                }

                clen++;
                sindex++;

                var value = 0;
                switch (command)
                {
                case CompressCommand.RepeatedByte:
                case CompressCommand.IncrementingByte:
                value = src[sindex];
                break;

                case CompressCommand.RepeatedWord:
                case CompressCommand.CopySection:
                value = src[sindex] | (src[sindex + 1] << 8);
                break;
                }

                list.Add(new CompressInfo(command, value, dindex, clen));

                switch (command)
                {
                case CompressCommand.DirectCopy: // Direct byte copy
                dindex += clen;
                sindex += clen;
                continue;
                case CompressCommand.RepeatedByte: // Fill with one byte repeated
                dindex += clen;
                sindex++;
                continue;
                case CompressCommand.RepeatedWord: // Fill with two alternating bytes
                dindex += clen;
                sindex += 2;
                continue;
                case CompressCommand.IncrementingByte: // Fill with incrementing byte value
                dindex += clen;
                sindex++;
                continue;
                case CompressCommand.CopySection: // Copy data from previous section
                dindex += clen;
                sindex += 2;
                continue;
                case (CompressCommand)5:
                case (CompressCommand)6:
                return null;

                default:
                throw new ArgumentException();
                }
            }
            return null;
        }

        public static int GetDecompressLength(byte[] compressedData)
        {
            return GetDecompressLength(compressedData, 0, compressedData.Length);
        }

        public static int GetDecompressLength(byte[] compressedData, int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex + length > compressedData.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            unsafe
            {
                fixed (byte* ptr = &compressedData[startIndex])
                    return Decompress(null, 0, ptr, length);
            }
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            return Decompress(compressedData, 0, compressedData.Length);
        }

        public static byte[] Decompress(byte[] compressedData, int startIndex, int length)
        {
            var dlen = GetDecompressLength(compressedData);
            if (dlen == 0)
            {
                return null;
            }

            return Decompress(dlen, compressedData, startIndex, length);
        }

        public static byte[] Decompress(int decompressLength, byte[] compressedData)
        {
            return Decompress(decompressLength, compressedData, 0, compressedData.Length);
        }

        public static byte[] Decompress(int decompressLength, byte[] compressedData, int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex + length > compressedData.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (decompressLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(decompressLength));
            }

            var dest = new byte[decompressLength];

            unsafe
            {
                fixed (byte* ptr = &compressedData[startIndex])
                fixed (byte* decompress = dest)
                    if (Decompress(decompress, decompressLength, ptr, length) == 0)
                    {
                        return null;
                    }
            }

            return dest;
        }

        public static unsafe int Decompress(byte* dest, int dlen, byte* src, int slen)
        {
            int sindex = 0, dindex = 0, clen;

            while (sindex < slen)
            {
                if (src[sindex] == 0xFF)
                {
                    return dindex;
                }

                // Command is three most significant bits
                var command = (CompressCommand)(src[sindex] >> 5);

                // Signifies extended length copy.
                if (command == CompressCommand.LongCommand)
                {
                    // Get new command
                    command = (CompressCommand)((src[sindex] >> 2) & 0x07);
                    if (command == CompressCommand.LongCommand)
                    {
                        return 0;
                    }

                    // Length is ten least significant bits.
                    clen = ((src[sindex] & 0x03) << BitsPerByte);
                    clen |= src[++sindex];
                }
                else
                {
                    clen = src[sindex] & 0x1F;
                }

                clen++;
                sindex++;

                switch (command)
                {
                case CompressCommand.DirectCopy: // Direct byte copy
                if (dest != null)
                {
                    if (dindex + clen > dlen)
                    {
                        return 0;
                    }

                    if (sindex + clen > slen)
                    {
                        return 0;
                    }

                    Buffer.MemoryCopy(src + sindex, dest + dindex, dlen - dindex, clen);
                }
                dindex += clen;
                sindex += clen;
                continue;
                case CompressCommand.RepeatedByte: // Fill with one byte repeated
                if (dest != null)
                {
                    if (dindex + clen > dlen)
                    {
                        return 0;
                    }

                    if (sindex >= slen)
                    {
                        return 0;
                    }

                    var to = dest + dindex;
                    var val = src[sindex];
                    for (var i = 0; i < clen; i++)
                    {
                        to[i] = val;
                    }
                }
                dindex += clen;
                sindex++;
                continue;
                case CompressCommand.RepeatedWord: // Fill with two alternating bytes
                if (dest != null)
                {
                    if (dindex + clen > dlen)
                    {
                        return 0;
                    }

                    if (sindex + 1 >= slen)
                    {
                        return 0;
                    }

                    var to = dest + dindex;
                    var val = src[sindex];
                    for (var i = 0; i < clen; i += 2)
                    {
                        to[i] = val;
                    }

                    val = src[sindex + 1];
                    for (var i = 1; i < clen; i += 2)
                    {
                        to[i] = val;
                    }
                }
                dindex += clen;
                sindex += 2;
                continue;
                case CompressCommand.IncrementingByte: // Fill with incrementing byte value
                if (dest != null)
                {
                    if (dindex + clen > dlen)
                    {
                        return 0;
                    }

                    if (sindex >= slen)
                    {
                        return 0;
                    }

                    for (int i = 0, j = src[sindex]; i < clen; i++, j++)
                    {
                        dest[dindex + i] = (byte)j;
                    }
                }
                dindex += clen;
                sindex++;
                continue;
                case CompressCommand.CopySection: // Copy data from previous section
                if (dest != null)
                {
                    if (dindex + clen > dlen)
                    {
                        return 0;
                    }

                    if (sindex + 1 >= slen)
                    {
                        return 0;
                    }

                    // We have to manually do this copy in case of overlapping regions
                    var write = dest + dindex;
                    var read = dest + ((src[sindex + 1] << BitsPerByte) | src[sindex]);
                    for (var i = 0; i < clen; i++)
                    {
                        write[i] = read[i];
                    }
                }
                dindex += clen;
                sindex += 2;
                continue;
                case (CompressCommand)5:
                case (CompressCommand)6:
                return 0;

                default:
                throw new ArgumentException();
                }
            }
            return 0;
        }

        public int GetCompressLength(byte[] data)
        {
            return GetCompressLength(data, 0, data.Length);
        }

        public int GetCompressLength(byte[] data, int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex + length > data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            unsafe
            {
                fixed (byte* ptr = &data[startIndex])
                    return GetCompressLength(ptr, length);
            }
        }

        public unsafe int GetCompressLength(byte* data, int length)
        {
            return Compress(null, 0, data, length, true);
        }

        public byte[] Compress(byte[] data)
        {
            return Compress(data, 0, data.Length);
        }

        public byte[] Compress(byte[] data, int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex + length > data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            unsafe
            {
                fixed (byte* ptr = &data[startIndex])
                {
                    var compressLength = GetCompressLength(ptr, length);
                    var dest = new byte[compressLength];
                    fixed (byte* compress = dest)
                        if (Compress(compress, compressLength, ptr, length, false) == 0)
                        {
                            return null;
                        }

                    return dest;
                }
            }
        }

        public byte[] Compress(int compressLength, byte[] data)
        {
            return Compress(compressLength, data, 0, data.Length);
        }

        public byte[] Compress(int compressLength, byte[] data, int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex + length > data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (compressLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(compressLength));
            }

            var dest = new byte[compressLength];

            unsafe
            {
                fixed (byte* ptr = &data[startIndex])
                fixed (byte* compress = dest)
                    if (Compress(compress, compressLength, ptr, length, true) == 0)
                    {
                        return null;
                    }
            }

            return dest;
        }

        public unsafe int Compress(byte* dst, int dstLength, byte* src, int slen, bool init)
        {
            /*
             * To maximize compressed data space and decompression time, we need to examine
             * several edge cases. For example, the minimum number of bytes to reduce space
             * for a repeating byte sequence is three
             *
             * data:     00 00 00
             * compress: 12 00       - Using repeating byte compression
             * altcomp:  02 00 00 00 - Direct copy costs space
             *
             * However, if there is an uncompressable byte before and after the data, we get
             *
             * data:     01 00 00 00 10
             * compress: 00 01|12 00|00 10 - Direct copy, repeating, and direct copy
             * altcomp:  04 01 00 00 00 10 - One continuous direct copy
             *
             * Both methods take the same amount of space, but the single direct copy would be
             * preferred since it reduces computation time when decompressing.
             *
             * data:     00 00 00 10
             * compress: 12 00|00 10
             * altcomp:  03 00 00 00 10
             *
             * data:     01 00 00 00
             * compress: 00 01|12 00
             * altcomp:  03 01 00 00 00
             *
             * data:     00 00 00 01 00 00 00 02
             * compress: 12 00|00 01|12 00|00 01
             * altcomp:  12 00|04 01 00 00 00 02
             *
             * We run into more edge case pain when one or both sides of uncompressable data is over 32 bytes
             * data:     [n <= 0x20 uncompressable bytes] 00 00 00 [m > 0x20 - n uncompressable bytes]
             * compress: 0n [n bytes]|12 00|0m [m bytes] - n + m + 4 bytes
             * altcomp:  xx xx [n + m + 3 bytes] - n + m + 5 bytes
             *
             * data:     [n > 0x20] 00 00 00 [m <= 0x20]
             * compress: xx xx [n bytes]|12 00|0m [m bytes] - n + m + 5 bytes
             * altcomp:  xx xx [n + m + 3 bytes] - n + m + 5 bytes
             *
             * data:     [n > 0x20] 00 00 00 [m > 0x20]
             * compress: xx xx [n bytes]|12 00|xx xx [m bytes] - n + m + 6 bytes
             * altcomp:  xx xx [n + m + 3 bytes] - n + m + 5 bytes
             *
             * We might have a corner case where uncompressed data is over 0x400 bytes, but this will only
             * cost us one byte (maybe). And seems incredibly hard to implement efficiently. So we will
             * leave it alone, but keep it mind that it exists.
             */

            // Current source and position index
            int srcIndex = 0, dstIndex = 0, length = 0, last = 0, lastDestIndex = 0;

            if (!init)
            {
                goto _begin;
            }

            Tree.CreateTree((IntPtr)src, slen);
            Commands.Clear();

            var prev = new CompressInfo(CompressCommand.DirectCopy, 0, 0, 0);

            // Find all acceptable compression commands
            while (srcIndex < slen)
            {
                // Get the current byte
                var val0 = src[srcIndex];

                // See if we have a repeating substring.
                var substring = Tree.GetLongestInternalSubstring(srcIndex);

                // Ensure we can read a second byte
                if (srcIndex + 1 < slen)
                {
                    // Get next byte
                    var val1 = src[srcIndex + 1];

                    // Ensure we can read the third byte
                    if (srcIndex + 2 < slen)
                    {
                        // Get third byte
                        var val2 = src[srcIndex + 2];

                        // We have a repeated word (or maybe three repeated bytes)
                        if (val2 == val0)
                        {
                            // We know for sure the command length is at least three bytes
                            length = 3;

                            // If first and second byte are equal, then this is a repeated byte copy
                            if (val0 == val1)
                            {
                                goto _repeatedByteCopy;
                            }

                            // Determine how long the repeated word copy goes
                            for (; srcIndex + length < slen; length++)
                            {
                                if (src[srcIndex + length] != val1)
                                {
                                    break;
                                }

                                if (srcIndex + ++length >= slen)
                                {
                                    break;
                                }

                                if (src[srcIndex + length] != val0)
                                {
                                    break;
                                }
                            }

                            if (length > 3)
                            {
                                if (substring.Length > length)
                                {
                                    Add(CompressCommand.CopySection, substring.Start, srcIndex, substring.Length);
                                    srcIndex += substring.Length;
                                    last = srcIndex;
                                    continue;
                                }
                                // Get the repeated word value
                                Add(CompressCommand.RepeatedWord, val0 + (val1 << BitsPerByte), srcIndex, length);
                                srcIndex += length;
                                last = srcIndex;
                                continue;
                            }
                        }
                    }

                    // See if we have a repeating byte sequence
                    if (val1 != val0)
                    {
                        goto _incrementedByteCheck;
                    }

                    length = 2;

                    // Note that this label is called when length == 3.
                    _repeatedByteCopy:
                    for (; srcIndex + length < slen; length++)
                    {
                        if (src[srcIndex + length] != val0)
                        {
                            break;
                        }
                    }

                    if (length > 2)
                    {
                        if (substring.Length > length + 1)
                        {
                            Add(CompressCommand.CopySection, substring.Start, srcIndex, substring.Length);
                            srcIndex += substring.Length;
                            last = srcIndex;
                            continue;
                        }
                        // Set repeated byte value
                        Add(CompressCommand.RepeatedByte, val0, srcIndex, length);
                        srcIndex += length;
                        last = srcIndex;
                        continue;
                    }

                    // See if we have an incrementing byte sequence.
                    _incrementedByteCheck:
                    if (val1 == (byte)(val0 + 1))
                    {
                        // Determine how long the incrementing byte sequence goes for.
                        for (length = 2; srcIndex + length < slen; length++)
                        {
                            if (src[srcIndex + length] != (byte)(val0 + length))
                            {
                                break;
                            }
                        }

                        if (length > 2)
                        {
                            if (substring.Length > length + 1)
                            {
                                Add(
                                    CompressCommand.CopySection,
                                    substring.Start,
                                    srcIndex,
                                    substring.Length);
                                srcIndex += substring.Length;
                                last = srcIndex;
                                continue;
                            }
                            // Set incrementing byte value.
                            Add(
                                CompressCommand.IncrementingByte,
                                val0,
                                srcIndex,
                                length);
                            srcIndex += length;
                            last = srcIndex;
                            continue;
                        }
                    }
                }

                if (substring.Length > 3)
                {
                    Add(
                        CompressCommand.CopySection,
                        substring.Start,
                        srcIndex,
                        substring.Length);
                    srcIndex += substring.Length;
                    last = srcIndex;
                    continue;
                }

                srcIndex++;
                continue;
            }

            // Add the last direct copy if we must
            if (srcIndex != last)
            {
                Add(CompressCommand.DirectCopy, 0, last, srcIndex - last);
            }

            // Now we write the compression commands
            dstIndex = 0;
            lastDestIndex = 0;

            _begin:
            int i = 0, count = Commands.Count;
            CompressInfo current, next;
            var previous = new CompressInfo((CompressCommand)(-1), 0, 0, 0);

            // This is for when we're at the last command, we can still read the "next" one.
            // Note we add this *after* getting the count
            Commands.Add(new CompressInfo((CompressCommand)(-1), 0, 0, 0));

            _loop:
            while (i < count)
            {
                current = Commands[i];
                next = Commands[i + 1];

                if (previous.Command == CompressCommand.DirectCopy && next.Command == CompressCommand.DirectCopy)
                {
                    var tlen = next.End - previous.Index;
                    var add = (previous.Length > 0x20 && next.Length > 0x20) ? 1 : 0;
                    var edge = (previous.Length <= 0x20 && next.Length <= 0x20) && tlen > 0x20;

                    switch (current.Command)
                    {
                    case CompressCommand.RepeatedByte:
                    case CompressCommand.IncrementingByte:
                    if (edge && current.Length == 3)
                    {
                        break;
                    }

                    if (current.Length > 3 + add)
                    {
                        break;
                    }

                    Commands[++i] = new CompressInfo(CompressCommand.DirectCopy, 0, previous.Index, tlen);
                    dstIndex = lastDestIndex;
                    continue;
                    case CompressCommand.RepeatedWord:
                    case CompressCommand.CopySection:
                    if (edge && current.Length == 4)
                    {
                        break;
                    }

                    if (current.Length > 4 + add)
                    {
                        break;
                    }

                    Commands[++i] = new CompressInfo(
                        CompressCommand.DirectCopy,
                        0,
                        previous.Index,
                        tlen);
                    dstIndex = lastDestIndex;
                    continue;
                    }
                }

                goto _write;
            }
            goto _end;

            _write:

            // We use this to account for 0x400 byte sections of section copies and direct copies.
            srcIndex = 0;

            var command = current.Command;
            for (length = current.Length; length > 0;)
            {
                var subLength = Math.Min(length, 0x400);
                if (dst != null)
                {
                    if (subLength > 0x20)
                    {
                        if (command == CompressCommand.DirectCopy)
                        {
                            if (dstIndex + 2 + subLength > dstLength)
                            {
                                return 0;
                            }
                        }
                        else if (dstIndex + 2 + CommandSizes[(int)command] + 1 > dstLength)
                        {
                            return 0;
                        }

                        lastDestIndex = dstIndex;
                        dst[dstIndex++] = (byte)(((int)CompressCommand.LongCommand << (BitsPerByte - 3)) | ((int)command << 2) | (--subLength >> BitsPerByte));
                        dst[dstIndex++] = (byte)subLength++;
                    }
                    else
                    {
                        if (command == CompressCommand.DirectCopy)
                        {
                            if (dstIndex + 1 + subLength > dstLength)
                            {
                                return 0;
                            }
                        }
                        else if (dstIndex + 1 + CommandSizes[(int)command] + 1 > dstLength)
                        {
                            return 0;
                        }

                        lastDestIndex = dstIndex;
                        dst[dstIndex++] = (byte)(((int)command) << (BitsPerByte - 3) | (subLength - 1));
                    }
                    switch (command)
                    {
                    case CompressCommand.RepeatedByte:
                    case CompressCommand.IncrementingByte:
                    dst[dstIndex++] = (byte)current.Value;
                    break;

                    case CompressCommand.RepeatedWord:
                    dst[dstIndex++] = (byte)current.Value;
                    dst[dstIndex++] = (byte)(current.Value >> BitsPerByte);
                    break;

                    case CompressCommand.CopySection:
                    dst[dstIndex++] = (byte)current.Value;
                    dst[dstIndex++] = (byte)((current.Value + srcIndex) >> BitsPerByte);
                    srcIndex += subLength;
                    break;

                    case CompressCommand.DirectCopy:
                    Buffer.MemoryCopy(
                        src + srcIndex + current.Index,
                        dst + dstIndex,
                        dstLength - dstIndex,
                        subLength);
                    dstIndex += subLength;
                    srcIndex += subLength;
                    break;

                    default:
                    throw new ArgumentException();
                    }
                }
                else
                {
                    lastDestIndex = dstIndex;
                    if (subLength > 0x20)
                    {
                        dstIndex += 2;
                    }
                    else
                    {
                        dstIndex++;
                    }

                    if (command == CompressCommand.DirectCopy)
                    {
                        dstIndex += subLength;
                    }
                    else
                    {
                        dstIndex += CommandSizes[(int)current.Command];
                    }
                }

                length -= subLength;
            }
            previous = Commands[i++];
            goto _loop;

            _end:
            if (dst != null)
            {
                dst[dstIndex] = 0xFF;
            }

            return ++dstIndex;
        }

        private void Add(CompressCommand command, int value, int index, int length)
        {
            Add(new CompressInfo(command, value, index, length));
        }

        private void Add(CompressInfo module)
        {
            var command = module.Command;
            if (command != CompressCommand.DirectCopy)
            {
                if (Commands.Count > 0)
                {
                    var last = Commands[Commands.Count - 1];
                    if (last.End != module.Index)
                    {
                        Commands.Add(new CompressInfo(CompressCommand.DirectCopy, 0, last.End, module.Index - last.End));
                    }
                }
                else if (module.Index > 0)
                {
                    Commands.Add(new CompressInfo(CompressCommand.DirectCopy, 0, 0, module.Index));
                }
            }

            Commands.Add(module);
        }
    }
}
