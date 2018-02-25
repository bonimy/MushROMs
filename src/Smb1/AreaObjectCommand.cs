// <copyright file="AreaObjectCommand.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Smb1
{
    using System;
    using Helper;
    using static Helper.SR;

    public struct AreaObjectCommand : IEquatable<AreaObjectCommand>
    {
        /// <summary>
        /// The size, in bytes, of this <see cref="AreaObjectCommand"/>.
        /// </summary>
        public const int Size = 2;

        public AreaObjectCommand(byte value1, byte value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public AreaObjectCommand(
            int x,
            int y,
            bool pageFlag,
            int command,
            int parameter)
            : this()
        {
            X = x;
            Y = y;
            PageFlag = pageFlag;
            Command = command;
            Parameter = parameter;
        }

        public byte Value1
        {
            get;
            set;
        }

        public byte Value2
        {
            get;
            set;
        }

        public int X
        {
            get
            {
                return Value1 >> 4;
            }

            set
            {
                Value1 &= 0x0F;
                Value1 |= (byte)((value & 0x0F) << 4);
            }
        }

        public int Y
        {
            get
            {
                return Value1 & 0x0F;
            }

            set
            {
                Value1 &= 0xF0;
                Value1 |= (byte)(value & 0x0F);
            }
        }

        public bool PageFlag
        {
            get
            {
                return (Value2 & 0x80) != 0;
            }

            set
            {
                if (value)
                {
                    Value2 |= 0x80;
                }
                else
                {
                    Value2 &= 0x7F;
                }
            }
        }

        public int Command
        {
            get
            {
                return (Value2 >> 4) & 7;
            }

            set
            {
                Value2 &= 0x8F;
                Value2 |= (byte)((value & 7) << 4);
            }
        }

        public int Parameter
        {
            get
            {
                return Value2 & 0x0F;
            }

            set
            {
                Value2 &= 0xF0;
                Value2 |= (byte)(value & 0x0F);
            }
        }

        public AreaObjectCode ObjectType
        {
            get
            {
                if (Y >= 0x0C)
                {
                    if (Y == 0x0D)
                    {
                        if (Command == 0)
                        {
                            return AreaObjectCode.PageSkip;
                        }

                        var code = (Y << 8) | (Command << 4) | Parameter;
                        return (AreaObjectCode)code;
                    }

                    return (AreaObjectCode)((Y << 8) | (Command << 4));
                }

                if (Command == 0)
                {
                    return (AreaObjectCode)Parameter;
                }

                return (AreaObjectCode)(Command << 4);
            }
        }

        public static bool operator ==(
            AreaObjectCommand left,
            AreaObjectCommand right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            AreaObjectCommand left,
            AreaObjectCommand right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is AreaObjectCommand other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(AreaObjectCommand other)
        {
            return
                Value1.Equals(other.Value1) &&
                Value2.Equals(other.Value2);
        }

        public override int GetHashCode()
        {
            return Value1 | (Value2 << 8);
        }

        public override string ToString()
        {
            return GetString(
                "({0:X}, {1:X}): {2}",
                X & 0x0F,
                Y,
                ObjectType);
        }
    }
}
