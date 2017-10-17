using System;
using Helper;

namespace SnesXM.Emulator
{
    public class C4Emulator
    {
        private IEmulator Emulator { get; set; }
        private IMemoryMap MemoryMap => Emulator.MemoryMap;
        private Pointer<byte> Ram => MemoryMap.C4Ram;
        private Int16ByteIndexer Ram16
        {
            get;
            set;
        }
        private C4 C4;

        public int this[int index]
        {
            get
            {
                index = (ushort)index;

                if (index == 0x7F5E)
                    return 0;

                return Ram[index - 0x6000];
            }
            set
            {
                index = (ushort)index;
                value = (byte)value;

                Ram[index - 0x6000] = (byte)value;

                if (index == 0x7f4f)
                {
                    if (Ram[0x1f4d] == 0x0e && value < 0x40 && (value & 3) == 0)
                    {
                        Ram[0x1f80] = (byte)(value >> 2);
                    }
                    else
                    {
                        switch (value)
                        {
                        case 0x00: // Sprite
                            ProcessSprites();
                            break;

                        case 0x01: // Draw wireframe
                            Ram.Clear(0x300, 16 * 12 * 3 * 4);
                            DrawWireFrame();
                            break;

                        case 0x05: // Propulsion (?)
                            {
                                var tmp = 0x10000;
                                if (GetUInt16(0x1f83) != 0)
                                    tmp = ((tmp / GetUInt16(0x1f83)) * GetUInt16(0x1f81)) >> 8;

                                SetUInt16(0x1f80, tmp);
                                break;
                            }

                        case 0x0d: // Set vector length
                            C4.C41FXVal    = GetInt16(0x1f80);
                            C4.C41FYVal    = GetInt16(0x1f83);
                            C4.C41FDistVal = GetInt16(0x1f86);
                            C4.Op0D();
                            SetInt16(0x1f89, C4.C41FXVal);
                            SetInt16(0x1f8c, C4.C41FYVal);
                            break;

                        case 0x10: // Polar to rectangluar
                            {
                                int r1 = GetUInt16(0x1f83);
                                if ((r1 & 0x8000) != 0)
                                    r1 |= ~0x7fff;
                                else
                                    r1 &= 0x7fff;

                                var tmp = (r1 * CosTable[GetUInt16(0x1f80) & 0x1ff] * 2) >> 0x10;
                                SetUInt24(0x1f86, tmp);
                                tmp = (r1 * SinTable[GetUInt16(0x1f80) & 0x1ff] * 2) >> 0x10;
                                SetUInt24(0x1f89, (tmp - (tmp >> 6)));
                                break;
                            }

                        case 0x13: // Polar to rectangluar
                            {
                                var tmp = (GetInt16(0x1f83) * CosTable[GetUInt16(0x1f80) & 0x1ff] * 2) >> 8;
                                SetUInt24(0x1f86, tmp);
                                tmp = (GetInt16(0x1f83) * SinTable[GetUInt16(0x1f80) & 0x1ff] * 2) >> 8;
                                SetUInt24(0x1f89, tmp);
                                break;
                            }

                        case 0x15: // Pythagorean
                            C4.C41FXVal = GetInt16(0x1f80);
                            C4.C41FYVal = GetInt16(0x1f83);
                            //C4Op15(); // optimized to:
                            C4.C41FDist = (short)Math.Sqrt((double)C4.C41FXVal * C4.C41FXVal + (double)C4.C41FYVal * C4.C41FYVal);
                            SetUInt16(0x1f80, C4.C41FDist);
                            break;

                        case 0x1f: // atan
                            C4.C41FXVal = GetInt16(0x1f80);
                            C4.C41FYVal = GetInt16(0x1f83);
                            C4.Op1F();
                            SetUInt16(0x1f86, C4.C41FAngleRes);
                            break;

                        case 0x22: // Trapezoid
                            {
                                var angle1 = GetUInt16(0x1f8c) & 0x1ff;
                                var angle2 = GetUInt16(0x1f8f) & 0x1ff;


                                var tan1 = (CosTable[angle1] != 0) ? ((SinTable[angle1] << 16) / CosTable[angle1]) : -0x80000000;
                                var tan2 = (CosTable[angle2] != 0) ? ((SinTable[angle2] << 16) / CosTable[angle2]) : -0x80000000;

                                var y = (short)(GetInt16(0x1f83) - GetInt16(0x1f89));
                                short left, right;

                                for (var j = 0; j < 225; j++)
                                {
                                    if (y >= 0)
                                    {
                                        left  = (short)(((tan1 * y) >> 16) - GetInt16(0x1f80) + GetInt16(0x1f86));
                                        right = (short)(((tan2 * y) >> 16) - GetInt16(0x1f80) + GetInt16(0x1f86) + GetInt16(0x1f93));

                                        if (left < 0 && right < 0)
                                        {
                                            left = 1;
                                            right = 0;
                                        }
                                        else
                                        if (left < 0)
                                            left = 0;
                                        else
                                        if (right < 0)
                                            right = 0;

                                        if (left > 255 && right > 255)
                                        {
                                            left = 255;
                                            right = 254;
                                        }
                                        else
                                        if (left > 255)
                                            left = 255;
                                        else
                                        if (right > 255)
                                            right = 255;
                                    }
                                    else
                                    {
                                        left = 1;
                                        right = 0;
                                    }

                                    Ram[j + 0x800] = (byte)left;
                                    Ram[j + 0x900] = (byte)right;

                                    y++;
                                }

                                break;
                            }

                        case 0x25: // Multiply
                            {
                                int foo = GetUInt24(0x1f80);
                                int bar = GetUInt24(0x1f83);
                                foo *= bar;
                                SetUInt24(0x1f80, foo);
                                break;
                            }

                        case 0x2d: // Transform Coords
                            C4.C4WFXVal = GetInt16(0x1f81);
                            C4.C4WFYVal = GetInt16(0x1f84);
                            C4.C4WFZVal = GetInt16(0x1f87);
                            C4.C4WFX2Val = Ram[0x1f89];
                            C4.C4WFY2Val = Ram[0x1f8a];
                            C4.C4WFDist = Ram[0x1f8b];
                            C4.C4WFScale = GetInt16(0x1f90);
                            C4.TransformWireFrame2();
                            SetInt16(0x1f80, C4.C4WFXVal);
                            SetInt16(0x1f83, C4.C4WFYVal);
                            break;

                        case 0x40: // Sum
                            {
                                ushort sum = 0;
                                for (var i = 0; i < 0x800; sum += Ram[i++]) ;
                                SetUInt16(0x1f80, sum);
                                break;
                            }

                        case 0x54: // Square
                            {
                                var a = ((long)GetUInt24(0x1f80) << 40) >> 40;
                                a *= a;
                                SetUInt24(0x1f83, (int)a);
                                SetUInt24(0x1f86, (int)(a >> 24));
                                break;
                            }

                        case 0x5c: // Immediate Reg
                            for (var i = 0; i < 12 * 4; i++)
                                Ram[i] = TestPattern[i];
                            break;

                        case 0x89: // Immediate ROM
                            Ram[0x1f80] = 0x36;
                            Ram[0x1f81] = 0x43;
                            Ram[0x1f82] = 0x05;
                            break;

                        default:
                            break;
                        }
                    }
                }
                else
                if (index == 0x7f47)
                {
                    var dest = Ram + (GetUInt24(0x1f45) & 0x1FFF);
                    var src = GetRomPointer(GetUInt24(0x1F40));
                    var size = GetUInt16(0x1F43);

                    dest.Copy(src, size);
                }
            }
        }

