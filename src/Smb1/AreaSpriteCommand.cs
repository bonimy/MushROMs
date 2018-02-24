// <copyright file="AreaSpriteCommand.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using Helper;

namespace MushROMs.SMB1
{
    public struct AreaSpriteCommand
    {
        /// <summary>
        /// Gets or sets the first value of this <see cref="AreaSpriteCommand"/>.
        /// </summary>
        public byte Value1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second value of this <see cref="AreaSpriteCommand"/>.
        /// </summary>
        public byte Value2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the third value of this <see cref="AreaSpriteCommand"/>.
        /// </summary>
        public byte Value3
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the size, in bytes, of this <see cref="AreaSpriteCommand"/>.
        /// </summary>
        public int Size
        {
            get
            {
                return Y == 0x0E ? 3 : 2;
            }
        }

        /// <summary>
        /// Gets or sets the X-coordinate of this <see cref="AreaSpriteCommand"/>.
        /// The coordinate is relative to the page the object is in.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Y-coordinate of this <see cref="AreaSpriteCommand"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a flag that determines whether this <see cref="AreaSpriteCommand"/>
        /// starts on the next page.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a flag that determines whether this <see cref="AreaSpriteCommand"/>
        /// only spawns after the hard world flag has been set.
        /// </summary>
        public bool HardWorldFlag
        {
            get
            {
                return (Value2 & 0x40) != 0;
            }

            set
            {
                if (value)
                {
                    Value2 |= 0x40;
                }
                else
                {
                    Value2 &= 0xBF;
                }
            }
        }

        /// <summary>
        /// Gets or sets the command value of this <see cref="AreaSpriteCommand"/>.
        /// </summary>
        public AreaSpriteCode Code
        {
            get
            {
                return (AreaSpriteCode)(Value2 & 0x3F);
            }

            set
            {
                Value2 &= 0xC0;
                Value2 |= (byte)((int)value & 0x3F);
            }
        }

        public AreaSpriteCommand(byte value1, byte value2) : this(value1, value2, 0)
        { }

        public AreaSpriteCommand(byte value1, byte value2, byte value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public static bool operator ==(AreaSpriteCommand left, AreaSpriteCommand right)
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

        public static bool operator !=(AreaSpriteCommand left, AreaSpriteCommand right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AreaSpriteCommand))
            {
                return false;
            }

            return (AreaSpriteCommand)obj == this;
        }

        public override int GetHashCode()
        {
            return (Value1) | (Value2 << 8) | (Value3 << 0x10);
        }

        public override string ToString()
        {
            return SR.GetString("({0}, {1}): {2}", X.ToString("X"), Y.ToString("X"), Code);
        }
    }
}
