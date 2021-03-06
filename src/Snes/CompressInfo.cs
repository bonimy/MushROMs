﻿// <copyright file="CompressInfo.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using System.Collections.Generic;
    using static Helper.StringHelper;

    public struct CompressInfo
    {
        private const int BitsPerByte = 8;

        public CompressInfo(
            CompressCommand command,
            int value,
            int index,
            int length)
        {
            Command = command;
            Value = value;
            Index = index;
            Length = length;
        }

        public CompressCommand Command
        {
            get;
        }

        public int Value
        {
            get;
        }

        public int Index
        {
            get;
        }

        public int Length
        {
            get;
        }

        public int End
        {
            get
            {
                return Index + Length;
            }
        }

        public int CommandLength
        {
            get
            {
                var len = Length > 0x20 ? 2 : 1;
                switch (Command)
                {
                    case CompressCommand.DirectCopy:
                        return len + Length;

                    case CompressCommand.RepeatedByte:
                    case CompressCommand.IncrementingByte:
                        return len + 1;

                    case CompressCommand.RepeatedWord:
                    case CompressCommand.CopySection:
                        return len + 2;

                    default:
                        throw new ArgumentException();
                }
            }
        }

        public static int GetTotalLength(
            IEnumerable<CompressInfo> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var result = 0;
            foreach (var info in collection)
            {
                result += info.CommandLength;
            }

            return result;
        }

        public static List<CompressInfo> GetCompressList(
            byte[] src,
            int index)
        {
            var list = new List<CompressInfo>();

            int sindex = 0, dindex = 0, clen;
            var slen = src.Length;

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
                    command =
                        (CompressCommand)((src[sindex] >> 2) & 0x07);

                    if (command == CompressCommand.LongCommand)
                    {
                        throw new ArgumentException();
                    }

                    // Length is ten least significant bits.
                    clen = (src[sindex] & 0x03) << BitsPerByte;
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

                var compressInfo = new CompressInfo(
                    command,
                    value,
                    dindex,
                    clen);

                list.Add(compressInfo);

                switch (command)
                {
                    // Direct byte copy
                    case CompressCommand.DirectCopy:
                        dindex += clen;
                        sindex += clen;
                        continue;

                    // Fill with one byte repeated
                    case CompressCommand.RepeatedByte:
                        dindex += clen;
                        sindex++;
                        continue;

                    // Fill with two alternating bytes
                    case CompressCommand.RepeatedWord:
                        dindex += clen;
                        sindex += 2;
                        continue;

                    // Fill with incrementing byte value
                    case CompressCommand.IncrementingByte:
                        dindex += clen;
                        sindex++;
                        continue;

                    // Copy data from previous section
                    case CompressCommand.CopySection:
                        dindex += clen;
                        sindex += 2;
                        continue;

                    default:
                        throw new ArgumentException();
                }
            }

            return null;
        }

        public override string ToString()
        {
            return GetString(
                "Index = {0}, Length = {1}, Command = {2}",
                Index,
                Length,
                Command);
        }
    }
}