        public C4Emulator(IEmulator emulator)
        {
            Emulator = emulator ?? throw new ArgumentNullException(nameof(emulator));

            C4 = new C4(emulator);
        }

        private void Initialize() => C4.Initialize();

        private void ConvOam()
        {
            var oamOffset = Ram[0x626] << 2;
            var oamPointer = Ram + oamOffset;
            for (var i = 0x1FD; i > oamOffset; i -= 4)
                Ram[i] = 0xE0;

            var globalX = GetUInt16(0x621);
            var globalY = GetUInt16(0x623);
            var oamPointer2 = Ram + 0x200 + (Ram[0x626] >> 2);

            if (Ram[0x620] != 0)
            {
                var spriteCount = (byte)(0x80 - Ram[0x626]);

                var offset = (byte)((Ram[0x626] & 3) * 2);
                var source = Ram + 0x220;

                for (int i = Ram[0x0620]; i > 0 && spriteCount > 0; i--, source += 0x10)
                {
                    var spriteX = (short)(GetUInt16(source, 0) - globalX);
                    var spriteY = (short)(GetUInt16(source, 2) - globalY);

                    var spriteName = source[5];
                    var spriteAttributes = (byte)(source[4] | source[6]);

                    var sprptr = GetRomPointer(GetUInt24(source, 7));

                    if (sprptr[0] != 0)
                    {
                        for (int sprCnt = sprptr++[0]; sprCnt > 0 && spriteCount > 0; sprCnt--, sprptr += 4)
                        {
                            short x = sprptr[1];
                            if ((spriteAttributes & 0x40) != 0)
                                x = (short)(-x - ((sprptr[0] & 0x20) != 0 ? 16 : 8));
                            x += spriteX;

                            if (x < -16 || x > 272)
                                continue;

                            short y = sprptr[2];
                            if ((spriteAttributes & 0x80) != 0)
                                x = (short)(-y - ((sprptr[0] & 0x20) != 0 ? 16 : 8));
                            y += spriteY;

                            if (y < -16 || y > 224)
                                continue;

                            oamPointer[0] = (byte)x;
                            oamPointer[1] = (byte)y;
                            oamPointer[2] = (byte)(spriteName + sprptr[3]);
                            oamPointer[3] = (byte)(spriteAttributes ^ (sprptr[0] & 0xC0));

                            oamPointer2[0] &= (byte)~(3 << offset);
                            if ((x & 0x100) != 0)
                                oamPointer2[0] |= (byte)(1 << offset);
                            if ((sprptr[0] & 0x20) != 0)
                                oamPointer2[0] |= (byte)(2 << offset);

                            oamPointer += 4;
                            spriteCount--;

                            offset += 2;
                            offset &= 6;
                            if (offset == 0)
                                oamPointer2++;
                        }
                    }
                    else if (spriteCount > 0)
                    {
                        oamPointer[0] = (byte)spriteX;
                        oamPointer[1] = (byte)spriteY;
                        oamPointer[2] = spriteName;
                        oamPointer[3] = spriteAttributes;

                        oamPointer2[0] &= (byte)~(3 << offset);
                        if ((spriteX & 0x100) != 0)
                            oamPointer2[0] |= (byte)(3 << offset);
                        else
                            oamPointer2[0] |= (byte)(2 << offset);

                        oamPointer += 4;
                        spriteCount--;

                        offset += 2;
                        offset &= 6;
                        if (offset == 0)
                            oamPointer2++;
                    }
                }
            }
        }

