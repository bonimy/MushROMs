﻿// <copyright file="Int24.cs>
//     Copyright (c) 2017 Nelson Garcia
// </copyright>

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Helper
{
    [DebuggerDisplay("{Value}")]
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public struct Int24 : IComparable, IComparable<Int24>, IConvertible, IEquatable<Int24>, IFormattable
    {
        public const int SizeOf = 3 * sizeof(byte);

        public static readonly Int24 MaxValue = 0x7FFFFF;
        public static readonly Int24 MinValue = -0x800000;

        [FieldOffset(0)]
        private byte _x0;

        [FieldOffset(1)]
        private byte _x1;

        [FieldOffset(2)]
        private byte _x2;

        private int Value
        {
            get
            {
                var result =
                    (_x0 << 0x00) |
                    (_x1 << 0x08) |
                    (_x2 << 0x10);

                return _x2 < 0x80 ? result : ((-1 & ~0xFFFFFF) | result);
            }
        }

        private Int24(int value)
        {
            _x0 = (byte)(value >> 0x00);
            _x1 = (byte)(value >> 0x08);
            _x2 = (byte)(value >> 0x10);
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            if (value is Int24 m)
                return Value - m.Value;

            throw new ArgumentException();
        }

        public int CompareTo(Int24 value) => this - value;

        public bool Equals(Int24 obj) => this == obj;

        public static implicit operator int(Int24 value) =>
            value.Value;

        public static implicit operator Int24(int value) =>
            new Int24(value);

        public static Int24 operator +(Int24 left, Int24 right) =>
            left.Value + right.Value;

        public static Int24 operator -(Int24 left, Int24 right) =>
            left.Value - right.Value;

        public static Int24 operator ++(Int24 value) =>
            value.Value + 1;

        public static Int24 operator --(Int24 value) =>
            value.Value - 1;

        public static Int24 operator *(Int24 left, Int24 right) =>
            left.Value * right.Value;

        public static Int24 operator /(Int24 left, Int24 right) =>
            left.Value / right.Value;

        public static Int24 operator %(Int24 left, Int24 right) =>
            left.Value % right.Value;

        public static Int24 operator &(Int24 left, Int24 right) =>
            left.Value & right.Value;

        public static Int24 operator |(Int24 left, Int24 right) =>
            left.Value | right.Value;

        public static Int24 operator ^(Int24 left, Int24 right) =>
            left.Value ^ right.Value;

        public static Int24 operator >>(Int24 left, int right) =>
            left.Value >> right;

        public static Int24 operator <<(Int24 left, int right) =>
            left.Value << right;

        public static Int24 operator +(Int24 value) =>
            +value.Value;

        public static Int24 operator -(Int24 value) =>
            -value.Value;

        public static bool operator ==(Int24 left, Int24 right)
        {
            return
                left._x0 == right._x0 &&
                left._x1 == right._x1 &&
                left._x2 == right._x2;
        }

        public static bool operator !=(Int24 left, Int24 right) =>
            !(left == right);

        public static bool operator >=(Int24 left, Int24 right) =>
            left.Value > right.Value;

        public static bool operator <=(Int24 left, Int24 right) =>
            left.Value <= right.Value;

        public static bool operator <(Int24 left, Int24 right) =>
            left.Value < right.Value;

        public static bool operator >(Int24 left, Int24 right) =>
            right.Value > left.Value;

        public override bool Equals(object obj)
        {
            if (obj is Int24 uint24)
                return uint24 == this;

            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() =>
            Value.ToString();

        public string ToString(IFormatProvider provider) =>
            Value.ToString(provider);

        public string ToString(string format) =>
            Value.ToString(format);

        public string ToString(string format, IFormatProvider provider) =>
            Value.ToString(format, provider);

        public static Int24 Parse(string s) =>
            Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);

        public static Int24 Parse(string s, NumberStyles style) =>
            Parse(s, style, NumberFormatInfo.CurrentInfo);

        public static Int24 Parse(string s, IFormatProvider provider) =>
            Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));

        public static Int24 Parse(string s, NumberStyles style, IFormatProvider provider) =>
            Parse(s, style, NumberFormatInfo.GetInstance(provider));

        private static Int24 Parse(string s, NumberStyles style, NumberFormatInfo info)
        {
            uint i = 0;
            try
            {
                i = UInt32.Parse(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException();
            }

            if (i > MaxValue)
                throw new OverflowException();

            return (int)i;
        }

        public static bool TryParse(string s, out Int24 result) =>
            TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);

        public static bool TryParse(string s, NumberStyles style, out Int24 result) =>
            TryParse(s, style, NumberFormatInfo.CurrentInfo, out result);

        public static bool TryParse(string s, IFormatProvider provider, out Int24 result) =>
            TryParse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider), out result);

        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Int24 result) =>
            TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);

        private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out Int24 result)
        {
            result = 0;
            if (!UInt32.TryParse(s, style, info, out var i))
                return false;

            if (i > MaxValue)
                return false;
            result = (int)i;
            return true;
        }

        TypeCode IConvertible.GetTypeCode() => ((IConvertible)Value).GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value);

        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Value);

        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(Value);

        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(Value);

        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(Value);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value);

        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(Value);

        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value);

        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(Value);

        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value);

        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Value);

        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Value);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value);

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Value);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            ((IConvertible)Value).ToType(conversionType, provider);
    }
}
