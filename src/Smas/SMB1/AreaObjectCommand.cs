// <copyright file="AreaObjectCommand.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Smas.Smb1
{
    using System;
    using static Helper.SR;
    using NesAreaObjectCommand = global::Smb1.AreaObjectCommand;

    public struct AreaObjectCommand : IEquatable<AreaObjectCommand>
    {
        public AreaObjectCommand(byte value1, byte value2)
            : this(value1, value2, 0)
        {
        }

        public AreaObjectCommand(byte value1, byte value2, byte value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = (byte)(((value1 & 0x0F) == 0x0F) ? value3 : 0);
        }

        public AreaObjectCommand(
            int x,
            int y,
            bool pageFlag,
            int command,
            int parameter,
            int extendedCommand)
            : this()
        {
            ExtendedCommand = extendedCommand;
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

        public byte Value3
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the size, in bytes, of this <see cref="AreaObjectCommand"/>.
        /// </summary>
        public int Size
        {
            get
            {
                return IsExtendedObject ? 3 : 2;
            }
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
                if (IsExtendedObject)
                {
                    return Value2 >> 4;
                }
                else
                {
                    return Value1 & 0x0F;
                }
            }

            set
            {
                if (IsExtendedObject)
                {
                    Value2 &= 0x0F;
                    Value2 |= (byte)((value & 0x0F) << 4);
                }
                else
                {
                    Value1 &= 0xF0;
                    Value1 |= (byte)(value & 0x0F);
                }
            }
        }

        public bool PageFlag
        {
            get
            {
                return ((IsExtendedObject ? Value3 : Value2) & 0x80) != 0;
            }

            set
            {
                if (IsExtendedObject)
                {
                    if (value)
                    {
                        Value3 |= 0x80;
                    }
                    else
                    {
                        Value3 &= 0x7F;
                    }
                }
                else
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
        }

        public int Command
        {
            get
            {
                if (IsExtendedObject)
                {
                    return (Value3 >> 4) & 7;
                }
                else
                {
                    return (Value2 >> 4) & 7;
                }
            }

            set
            {
                if (IsExtendedObject)
                {
                    Value3 &= 0x8F;
                    Value3 |= (byte)((value & 7) << 4);
                }
                else
                {
                    Value2 &= 0x8F;
                    Value2 |= (byte)((value & 7) << 4);
                }
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

        public int ExtendedCommand
        {
            get
            {
                if (IsExtendedObject)
                {
                    return Value3 & 0x0F;
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                var p = PageFlag;
                var c = Command;
                var v = Parameter;
                var y = Y;

                if (value == -1)
                {
                    IsExtendedObject = false;
                    Y = y;
                    PageFlag = p;
                    Command = c;
                    Parameter = v;
                    Value3 = 0;
                }
                else
                {
                    IsExtendedObject = true;
                    Y = y;
                    PageFlag = p;
                    Command = c;
                    Parameter = v;
                    Value3 &= 0xF0;
                    Value3 |= (byte)(value & 0x0F);
                }
            }
        }

        public AreaObjectCode AreaObjectCode
        {
            get
            {
                if (IsExtendedObject)
                {
                    return (AreaObjectCode)(
                        ExtendedCommand << 0x0C |
                        0xF00 |
                        (Command << 4));
                }

                if (Y >= 0x0C)
                {
                    if (Y == 0x0D)
                    {
                        if (Command == 0)
                        {
                            return AreaObjectCode.PageSkip;
                        }

                        return (AreaObjectCode)(
                        (Y << 8) |
                        (Command << 4) |
                        Parameter);
                    }
                    else if (Y == 0x0E)
                    {
                        return (AreaObjectCode)(0x0E00 | (Command >= 4 ? 0x40 : 0));
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

        public bool IsEmpty
        {
            get
            {
                switch (AreaObjectCode)
                {
                    case AreaObjectCode.Empty:
                    case AreaObjectCode.Empty2:
                    case AreaObjectCode.Empty3:
                    case AreaObjectCode.Empty4:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public bool IsExtendedObject
        {
            get
            {
                return (Value1 & 0x0F) == 0x0F;
            }

            private set
            {
                if (value)
                {
                    Value1 |= 0x0F;
                }
                else
                {
                    Value1 &= 0xF0;
                }
            }
        }

        public static implicit operator AreaObjectCommand(NesAreaObjectCommand src)
        {
            if (src.Y == 0x0F)
            {
                return new AreaObjectCommand(
                    src.X,
                    0x0F,
                    src.PageFlag,
                    src.Command,
                    src.Parameter,
                    0);
            }

            return new AreaObjectCommand(
                src.X,
                src.Y,
                src.PageFlag,
                src.Command,
                src.Parameter,
                -1);
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
            if (obj is AreaObjectCommand command)
            {
                return command == this;
            }

            return false;
        }

        public bool Equals(AreaObjectCommand other)
        {
            if (IsExtendedObject != other.IsExtendedObject)
            {
                return false;
            }

            if (Value1 != other.Value1 || Value2 != other.Value2)
            {
                return false;
            }

            if (!IsExtendedObject)
            {
                return true;
            }

            return Value3 == other.Value3;
        }

        public override int GetHashCode()
        {
            return Value1 | (Value2 << 8) | (Value3 << 16);
        }

        public override string ToString()
        {
            return GetString(
                "({0:X}, {1:X}): {2}",
                X,
                Y,
                AreaObjectCode);
        }
    }
}