        private void DoScaleRotate(int rowPadding)
        {
            short a, b, c, d;

            int xScale = GetUInt16(0x1F8F);
            if ((xScale & 0x8000) != 0)
                xScale = 0x7FFF;

            int yScale = GetUInt16(0x1F92);
            if ((yScale & 0x8000) != 0)
                yScale = 0x7FFF;

            var angle = GetUInt16(0x1F80) & 0x1FF;

            if (angle == 0)
            {
                a = (short)xScale;
                b = 0;
                c = 0;
                d = (short)yScale;
            }
            else if (angle == 0x80)
            {
                a = 0;
                b = (short)-yScale;
                c = (short)xScale;
                d = 0;
            }
            else if (angle == 0x100)
            {
                a = (short)-xScale;
                b = 0;
                c = 0;
                d = (short)-yScale;
            }
            else if (angle == 0x180)
            {
                a = 0;
                b = (short)yScale;
                c = (short)-xScale;
                d = 0;
            }
            else
            {
                a = (short)(CosTable[angle] * xScale >> 15);
                b = (short)(-SinTable[angle] * yScale >> 15);
                c = (short)(SinTable[angle] * xScale >> 15);
                d = (short)(CosTable[angle] * yScale >> 15);
            }

            var w = (byte)(Ram[0x1F89] & ~7);
            var h = (byte)(Ram[0x1F8C] & ~7);

            Array.Clear(
                Ram.GetArray(),
                Ram.Offset,
                (w + rowPadding / 4) * h / 2
                );

            int cX = (short)GetUInt16(0x1F83);
            int cy = (short)GetUInt16(0x1F86);

            var lineX = (cX << 12) - cX * a - cX * b;
            var lineY = (cy << 12) - cy * c - cy * d;

            var destIndex = 0;
            byte bit = 0x80;

            for (var j = 0; j < h; j++)
            {
                var x = lineX;
                var y = lineY;

                for (var i = 0; i < w; i++)
                {
                    byte value = 0;

                    if ((x >> 12) < w && (y >> 12) < h)
                    {
                        var address = (y >> 12) * w + (x >> 12);
                        value = Ram[0x600 + address];
                        if ((address & 1) != 0)
                            value >>= 4;
                    }

                    if ((value & 1) != 0)
                        Ram[destIndex + 0x00] |= bit;
                    if ((value & 2) != 0)
                        Ram[destIndex + 0x01] |= bit;
                    if ((value & 4) != 0)
                        Ram[destIndex + 0x10] |= bit;
                    if ((value & 8) != 0)
                        Ram[destIndex + 0x11] |= bit;

                    bit >>= 1;
                    if (bit == 0)
                    {
                        bit = 0x80;
                        destIndex += 0x20;
                    }

                    x += a;
                    y += c;
                }

                destIndex += 2 + rowPadding;
                if ((destIndex & 0x10) != 0)
                    destIndex &= ~0x10;
                else
                    destIndex -= w * 4 + rowPadding;

                lineX += b;
                lineY += d;
            }
        }

