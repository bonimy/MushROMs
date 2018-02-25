// <copyright file="AreaSpriteCommand.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Smb1
{
    using System;
    using static Helper.SR;

    public struct AreaSpriteCommand : IEquatable<AreaSpriteCommand>
    {
        public AreaSpriteCommand(byte value1, byte value2)
            : this(value1, value2, 0)
        {
        }

        public AreaSpriteCommand(byte value1, byte value2, byte value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

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

        public bool IsExtendedSpriteCommand
        {
            get
            {
                return Y == 0x0E;
            }
        }

        /// <summary>
        /// Gets the size, in bytes, of this <see cref="AreaSpriteCommand"/>.
        /// </summary>
        public int Size
        {
            get
            {
                return IsExtendedSpriteCommand ? 3 : 2;
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
        /// Gets or sets a value indicating whether this <see cref="AreaSpriteCommand"/>
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
        /// Gets or sets a value indicating whether this <see cref="AreaSpriteCommand"/>
        /// will only spawn after the hard world flag has been set.
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

        public static bool operator ==(
            AreaSpriteCommand left,
            AreaSpriteCommand right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            AreaSpriteCommand left,
            AreaSpriteCommand right)
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

        public bool Equals(AreaSpriteCommand other)
        {
            if (IsExtendedSpriteCommand != other.IsExtendedSpriteCommand)
            {
                return false;
            }

            if (Value1 != other.Value1 || Value2 != other.Value2)
            {
                return false;
            }

            if (!IsExtendedSpriteCommand)
            {
                return true;
            }

            return Value3 == other.Value3;
        }

        public override int GetHashCode()
        {
            return Value1 | (Value2 << 8) | (Value3 << 0x10);
        }

        public override string ToString()
        {
            return GetString(
                "({0:X}, {1:X}): {2:X}",
                X,
                Y,
                Code);
        }
    }
}
