// <copyright file="Rom.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes.Emulator
{
    using System;
    using System.IO;

    public class Rom
    {
        public Rom(string path)
            : this(File.ReadAllBytes(path))
        {
            Path = path;
        }

        public Rom(byte[] data)
        {
            Data = data;

            var header = new byte[NesHeader.Size];
            Array.Copy(
                Data,
                header,
                NesHeader.Size);

            var prgRom = new byte[PrgRom.Size];
            Array.Copy(
                Data,
                NesHeader.Size,
                prgRom,
                0x8000,
                0x8000);

            var chrRom = new byte[ChrRom.Size];
            Array.Copy(
                Data,
                NesHeader.Size + 0x8000,
                chrRom,
                0,
                ChrRom.Size);

            Header = new NesHeader(header);
            PrgRom = new PrgRom(prgRom);
            ChrRom = new ChrRom(chrRom);
        }

        public string Path
        {
            get;
        }

        public NesHeader Header
        {
            get;
        }

        public PrgRom PrgRom
        {
            get;
        }

        public ChrRom ChrRom
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
                return Data[index];
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
    }
}