        private void DrawLine(int x1, int y1, int z1, int x2, int y2, int z2, byte color)
        {
            C4.C4WFXVal = (short)x1;
            C4.C4WFYVal = (short)y1;
            C4.C4WFZVal = (short)z1;
            C4.C4WFScale = Ram[0x1F90];
            C4.C4WFX2Val = Ram[0x1F86];
            C4.C4WFY2Val = Ram[0x1F87];
            C4.C4WFDist = Ram[0x1F88];
            C4.TransformWireFrame();
            x1 = (C4.C4WFXVal + 0x30) << 8;
            y1 = (C4.C4WFYVal + 0x30) << 8;

            C4.C4WFXVal = (short)x2;
            C4.C4WFYVal = (short)y2;
            C4.C4WFZVal = (short)z2;
            C4.TransformWireFrame2();
            x2 = (C4.C4WFXVal + 0x30) << 8;
            y2 = (C4.C4WFYVal + 0x30) << 8;

            C4.C4WFXVal = (short)(x1 >> 8);
            C4.C4WFYVal = (short)(y1 >> 8);
            C4.C4WFX2Val = (short)(x2 >> 8);
            C4.C4WFY2Val = (short)(y2 >> 8);
            C4.CalculateWireFrame();
            x2 = C4.C4WFXVal;
            y2 = C4.C4WFYVal;

            for (var i = C4.C4WFDist != 0 ? C4.C4WFDist : 1; i > 0; i--)
            {
                if (x1 > 0xFF && y1 > 0xFF && x1 < 0x6000 && y1 < 0x6000)
                {
                    var address = (ushort)(
                        (((y1 >> 8) >> 3) << 8) -
                        (((y1 >> 8) >> 3) << 6) +
                        (((x1 >> 8) >> 3) << 4) +
                        ((y1 >> 8) & 7) * 2
                        );
                    var bit = (byte)(0x80 >> ((x1 >> 8) & 7));

                    Ram[address + 0x300] &= (byte)~bit;
                    Ram[address + 0x301] &= (byte)~bit;
                    if ((color & 1) != 0)
                        Ram[address + 0x300] |= bit;
                    if ((color & 2) != 0)
                        Ram[address + 0x301] |= bit;
                }

                x1 += x2;
                y1 += y2;
            }
        }

        private void DrawWireFrame()
        {
            var line = GetRomPointer(GetUInt24(0x1F80));

            Pointer<byte> point1, point2;

            for (int i = Ram[0x0295]; i > 0; i--, line += 5)
            {
                if (line[0] == 0xFF && line[1] == 0xFF)
                {
                    var tmp = line - 5;
                    while (tmp[2] == 0xFF && tmp[3] == 0xFF)
                        tmp -= 5;
                    point1 = GetRomPointer((Ram[0x1F82] << 16) | (tmp[2] << 8) | tmp[3]);
                }
                else
                    point1 = GetRomPointer((Ram[0x1F82] << 16) | (line[0] << 8) | line[1]);

                point2 = GetRomPointer((Ram[0x1F82] << 16) | (line[2] << 8) | line[3]);

                var X1 = (short)((point1[0] << 8) | point1[1]);
                var Y1 = (short)((point1[2] << 8) | point1[3]);
                var Z1 = (short)((point1[4] << 8) | point1[5]);
                var X2 = (short)((point2[0] << 8) | point2[1]);
                var Y2 = (short)((point2[2] << 8) | point2[3]);
                var Z2 = (short)((point2[4] << 8) | point2[5]);

                var color = line[4];

                DrawLine(X1, Y1, Z1, X2, Y2, Z2, color);
            }
        }

        private void TransformLines()
        {
            C4.C4WFX2Val = Ram[0x1f83];
            C4.C4WFY2Val = Ram[0x1f86];
            C4.C4WFDist  = Ram[0x1f89];
            C4.C4WFScale = Ram[0x1f8c];

            // Transform vertices
            var ptr = Ram.Clone();

            for (int i = GetUInt16(0x1f80); i > 0; i--, ptr += 0x10)
            {
                C4.C4WFXVal = GetInt16(ptr, 1);
                C4.C4WFYVal = GetInt16(ptr, 5);
                C4.C4WFZVal = GetInt16(ptr, 9);
                C4.TransformWireFrame();

                // Displace
                SetInt16(ptr, 1, C4.C4WFXVal + 0x80);
                SetInt16(ptr, 5, C4.C4WFYVal + 0x50);
            }

            SetInt16(0x600 + 0, 23);
            SetInt16(0x602 + 0, 0x60);
            SetInt16(0x605 + 0, 0x40);
            SetInt16(0x600 + 8, 23);
            SetInt16(0x602 + 8, 0x60);
            SetInt16(0x605 + 8, 0x40);

            ptr = Ram + 0xb02;
            var ptr2 = Ram.Clone();

            for (int i = GetUInt16(0xb00); i > 0; i--, ptr += 2, ptr2 += 8)
            {
                C4.C4WFXVal  = GetInt16((ptr[0] << 4) + 1);
                C4.C4WFYVal  = GetInt16((ptr[0] << 4) + 5);
                C4.C4WFX2Val = GetInt16((ptr[1] << 4) + 1);
                C4.C4WFY2Val = GetInt16((ptr[1] << 4) + 5);
                C4.CalculateWireFrame();

                SetUInt16(ptr2, 0x600, C4.C4WFDist != 0 ? C4.C4WFDist : 1);
                SetUInt16(ptr2, 0x602, C4.C4WFXVal);
                SetUInt16(ptr2, 0x605, C4.C4WFYVal);
            }
        }

