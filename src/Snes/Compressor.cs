// <copyright file="Compressor.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

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

        /// <summary>
        /// The default <see cref="Compressor"/>. Use this instance for decompressing only when operating in a single thread.
        /// </summary>
        public static readonly Compressor Default = new Compressor();

        private static readonly int[] CommandSizes = { -1, 1, 2, 1, 2 };

        private SuffixTree Tree
        {
            get;
            set;
        }

        private Dictionary<CompressCommand, Action> DecompressCommands
        {
            get;
            set;
        }

        private List<CompressInfo> Commands
        {
            get;
            set;
        }

        private unsafe byte* Uncompressed
        {
            get;
            set;
        }

        private unsafe byte CurrentUncompressed
        {
            get
            {
                return Uncompressed[UncompressedIndex];
            }

            set
            {
                Uncompressed[UncompressedIndex] = value;
            }
        }

        private unsafe byte NextUncompressed
        {
            get
            {
                return Uncompressed[++UncompressedIndex];
            }

            set
            {
                Uncompressed[++UncompressedIndex] = value;
            }
        }

        private int UncompressedIndex
        {
            get;
            set;
        }

        private int UncompressedLength
        {
            get;
            set;
        }

        private unsafe byte* Compressed
        {
            get;
            set;
        }

        private unsafe byte CurrentCompressed
        {
            get
            {
                return Compressed[CompressedIndex];
            }

            set
            {
                Compressed[CompressedIndex] = value;
            }
        }

        private unsafe byte NextCompressed
        {
            get
            {
                return Compressed[++CompressedIndex];
            }

            set
            {
                Compressed[++CompressedIndex] = value;
            }
        }

        private int CompressedIndex
        {
            get;
            set;
        }

        private int CompressedLength
        {
            get;
            set;
        }

        private int CommandLength
        {
            get;
            set;
        }

        public Compressor()
        {
            Tree = new SuffixTree();
            Commands = new List<CompressInfo>(0x100);

            DecompressCommands = new Dictionary<CompressCommand, Action>()
            {
                { CompressCommand.DirectCopy, AddDirectCopy },
                { CompressCommand.RepeatedByte, AddRepeatedByte },
                { CompressCommand.RepeatedWord, AddRepeatedWord },
                { CompressCommand.IncrementingByte, AddIncrementingByte },
                { CompressCommand.CopySection, AddCopySection }
            };
        }

        public int GetDecompressLength(byte[] compressedData)
        {
            return GetDecompressLength(compressedData, 0, compressedData.Length);
        }

        public int GetDecompressLength(byte[] compressedData, int startIndex, int length)
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

        private void AssertSufficientSpace(int padding)
        {
            if (UncompressedIndex + CommandLength > UncompressedLength)
            {
                throw new InvalidOperationException();
            }

            if (CompressedIndex + padding >= CompressedLength)
            {
                throw new InvalidOperationException();
            }
        }

        private unsafe void AddDirectCopy()
        {
            if (Uncompressed != null)
            {
                AssertSufficientSpace(CommandLength);

                Buffer.MemoryCopy(
                    Compressed + CompressedIndex,
                    Uncompressed + UncompressedIndex,
                    UncompressedLength - UncompressedIndex,
                    CommandLength);
            }

            UncompressedIndex += CommandLength;
            CompressedIndex += CommandLength;
        }

        private unsafe void AddRepeatedByte()
        {
            if (Uncompressed != null)
            {
                AssertSufficientSpace(0);

                var dst = Uncompressed + UncompressedIndex;
                var value = Compressed[CompressedIndex];

                // We know CommandLength > 0, so a do-while loop is (one check) faster.
                var i = CommandLength;
                do
                {
                    dst[--i] = value;
                }
                while (i > 0);
            }

            UncompressedIndex += CommandLength;
            CompressedIndex++;
        }

        private unsafe void AddRepeatedWord()
        {
            if (Uncompressed != null)
            {
                AssertSufficientSpace(1);

                var dst = Uncompressed + UncompressedIndex;
                var value1 = Compressed[CompressedIndex];
                var value2 = Compressed[CompressedIndex + 1];

                var i = CommandLength;

                // Determine whether CommandLength is even or odd.
                if ((CommandLength & 1) == 0)
                {
                    // Command length is even.

                    // We're guaranteed CommandLength > 0 and therefore >= 2 since it is also even.
                    do
                    {
                        dst[--i] = value2;
                        dst[--i] = value1;
                    }
                    while (i > 0);
                }
                else
                {
                    // Command length is odd.

                    // Write the last odd value first.
                    dst[--i] = value1;

                    // We're not guaranteed CommandLength > 1. Consider the compression command 40 00 FF. It's a 1 byte word copy. Inefficient, but it can exist.
                    while (i > 0)
                    {
                        dst[--i] = value2;
                        dst[--i] = value1;
                    }
                }
            }

            UncompressedIndex += CommandLength;
            CompressedIndex += 2;
        }

        private unsafe void AddIncrementingByte()
        {
            if (Uncompressed != null)
            {
                AssertSufficientSpace(0);

                var dst = Uncompressed + UncompressedIndex;
                var value = Compressed[CompressedIndex];

                // Offset value to last incremented.
                value += (byte)CommandLength;

                // We know CommandLength > 0, so a do-while loop is (one check) faster.
                var i = CommandLength;
                do
                {
                    dst[--i] = --value;
                }
                while (i > 0);
            }

            UncompressedIndex += CommandLength;
            CompressedIndex++;
        }

        private unsafe void AddCopySection()
        {
            if (Uncompressed != null)
            {
                AssertSufficientSpace(1);

                var dst = Uncompressed + UncompressedIndex;
                var value1 = Compressed[CompressedIndex];
                var value2 = Compressed[CompressedIndex + 1];
                var src = Uncompressed + (value1 | (value2 << BitsPerByte));

                // We have to go in forward order because the section can overlap itself.
                var i = 0;
                do
                {
                    dst[i] = src[i];
                }
                while (++i < CommandLength);
            }

            UncompressedIndex += CommandLength;
            CompressedIndex += 2;
        }

        private CompressCommand InitCommandData()
        {
            // Command is three most significant bits
            var command = (CompressCommand)(CurrentCompressed >> 5);
            int clen;

            // Signifies extended length copy.
            if (command == CompressCommand.LongCommand)
            {
                // Get new command
                command = (CompressCommand)((CurrentCompressed >> 2) & 0x07);
                if (command == CompressCommand.LongCommand)
                {
                    throw new InvalidOperationException();
                }

                // Length is ten least significant bits.
                clen = (CurrentCompressed & 0x03) << BitsPerByte;
                clen |= NextCompressed;
            }
            else
            {
                clen = CurrentCompressed & 0x1F;
            }

            CommandLength = clen + 1;
            CompressedIndex++;

            return command;
        }

        public byte[] Decompress(byte[] compressedData)
        {
            return Decompress(compressedData, 0, compressedData.Length);
        }

        public byte[] Decompress(byte[] compressedData, int startIndex, int length)
        {
            var dlen = GetDecompressLength(compressedData);
            if (dlen == 0)
            {
                return null;
            }

            return Decompress(dlen, compressedData, startIndex, length);
        }

        public byte[] Decompress(int decompressLength, byte[] compressedData)
        {
            return Decompress(decompressLength, compressedData, 0, compressedData.Length);
        }

        public byte[] Decompress(int decompressLength, byte[] compressedData, int startIndex, int length)
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

        private unsafe int Decompress(byte* dest, int dlen, byte* src, int slen)
        {
            Compressed = src;
            Uncompressed = dest;
            CompressedIndex = 0;
            UncompressedIndex = 0;
            CompressedLength = slen;
            UncompressedLength = dlen;

            while (CompressedIndex < CompressedLength)
            {
                if (src[CompressedIndex] == 0xFF)
                {
                    return UncompressedIndex;
                }

                var command = InitCommandData();

                if (!DecompressCommands.ContainsKey(command))
                {
                    throw new InvalidOperationException();
                }

                DecompressCommands[command]();
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

        private unsafe int GetCompressLength(byte* data, int length)
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

        private unsafe int Compress(byte* dst, int dstLength, byte* src, int slen, bool init)
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
