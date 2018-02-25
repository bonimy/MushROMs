// <copyright file="PrgRom.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class PrgRom : IReadOnlyList<byte>
    {
        public const int Size = 0x10000;

        public PrgRom()
        {
            Data = new byte[Size];
            ReadHardwareRegisters = new Dictionary<int, Func<int, int>>();
            WriteHardwareRegisters = new Dictionary<int, Action<int, int>>();
        }

        internal PrgRom(byte[] data)
            : this()
        {
            Array.Copy(data, Data, Size);
        }

        int IReadOnlyCollection<byte>.Count
        {
            get
            {
                return Size;
            }
        }

        public IDictionary<int, Func<int, int>> ReadHardwareRegisters
        {
            get;
        }

        public IDictionary<int, Action<int, int>> WriteHardwareRegisters
        {
            get;
        }

        private byte[] Data
        {
            get;
        }

        public byte this[int index]
        {
            get
            {
                if (ReadHardwareRegisters.TryGetValue(
                    index,
                    out var readHarwardRegister))
                {
                    return (byte)readHarwardRegister(index);
                }

                return Data[index];
            }

            set
            {
                if (WriteHardwareRegisters.TryGetValue(
                    index,
                    out var writeHardwareRegister))
                {
                    writeHardwareRegister(index, value);
                }

                Data[index] = value;
            }
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            Data.CopyTo(array, arrayIndex);
        }

        public void CopyTo(byte[] array, int arrayIndex, int length)
        {
            Array.Copy(Data, 0, array, arrayIndex, length);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return (Data as IReadOnlyList<byte>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