        private void BitPlaneWave()
        {
            var dst = Ram.Clone();
            var waveptr = Ram[0x1f83];
            ushort mask1 = 0xc0c0;
            ushort mask2 = 0x3f3f;

            for (var j = 0; j < 0x10; j++)
            {
                do
                {
                    var height = (short)(-((sbyte)Ram[waveptr + 0xb00]) - 16);

                    for (var i = 0; i < 40; i++)
                    {
                        var tmp = GetUInt16(dst, BmpData[i]) & mask2;
                        if (height >= 0)
                        {
                            if (height < 8)
                                tmp |= mask1 & GetUInt16(0xa00 + height * 2);
                            else
                                tmp |= mask1 & 0xff00;
                        }

                        SetUInt16(dst, BmpData[i], tmp);

                        height++;
                    }

                    waveptr++;
                    waveptr &= 0x7F;

                    mask1 = (ushort)((mask1 >> 2) | (mask1 << 6));
                    mask2 = (ushort)((mask2 >> 2) | (mask2 << 6));
                }
                while (mask1 != 0xc0c0);

                dst += 16;

                do
                {
                    var height = (short)(-((sbyte)Ram[waveptr + 0xb00]) - 16);

                    for (int i = 0; i < 40; i++)
                    {
                        var tmp = GetUInt16(dst, BmpData[i]) & mask2;
                        if (height >= 0)
                        {
                            if (height < 8)
                                tmp |= mask1 & GetUInt16(0xa10 + height * 2);
                            else
                                tmp |= mask1 & 0xff00;
                        }

                        SetUInt16(dst, BmpData[i], tmp);

                        height++;
                    }

                    waveptr++;
                    waveptr &= 0x7F;
                    mask1 = (ushort)((mask1 >> 2) | (mask1 << 6));
                    mask2 = (ushort)((mask2 >> 2) | (mask2 << 6));
                }
                while (mask1 != 0xc0c0);

                dst += 16;
            }
        }

        private void SpriteDisintegrate()
        {

            var width = Ram[0x1f89];
            var height = Ram[0x1f8c];
            var Cx = GetInt16(0x1f80);
            var Cy = GetInt16(0x1f83);
            var scaleX = GetInt16(0x1f86);
            var scaleY = GetInt16(0x1f8f);
            var StartX = -Cx * scaleX + (Cx << 8);
            var StartY = -Cy * scaleY + (Cy << 8);

            var src = 0x600;

            Ram.Clear(width * height / 2);

            for (int y = StartY, i = 0; i < height; i++, y += scaleY)
            {
                for (int x = StartX, j = 0; j < width; j++, x += scaleX)
                {
                    if ((x >> 8) < width && (y >> 8) < height && (y >> 8) * width + (x >> 8) < 0x2000)
                    {
                        var pixel = (j & 1) != 0 ? (Ram[src] >> 4) : Ram[src];
                        var idx = (y >> 11) * width * 4 + (x >> 11) * 32 + ((y >> 8) & 7) * 2;
                        var mask = (byte)(0x80 >> ((x >> 8) & 7));

                        if ((pixel & 1) != 0)
                            Ram[idx] |= mask;
                        if ((pixel & 2) != 0)
                            Ram[idx + 1] |= mask;
                        if ((pixel & 4) != 0)
                            Ram[idx + 16] |= mask;
                        if ((pixel & 8) != 0)
                            Ram[idx + 17] |= mask;
                    }

                    if ((j & 1) != 0)
                        src++;
                }
            }
        }

        private void ProcessSprites()
        {
            switch (Ram[0x1f4d])
            {
            case 0x00: // Build OAM
                ConvOam();
                break;

            case 0x03: // Scale/Rotate
                DoScaleRotate(0);
                break;

            case 0x05: // Transform Lines
                TransformLines();
                break;

            case 0x07: // Scale/Rotate
                DoScaleRotate(64);
                break;

            case 0x08: // Draw wireframe
                DrawWireFrame();
                break;

            case 0x0b: // Disintegrate
                SpriteDisintegrate();
                break;

            case 0x0c: // Wave
                BitPlaneWave();
                break;

            default:
                break;
            }
        }

