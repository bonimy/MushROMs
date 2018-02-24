// <copyright file="AreaObjectCommand.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using Helper;

namespace MushROMs.SMAS.SMB1
{
    public struct AreaObjectCommand
    {
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
        /// The size, in bytes, of this <see cref="AreaObjectCommand"/>.
        /// </summary>
        public int Size
        {
            get
            {
                return YInternal ? 3 : 2;
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

        private bool YInternal
        {
            get
            {
                return (Value1 & 0x0F) == 0x0F;
            }

            set
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

        public int Y
        {
            get
            {
                if (YInternal)
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
                if (YInternal)
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
                return ((YInternal ? Value3 : Value2) & 0x80) != 0;
            }

            set
            {
                if (YInternal)
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
                if (YInternal)
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
                if (YInternal)
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
                if (YInternal)
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
                    YInternal = false;
                    Y = y;
                    PageFlag = p;
                    Command = c;
                    Parameter = v;
                    Value3 = 0;
                }
                else
                {
                    YInternal = true;
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
                if (YInternal)
                {
                    return (AreaObjectCode)((ExtendedCommand << 0x0C | 0xF00 | (Command << 4)));
                }
                else
                {
                    if (Y >= 0x0C)
                    {
                        if (Y == 0x0D)
                        {
                            if (Command == 0)
                            {
                                return AreaObjectCode.PageSkip;
                            }
                            else
                            {
                                return (AreaObjectCode)((Y << 8) | (Command << 4) | (Parameter));
                            }
                        }
                        else if (Y == 0x0E)
                        {
                            return (AreaObjectCode)(0x0E00 | (Command >= 4 ? 0x40 : 0));
                        }
                        else
                        {
                            return (AreaObjectCode)((Y << 8) | (Command << 4));
                        }
                    }
                    else if (Command == 0)
                    {
                        return (AreaObjectCode)Parameter;
                    }
                    else
                    {
                        return (AreaObjectCode)(Command << 4);
                    }
                }
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

        public AreaObjectCommand(byte value1, byte value2) : this(value1, value2, 0)
        { }

        public AreaObjectCommand(byte value1, byte value2, byte value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = (byte)(((value1 & 0x0F) == 0x0F) ? value3 : 0);
        }

        public AreaObjectCommand(
            int x, int y, bool pageFlag,
            int command, int parameter, int extendedCommand) : this()
        {
            ExtendedCommand = extendedCommand;
            X = x;
            Y = y;
            PageFlag = pageFlag;
            Command = command;
            Parameter = parameter;
        }

        public static implicit operator AreaObjectCommand(MushROMs.SMB1.AreaObjectCommand src)
        {
            if (src.Y == 0x0F)
            {
                return new AreaObjectCommand(src.X, 0x0F, src.PageFlag, src.Command, src.Parameter, 0);
            }
            else
            {
                return new AreaObjectCommand(src.X, src.Y, src.PageFlag, src.Command, src.Parameter, -1);
            }
        }

        public static bool operator ==(AreaObjectCommand left, AreaObjectCommand right)
        {
            if (left.Size == 2 && right.Size == 2)
            {
                return left.Value1 == right.Value1 &&
                    left.Value2 == right.Value2;
            }
            else if (left.Size == 3 && right.Size == 3)
            {
                return left.Value1 == right.Value1 &&
                    left.Value2 == right.Value2 &&
                    left.Value3 == right.Value3;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(AreaObjectCommand left, AreaObjectCommand right)
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

        public override int GetHashCode()
        {
            return (Value1) | (Value2 << 8) | (Value3 << 16);
        }

        public override string ToString()
        {
            return SR.GetString("({0}, {1}): {2}", X.ToString("X"), Y.ToString("X"), AreaObjectCode);
        }
    }
}
