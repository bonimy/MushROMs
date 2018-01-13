using System;
using Helper;

namespace SnesXM.Emulator
{
    public class C4 : IC4
    {
        private const double Pi = 3.14159265;

        private IEmulator Emulator;
        private IMemoryMap MemoryMap => Emulator.MemoryMap;
        private Pointer<byte> Ram => MemoryMap.C4Ram;

        internal short C4WFXVal;
        internal short C4WFYVal;
        internal short C4WFZVal;
        internal short C4WFX2Val;
        internal short C4WFY2Val;
        internal short C4WFDist;
        internal short C4WFScale;
        internal short C41FXVal;
        internal short C41FYVal;
        internal short C41FAngleRes;
        internal short C41FDist;
        internal short C41FDistVal;

        public C4(IEmulator emulator)
        {
            Emulator = emulator ?? throw new NotImplementedException(nameof(emulator));
        }

        public void Initialize()
        {
            Ram.Clear(0x2000);
        }

        internal void TransformWireFrame()
        {
            var c4x = (double)C4WFXVal;
            var c4y = (double)C4WFYVal;
            var c4z = (double)C4WFZVal - 0x95;

            var tanval = -C4WFX2Val * Pi * 2 / 128;
            var c4y2 = c4y * Math.Cos(tanval) - c4z * Math.Sin(tanval);
            var c4z2 = c4y * Math.Sin(tanval) + c4z * Math.Cos(tanval);

            tanval = -C4WFY2Val * Pi * 2 / 128;
            var c4x2 = c4x * Math.Cos(tanval) + c4z2 * Math.Sin(tanval);
            c4z = c4x * -Math.Sin(tanval) + c4z2 * Math.Cos(tanval);

            tanval = -C4WFDist * Pi * 2 / 128;
            c4x = c4x2 * Math.Cos(tanval) - c4y2 * Math.Sin(tanval);
            c4y = c4x2 * Math.Sin(tanval) + c4y2 * Math.Cos(tanval);

            C4WFXVal = (short)(c4x * C4WFScale / (0x90 * (c4z + 0x95)) * 0x95);
            C4WFYVal = (short)(c4y * C4WFScale / (0x90 * (c4z + 0x95)) * 0x95);
        }

        internal void TransformWireFrame2()
        {
            var c4x = (double)C4WFXVal;
            var c4y = (double)C4WFYVal;
            var c4z = (double)C4WFZVal;

            var tanval = -C4WFX2Val * Pi * 2 / 128;
            var c4y2 = c4y * Math.Cos(tanval) - c4z * Math.Sin(tanval);
            var c4z2 = c4y * Math.Sin(tanval) + c4z * Math.Cos(tanval);

            tanval = -C4WFY2Val * Pi * 2 / 128;
            var c4x2 = c4x * Math.Cos(tanval) + c4z2 * Math.Sin(tanval);
            c4z = c4x * -Math.Sin(tanval) + c4z2 * Math.Cos(tanval);

            tanval = -C4WFDist * Pi * 2 / 128;
            c4x = c4x2 * Math.Cos(tanval) - c4y2 * Math.Sin(tanval);
            c4y = c4x2 * Math.Sin(tanval) + c4y2 * Math.Cos(tanval);

            C4WFXVal = (short)(c4x * C4WFScale / 256.0);
            C4WFYVal = (short)(c4y * C4WFScale / 256.0);
        }

        internal void CalculateWireFrame()
        {
            C4WFXVal = (short)(C4WFX2Val - C4WFXVal);
            C4WFYVal = (short)(C4WFY2Val - C4WFYVal);

            if (Math.Abs(C4WFXVal) > Math.Abs(C4WFYVal))
            {
                C4WFDist = (short)(Math.Abs(C4WFXVal) + 1);
                C4WFYVal = (short)(256 * (double)C4WFYVal / Math.Abs(C4WFXVal));
                if (C4WFXVal < 0)
                    C4WFXVal = -256;
                else
                    C4WFXVal = 256;
            }
            else
            {
                if (C4WFYVal != 0)
                {
                    C4WFDist = (short)(Math.Abs(C4WFYVal) + 1);
                    C4WFXVal = (short)(256 * (double)C4WFXVal / Math.Abs(C4WFYVal));
                    if (C4WFYVal < 0)
                        C4WFYVal = -256;
                    else
                        C4WFYVal = 256;
                }
                else
                    C4WFDist = 0;
            }
        }

        public Pointer<byte> GetBasePointer(int index)
        {
            index = (ushort)index;
            if (index >= 0x7F40 && index <= 0x7F5E)
                return null;
            return new Pointer<byte>(MemoryMap.C4Ram, -0x6000);
        }

        public Pointer<byte> GetMemPointer(int index)
        {
            index = (ushort)index;
            if (index >= 0x7F40 && index <= 0x7F5E)
                return null;
            return new Pointer<byte>(MemoryMap.C4Ram, index - 0x6000);
        }

        internal void Op0D()
        {
            var tanval = Math.Sqrt((double)C41FYVal * C41FYVal + (double)C41FXVal * C41FXVal);
            tanval = C41FDistVal / tanval;
            C41FYVal = (short)(C41FYVal * tanval * 0.99);
            C41FXVal = (short)(C41FXVal * tanval * 0.98);
        }

        internal void Op15()
        {
            var tanval = Math.Sqrt((double)C41FYVal * C41FYVal + (double)C41FXVal * C41FXVal);
            C41FDist = (short)tanval;
        }

        internal void Op1F()
        {
            if (C41FXVal == 0)
            {
                if (C41FYVal > 0)
                    C41FAngleRes = 0x80;
                else
                    C41FAngleRes = 0x180;
            }
            else
            {
                var tanval = (double)C41FYVal / C41FXVal;
                C41FAngleRes = (short)(Math.Atan(tanval) / (Pi * 2) * 512);
                if (C41FXVal < 0)
                    C41FAngleRes += 0x100;
                C41FAngleRes &= 0x1FF;
            }
        }
    }
}