        private ushort GetUInt16(int index) =>
            GetUInt16(Ram, index);

        private static ushort GetUInt16(Pointer<byte> pointer, int index) =>
            UInt16ByteIndexer.GetUInt16(pointer, index);

        private void SetUInt16(int index, int value) =>
            SetUInt16(Ram, index, value);

        private static void SetUInt16(Pointer<byte> pointer, int index, int value) =>
            UInt16ByteIndexer.SetUInt16(pointer, index, value);

        private short GetInt16(int index) =>
            GetInt16(Ram, index);

        private static short GetInt16(Pointer<byte> pointer, int index) =>
            Int16ByteIndexer.GetInt16(pointer, index);

        private void SetInt16(int index, int value) =>
            SetInt16(Ram, index, value);

        private static void SetInt16(Pointer<byte> pointer, int index, int value) =>
            Int16ByteIndexer.SetInt16(pointer, index, value);

        private UInt24 GetUInt24(int offset)
        {
            return GetUInt24(Ram, offset);
        }

        private static UInt24 GetUInt24(Pointer<byte> pointer, int index) =>
            UInt24ByteIndexer.GetUInt24(pointer, index);

        private void SetUInt24(int index, int value) =>
            SetUInt24(Ram, index, value);

        private static void SetUInt24(Pointer<byte> pointer, int index, int value) =>
            UInt24ByteIndexer.SetUInt24(pointer, index, value);

        private void SetInt24(int index, int value) =>
            SetInt24(Ram, index, value);

        private static void SetInt24(Pointer<byte> pointer, int index, int value) =>
            Int24ByteIndexer.SetInt24(pointer, index, value);

        private Pointer<byte> GetRomPointer(int address)
        {
            return MemoryMap.Rom + ((address & 0xFF0000) >> 1) + (address & 0x7FFF);
        }

        private static readonly ushort[] BmpData =
        {
            0x0000, 0x0002, 0x0004, 0x0006, 0x0008, 0x000A, 0x000C, 0x000E,
            0x0200, 0x0202, 0x0204, 0x0206, 0x0208, 0x020A, 0x020C, 0x020E,
            0x0400, 0x0402, 0x0404, 0x0406, 0x0408, 0x040A, 0x040C, 0x040E,
            0x0600, 0x0602, 0x0604, 0x0606, 0x0608, 0x060A, 0x060C, 0x060E,
            0x0800, 0x0802, 0x0804, 0x0806, 0x0808, 0x080A, 0x080C, 0x080E
        };

