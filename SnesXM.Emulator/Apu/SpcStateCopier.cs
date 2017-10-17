using System;
using Helper;

namespace SnesXM.Emulator.Apu
{
    internal class SpcStateCopier
    {
        private Pointer<byte> Data
        {
            get;
            set;
        }
        DspCopyFunction Function
        {
            get;
            set;
        }

        public SpcStateCopier(Pointer<byte> data, DspCopyFunction function)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Function = function ?? throw new ArgumentNullException(nameof(function));
        }

        public void CopyArray(Array array)
        {
            Function(Data, array);
        }

        public bool CopyBool(bool value)
        {
            var array = BitConverter.GetBytes(value);
            Function(Data, array);
            return BitConverter.ToBoolean(array, 0);
        }

        public byte CopyByte(int value)
        {
            var array = BitConverter.GetBytes((byte)value);
            Function(Data, array);
            return array[0];
        }

        public sbyte CopySbyte(int value)
        {
            var array = BitConverter.GetBytes((sbyte)value);
            Function(Data, array);
            return (sbyte)array[0];
        }

        public char CopyChar(char value)
        {
            var array = BitConverter.GetBytes(value);
            Function(Data, array);
            return BitConverter.ToChar(array, 0);
        }

        public short CopyInt16(int value)
        {
            var array = BitConverter.GetBytes((short)value);
            Function(Data, array);
            return BitConverter.ToInt16(array, 0);
        }

        public ushort CopyUInt16(int value)
        {
            var array = BitConverter.GetBytes((ushort)value);
            Function(Data, array);
            return BitConverter.ToUInt16(array, 0);
        }

        public int CopyInt32(int value)
        {
            var array = BitConverter.GetBytes(value);
            Function(Data, array);
            return BitConverter.ToInt32(array, 0);
        }

        public uint CopyUInt32(long value)
        {
            var array = BitConverter.GetBytes((uint)value);
            Function(Data, array);
            return BitConverter.ToUInt32(array, 0);
        }

        public long CopyInt64(long value)
        {
            var array = BitConverter.GetBytes(value);
            Function(Data, array);
            return BitConverter.ToInt64(array, 0);
        }

        public ulong CopyUInt64(ulong value)
        {
            var array = BitConverter.GetBytes(value);
            Function(Data, array);
            return BitConverter.ToUInt64(array, 0);
        }

        public float CopySingle(float value)
        {
            var array = BitConverter.GetBytes(value);
            Function(Data, array);
            return BitConverter.ToSingle(array, 0);
        }

        public double CopyDouble(double value)
        {
            var array = BitConverter.GetBytes(value);
            Function(Data, array);
            return BitConverter.ToDouble(array, 0);
        }

        public string CopyString(string value)
        {
            var array = value.ToCharArray();
            Function(Data, array);
            return new string(array);
        }

        public void Skip(int count)
        {
            if (count > 0)
            {
                Function(Data, new byte[count]);
            }
        }

        public void Extra()
        {
            byte n = 0;
            n = CopyByte(n);
            Skip(n);
        }
    }
}
