// <copyright file="ChrRom.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes.Emulator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ChrRom : IReadOnlyList<byte>
    {
        public const int Size = 0x2000;

        public ChrRom()
        {
            Data = new byte[Size];
        }

        internal ChrRom(byte[] data)
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

        private byte[] Data
        {
            get;
        }

        public byte this[int index]
        {
            get
            {
                return Data[index];
            }

            set
            {
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