        private static readonly short[] SinTable = new short[0x200]
        {
             0,    402,    804,   1206,   1607,   2009,   2410,   2811,
          3211,   3611,   4011,   4409,   4808,   5205,   5602,   5997,
          6392,   6786,   7179,   7571,   7961,   8351,   8739,   9126,
          9512,   9896,  10278,  10659,  11039,  11416,  11793,  12167,
         12539,  12910,  13278,  13645,  14010,  14372,  14732,  15090,
         15446,  15800,  16151,  16499,  16846,  17189,  17530,  17869,
         18204,  18537,  18868,  19195,  19519,  19841,  20159,  20475,
         20787,  21097,  21403,  21706,  22005,  22301,  22594,  22884,
         23170,  23453,  23732,  24007,  24279,  24547,  24812,  25073,
         25330,  25583,  25832,  26077,  26319,  26557,  26790,  27020,
         27245,  27466,  27684,  27897,  28106,  28310,  28511,  28707,
         28898,  29086,  29269,  29447,  29621,  29791,  29956,  30117,
         30273,  30425,  30572,  30714,  30852,  30985,  31114,  31237,
         31357,  31471,  31581,  31685,  31785,  31881,  31971,  32057,
         32138,  32214,  32285,  32351,  32413,  32469,  32521,  32568,
         32610,  32647,  32679,  32706,  32728,  32745,  32758,  32765,
         32767,  32765,  32758,  32745,  32728,  32706,  32679,  32647,
         32610,  32568,  32521,  32469,  32413,  32351,  32285,  32214,
         32138,  32057,  31971,  31881,  31785,  31685,  31581,  31471,
         31357,  31237,  31114,  30985,  30852,  30714,  30572,  30425,
         30273,  30117,  29956,  29791,  29621,  29447,  29269,  29086,
         28898,  28707,  28511,  28310,  28106,  27897,  27684,  27466,
         27245,  27020,  26790,  26557,  26319,  26077,  25832,  25583,
         25330,  25073,  24812,  24547,  24279,  24007,  23732,  23453,
         23170,  22884,  22594,  22301,  22005,  21706,  21403,  21097,
         20787,  20475,  20159,  19841,  19519,  19195,  18868,  18537,
         18204,  17869,  17530,  17189,  16846,  16499,  16151,  15800,
         15446,  15090,  14732,  14372,  14010,  13645,  13278,  12910,
         12539,  12167,  11793,  11416,  11039,  10659,  10278,   9896,
          9512,   9126,   8739,   8351,   7961,   7571,   7179,   6786,
          6392,   5997,   5602,   5205,   4808,   4409,   4011,   3611,
          3211,   2811,   2410,   2009,   1607,   1206,    804,    402,
             0,   -402,   -804,  -1206,  -1607,  -2009,  -2410,  -2811,
         -3211,  -3611,  -4011,  -4409,  -4808,  -5205,  -5602,  -5997,
         -6392,  -6786,  -7179,  -7571,  -7961,  -8351,  -8739,  -9126,
         -9512,  -9896, -10278, -10659, -11039, -11416, -11793, -12167,
        -12539, -12910, -13278, -13645, -14010, -14372, -14732, -15090,
        -15446, -15800, -16151, -16499, -16846, -17189, -17530, -17869,
        -18204, -18537, -18868, -19195, -19519, -19841, -20159, -20475,
        -20787, -21097, -21403, -21706, -22005, -22301, -22594, -22884,
        -23170, -23453, -23732, -24007, -24279, -24547, -24812, -25073,
        -25330, -25583, -25832, -26077, -26319, -26557, -26790, -27020,
        -27245, -27466, -27684, -27897, -28106, -28310, -28511, -28707,
        -28898, -29086, -29269, -29447, -29621, -29791, -29956, -30117,
        -30273, -30425, -30572, -30714, -30852, -30985, -31114, -31237,
        -31357, -31471, -31581, -31685, -31785, -31881, -31971, -32057,
        -32138, -32214, -32285, -32351, -32413, -32469, -32521, -32568,
        -32610, -32647, -32679, -32706, -32728, -32745, -32758, -32765,
        -32767, -32765, -32758, -32745, -32728, -32706, -32679, -32647,
        -32610, -32568, -32521, -32469, -32413, -32351, -32285, -32214,
        -32138, -32057, -31971, -31881, -31785, -31685, -31581, -31471,
        -31357, -31237, -31114, -30985, -30852, -30714, -30572, -30425,
        -30273, -30117, -29956, -29791, -29621, -29447, -29269, -29086,
        -28898, -28707, -28511, -28310, -28106, -27897, -27684, -27466,
        -27245, -27020, -26790, -26557, -26319, -26077, -25832, -25583,
        -25330, -25073, -24812, -24547, -24279, -24007, -23732, -23453,
        -23170, -22884, -22594, -22301, -22005, -21706, -21403, -21097,
        -20787, -20475, -20159, -19841, -19519, -19195, -18868, -18537,
        -18204, -17869, -17530, -17189, -16846, -16499, -16151, -15800,
        -15446, -15090, -14732, -14372, -14010, -13645, -13278, -12910,
        -12539, -12167, -11793, -11416, -11039, -10659, -10278,  -9896,
         -9512,  -9126,  -8739,  -8351,  -7961,  -7571,  -7179,  -6786,
         -6392,  -5997,  -5602,  -5205,  -4808,  -4409,  -4011,  -3611,
         -3211,  -2811,  -2410,  -2009,  -1607,  -1206,   -804,   -402
        };

