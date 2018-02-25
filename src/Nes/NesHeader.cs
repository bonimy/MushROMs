// <copyright file="NesHeader.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Nes.Emulator
{
    using System;
    using System.Text;

    public struct NesHeader : IEquatable<NesHeader>
    {
        public const int Size = 0x10;

        public const string ExpectedName = "NES\x1A";

        public const int NameSize = 4;

        public static readonly NesHeader SmbHeader = new NesHeader(
            ExpectedName,
            2,
            1,
            1,
            0);

        public NesHeader(byte[] header)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            if (header.Length < Size)
            {
                throw new ArgumentException();
            }

            Name = Encoding.ASCII.GetString(header, 0, NameSize);
            PrgSize = header[4];
            ChrSize = header[5];
            RomType1 = header[6];
            RomType2 = header[7];
        }

        private NesHeader(
            string type,
            int prgSize,
            int chrSize,
            int romType1,
            int romType2)
        {
            Name = type;
            PrgSize = prgSize;
            ChrSize = chrSize;
            RomType1 = romType1;
            RomType2 = romType2;
        }

        public string Name
        {
            get;
        }

        public int PrgSize
        {
            get;
        }

        public int ChrSize
        {
            get;
        }

        public int RomType1
        {
            get;
        }

        public int RomType2
        {
            get;
        }

        public byte[] ToData()
        {
            var header = new byte[Size];

            Array.Copy(Encoding.ASCII.GetBytes(Name), header, NameSize);

            header[4] = (byte)PrgSize;
            header[5] = (byte)ChrSize;
            header[6] = (byte)RomType1;
            header[7] = (byte)RomType2;

            return header;
        }

        public override bool Equals(object obj)
        {
            if (obj is NesHeader value)
            {
                return Equals(value);
            }

            return false;
        }

        public bool Equals(NesHeader obj)
        {
            return
                Name.Equals(obj.Name) &&
                PrgSize.Equals(obj.PrgSize) &&
                ChrSize.Equals(obj.ChrSize) &&
                RomType1.Equals(obj.RomType1) &&
                RomType2.Equals(obj.RomType2);
        }

        public override int GetHashCode()
        {
            return PrgSize ^ ChrSize;
        }
    }
}