        private static readonly short[] CosTable = new short[0x200]
        {
         32767,  32765,  32758,  32745,  32728,  32706,  32679,  32647,
         32610,  32568,  32521,  32469,  32413,  32351,  32285,  32214,
         32138,  32057,  31971,  31881,  31785,  31685,  31581,  31471,
         31357,  31237,  31114,  30985,  30852,  30714,  30572,  30425,
         30273,  30117,  29956,  29791,  29621,  29447,  29269,  29086,
         28898,  28707,  28511,  28310,  28106,  27897,  27684,  27466,
         27245,  27020,  26790,  26557,  26319,  26077,  25832,  25583,
         25330,  25073,  24812,  24547,  24279,  24007,  23732,  23453,
         23170,  22884,  22594,  22301,  22005,  21706,  21403,  21097,
         20787,  20475,  20159,  19841,  19519,  19195,  18868,  18537,
         18204,  17869,  17530,  17189,  16846,  16499,  16151,  15800,
         15446,  15090,  14732,  14372,  14010,  13645,  13278,  12910,
         12539,  12167,  11793,  11416,  11039,  10659,  10278,   9896,
          9512,   9126,   8739,   8351,   7961,   7571,   7179,   6786,
          6392,   5997,   5602,   5205,   4808,   4409,   4011,   3611,
          3211,   2811,   2410,   2009,   1607,   1206,    804,    402,
             0,   -402,   -804,  -1206,  -1607,  -2009,  -2410,  -2811,
         -3211,  -3611,  -4011,  -4409,  -4808,  -5205,  -5602,  -5997,
         -6392,  -6786,  -7179,  -7571,  -7961,  -8351,  -8739,  -9126,
         -9512,  -9896, -10278, -10659, -11039, -11416, -11793, -12167,
        -12539, -12910, -13278, -13645, -14010, -14372, -14732, -15090,
        -15446, -15800, -16151, -16499, -16846, -17189, -17530, -17869,
        -18204, -18537, -18868, -19195, -19519, -19841, -20159, -20475,
        -20787, -21097, -21403, -21706, -22005, -22301, -22594, -22884,
        -23170, -23453, -23732, -24007, -24279, -24547, -24812, -25073,
        -25330, -25583, -25832, -26077, -26319, -26557, -26790, -27020,
        -27245, -27466, -27684, -27897, -28106, -28310, -28511, -28707,
        -28898, -29086, -29269, -29447, -29621, -29791, -29956, -30117,
        -30273, -30425, -30572, -30714, -30852, -30985, -31114, -31237,
        -31357, -31471, -31581, -31685, -31785, -31881, -31971, -32057,
        -32138, -32214, -32285, -32351, -32413, -32469, -32521, -32568,
        -32610, -32647, -32679, -32706, -32728, -32745, -32758, -32765,
        -32767, -32765, -32758, -32745, -32728, -32706, -32679, -32647,
        -32610, -32568, -32521, -32469, -32413, -32351, -32285, -32214,
        -32138, -32057, -31971, -31881, -31785, -31685, -31581, -31471,
        -31357, -31237, -31114, -30985, -30852, -30714, -30572, -30425,
        -30273, -30117, -29956, -29791, -29621, -29447, -29269, -29086,
        -28898, -28707, -28511, -28310, -28106, -27897, -27684, -27466,
        -27245, -27020, -26790, -26557, -26319, -26077, -25832, -25583,
        -25330, -25073, -24812, -24547, -24279, -24007, -23732, -23453,
        -23170, -22884, -22594, -22301, -22005, -21706, -21403, -21097,
        -20787, -20475, -20159, -19841, -19519, -19195, -18868, -18537,
        -18204, -17869, -17530, -17189, -16846, -16499, -16151, -15800,
        -15446, -15090, -14732, -14372, -14010, -13645, -13278, -12910,
        -12539, -12167, -11793, -11416, -11039, -10659, -10278,  -9896,
         -9512,  -9126,  -8739,  -8351,  -7961,  -7571,  -7179,  -6786,
         -6392,  -5997,  -5602,  -5205,  -4808,  -4409,  -4011,  -3611,
         -3211,  -2811,  -2410,  -2009,  -1607,  -1206,   -804,   -402,
             0,    402,    804,   1206,   1607,   2009,   2410,   2811,
          3211,   3611,   4011,   4409,   4808,   5205,   5602,   5997,
          6392,   6786,   7179,   7571,   7961,   8351,   8739,   9126,
          9512,   9896,  10278,  10659,  11039,  11416,  11793,  12167,
         12539,  12910,  13278,  13645,  14010,  14372,  14732,  15090,
         15446,  15800,  16151,  16499,  16846,  17189,  17530,  17869,
         18204,  18537,  18868,  19195,  19519,  19841,  20159,  20475,
         20787,  21097,  21403,  21706,  22005,  22301,  22594,  22884,
         23170,  23453,  23732,  24007,  24279,  24547,  24812,  25073,
         25330,  25583,  25832,  26077,  26319,  26557,  26790,  27020,
         27245,  27466,  27684,  27897,  28106,  28310,  28511,  28707,
         28898,  29086,  29269,  29447,  29621,  29791,  29956,  30117,
         30273,  30425,  30572,  30714,  30852,  30985,  31114,  31237,
         31357,  31471,  31581,  31685,  31785,  31881,  31971,  32057,
         32138,  32214,  32285,  32351,  32413,  32469,  32521,  32568,
         32610,  32647,  32679,  32706,  32728,  32745,  32758,  32765
        };

        private static readonly byte[] TestPattern = new byte[12 * 4]
        {
            0x00, 0x00, 0x00, 0xff,
            0xff, 0xff, 0x00, 0xff,
            0x00, 0x00, 0x00, 0xff,
            0xff, 0xff, 0x00, 0x00,
            0xff, 0xff, 0x00, 0x00,
            0x80, 0xff, 0xff, 0x7f,
            0x00, 0x80, 0x00, 0xff,
            0x7f, 0x00, 0xff, 0x7f,
            0xff, 0x7f, 0xff, 0xff,
            0x00, 0x00, 0x01, 0xff,
            0xff, 0xfe, 0x00, 0x01,
            0x00, 0xff, 0xfe, 0x00
        };
    }
}
