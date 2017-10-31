using System;
using System.ComponentModel;
using System.Text;
using Helper;

namespace Snes
{
    public unsafe struct GfxTile
    {
        public const int DotsPerPlane = 8;
        public const int PlanesPerTile = DotsPerPlane;
        public const int DotsPerTile = DotsPerPlane * PlanesPerTile;
        public const int Size = DotsPerTile;

        private fixed byte _components[Size];

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index > Size)
                {
                    throw new ArgumentOutOfRangeException(nameof(index),
                        SR.ErrorArrayBounds(nameof(index), index, Size));
                }

                return UnsafeData[index];
            }
            set
            {
                if (index < 0 || index > Size)
                {
                    throw new ArgumentOutOfRangeException(nameof(index),
                        SR.ErrorArrayBounds(nameof(index), index, Size));
                }

                UnsafeData[index] = value;
            }
        }

        internal byte* UnsafeData
        {
            get
            {
                fixed (byte* data = _components)
                    return data;
            }
        }

        public IntPtr Data
        {
            get { return new IntPtr(UnsafeData); }
        }

        public void FlipX()
        {
            for (var y = PlanesPerTile; --y >= 0;)
            {
                var plane = UnsafeData + y * DotsPerPlane;
                var i = 0;
                var j = DotsPerPlane;
                for (var x = DotsPerPlane / 2; --x >= 0; i++)
                {
                    j--;
                    var dummy = plane[i];
                    plane[i] = plane[--j];
                    plane[j] = dummy;
                }
            }
        }

        public void FlipY()
        {
            for (var x = DotsPerPlane; --x >= 0;)
            {
                var plane = UnsafeData + x;
                var i = 0;
                var j = Size;
                for (var y = PlanesPerTile / 2; --y >= 0; i += DotsPerTile)
                {
                    var dummy = plane[i];
                    plane[i] = plane[j -= DotsPerTile];
                    plane[j] = dummy;
                }
            }
        }

        public void Rotate90()
        {
            var data = UnsafeData;
            var i = 0;
            var j = Size;
            var k = PlanesPerTile;
            for (var y = 0; y < PlanesPerTile / 2; y++, i += DotsPerPlane)
            {
                k--;
                j -= DotsPerPlane;

                var n = 0;
                var m = Size;
                var o = DotsPerPlane;
                for (var x = 0; x < DotsPerPlane / 2; x++, n += DotsPerPlane)
                {
                    o--;
                    m -= DotsPerPlane;

                    var dummy = this[i + x];
                    this[i + x] = this[m + y];
                    this[m + y] = this[j + o];
                    this[j + o] = this[n + k];
                    this[n + k] = dummy;
                }
            }
        }

        public void Rotate180()
        {
            var data = UnsafeData;
            var i = 0;
            var j = Size;
            for (var y = 0; y < PlanesPerTile / 2; y++, i += DotsPerPlane)
            {
                j -= DotsPerPlane;

                var n = 0;
                var o = DotsPerPlane;
                for (var x = 0; x < DotsPerPlane; x++, n += DotsPerPlane)
                {
                    o--;

                    var dummy = data[i + x];
                    data[i + x] = data[j + o];
                    data[j + o] = dummy;
                }
            }
        }

        public void Rotate270()
        {
            var data = UnsafeData;
            var i = 0;
            var j = Size;
            var k = PlanesPerTile;
            for (var y = 0; y < PlanesPerTile / 2; y++, i += DotsPerPlane)
            {
                k--;
                j -= DotsPerPlane;

                var n = 0;
                var m = Size;
                var o = DotsPerPlane;
                for (var x = 0; x < DotsPerPlane / 2; x++, n += DotsPerPlane)
                {
                    o--;
                    m -= DotsPerPlane;

                    var dummy = data[i + x];
                    data[i + x] = data[n + k];
                    data[n + k] = data[j + o];
                    data[j + o] = data[m + y];
                    data[m + y] = dummy;
                }
            }
        }

        public void GetTileData(byte[] data, int tile, GraphicsFormat format)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var index = GetStartAddress(tile, format);
            if (!IsValidSize(index, data.Length, format))
            {
                throw new ArgumentOutOfRangeException(nameof(tile));
            }

            fixed (byte* ptr = &data[tile])
                GetTileData(ptr, format);
        }

        public void GetTileData(IntPtr data, int tile, int length, GraphicsFormat format)
        {
            var index = GetStartAddress(tile, format);
            if (!IsValidSize(index, length, format))
            {
                throw new ArgumentOutOfRangeException(nameof(tile));
            }

            GetTileData((byte*)data + tile, format);
        }

        public static bool IsValidSize(int index, int length, GraphicsFormat format)
        {
            return IsValidSize(length - index, format);
        }

        public static bool IsValidSize(int length, GraphicsFormat format)
        {
            switch (format)
            {
            default:
            return GetTileDataSize(format) <= length;
            }
        }

        public static int GetStartAddress(int tile, GraphicsFormat format)
        {
            return tile * GetTileDataSize(format);
        }

        public static int GetBitsPerPixel(GraphicsFormat format)
        {
            var bpp = (int)format & 0x0F;
            if (bpp == 0)
            {
                throw new InvalidEnumArgumentException(nameof(format), (int)format, typeof(GraphicsFormat));
            }

            return bpp;
        }

        public static int GetColorsPerPixel(GraphicsFormat format)
        {
            return 1 << GetBitsPerPixel(format);
        }

        public static int GetTileDataSize(GraphicsFormat format)
        {
            return GetBitsPerPixel(format) * PlanesPerTile;
        }

        internal void GetTileData(byte* data, int tile, GraphicsFormat format)
        {
            GetTileData(data + GetStartAddress(tile, format), format);
        }

        internal void GetTileData(byte* data, GraphicsFormat format)
        {
            switch (format)
            {
            case GraphicsFormat.Format1Bpp8x8:
            GetTileData1Bpp(data);
            return;

            case GraphicsFormat.Format2BppNes:
            GetTileData2BppNes(data);
            return;

            case GraphicsFormat.Format2BppGb:
            GetTileData2BppGb(data);
            return;

            case GraphicsFormat.Format2BppNgp:
            GetTileData2BppNgp(data);
            return;

            case GraphicsFormat.Format2BppVb:
            GetTileData2BppVb(data);
            return;

            case GraphicsFormat.Format3BppSnes:
            GetTileData3BppSnes(data);
            return;

            case GraphicsFormat.Format3Bpp8x8:
            GetTileData3Bpp8x8(data);
            return;

            case GraphicsFormat.Format4BppSnes:
            GetTileData4BppSnes(data);
            return;

            case GraphicsFormat.Format4BppGba:
            GetTileData4BppGba(data);
            return;

            case GraphicsFormat.Format4BppSms:
            GetTileData4BppSms(data);
            return;

            case GraphicsFormat.Format4BppMsx2:
            GetTileData4BppMsx2(data);
            return;

            case GraphicsFormat.Format4Bpp8x8:
            GetTileData4Bpp8x8(data);
            return;

            case GraphicsFormat.Format8BppSnes:
            GetTileData8BppSnes(data);
            return;

            case GraphicsFormat.Format8BppMode7:
            GetTileData8BppMode7(data);
            return;

            default:
            throw new InvalidEnumArgumentException(nameof(format), (int)format, typeof(GraphicsFormat));
            }
        }

        private void GetTileData1Bpp(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val = *src;
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)((val >> x) & 1);
                }
            }
        }

        private void GetFormatData1Bpp(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest++)
            {
                var val = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if (*src != 0)
                    {
                        val |= 1 << x;
                    }
                }

                *dest = (byte)val;
            }
        }

        private void GetTileData2BppNes(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0];
                var val2 = src[PlanesPerTile];
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)(((val1 >> x) & 1) |
                        ((val2 >> x) & 1) << 1);
                }
            }
        }

        private void GetFormatData2BppNes(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest++)
            {
                var val1 = 0;
                var val2 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((src[x] & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }
                }
                dest[0] = (byte)val1;
                dest[PlanesPerTile] = (byte)val2;
            }
        }

        private void GetTileData2BppGb(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0];
                var val2 = src[1];
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)(((val1 >> x) & 1) |
                        ((val2 >> x) & 1) << 1);
                }
            }
        }

        private void GetFormatData2BppGb(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest += 2)
            {
                var val1 = 0;
                var val2 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((src[x] & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }
                }
                dest[0] = (byte)val1;
                dest[1] = (byte)val2;
            }
        }

        private void GetTileData2BppNgp(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src += sizeof(ushort))
            {
                var val = *(ushort*)src;
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)((val >> (x << 1)) & 3);
                }
            }
        }

        private void GetFormatData2BppNgp(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest += sizeof(ushort))
            {
                var val = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    val |= (*src & 3) << (x << 1);
                }

                *(ushort*)dest = (ushort)val;
            }
        }

        private void GetTileData2BppVb(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest += DotsPerPlane, src += sizeof(ushort))
            {
                var val = *(ushort*)src;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    dest[x] = (byte)((val >> (x << 1)) & 3);
                }
            }
        }

        private void GetFormatData2BppVb(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest += sizeof(ushort))
            {
                var val = 0;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    val |= (src[x] & 3) << (x << 1);
                }

                *(ushort*)dest = (ushort)val;
            }
        }

        private void GetTileData3BppSnes(byte* src)
        {
            var dest = UnsafeData;
            for (var y = 0; y < PlanesPerTile; y++)
            {
                var val1 = src[y << 1];
                var val2 = src[(y << 1) + 1];
                var val3 = src[y + (PlanesPerTile << 1)];
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2));
                }
            }
        }

        private void GetFormatData3BppSnes(byte* dest)
        {
            var src = UnsafeData;
            for (var y = 0; y < PlanesPerTile; y++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((src[x] & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((src[x] & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }
                }
                dest[y << 1] = (byte)val1;
                dest[(y << 1) + 1] = (byte)val2;
                dest[y + (PlanesPerTile << 1)] = (byte)val3;
            }
        }

        private void GetTileData3Bpp8x8(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0 * PlanesPerTile];
                var val2 = src[1 * PlanesPerTile];
                var val3 = src[2 * PlanesPerTile];
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2));
                }
            }
        }

        private void GetFormatData3Bpp8x8(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((src[x] & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((src[x] & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }
                }
                dest[0 * PlanesPerTile] = (byte)val1;
                dest[1 * PlanesPerTile] = (byte)val2;
                dest[2 * PlanesPerTile] = (byte)val3;
            }
        }

        private void GetTileData4BppSnes(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0];
                var val2 = src[1];
                var val3 = src[0 + (2 * PlanesPerTile)];
                var val4 = src[1 + (2 * PlanesPerTile)];
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
                }
            }
        }

        private void GetFormatData4BppSnes(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest += 2)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((src[x] & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((src[x] & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((src[x] & 8) != 0)
                    {
                        val4 |= 1 << x;
                    }
                }
                dest[0] = (byte)val1;
                dest[1] = (byte)val2;
                dest[0 + (2 * PlanesPerTile)] = (byte)val3;
                dest[1 + (2 * PlanesPerTile)] = (byte)val4;
            }
        }

        private void GetTileData4BppGba(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src += sizeof(uint), dest += PlanesPerTile)
            {
                var val = *(uint*)src;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    dest[x] = (byte)((val >> (x << 2)) & 0x0F);
                }
            }
        }

        private void GetFormatData4BppGba(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest += sizeof(uint), src += PlanesPerTile)
            {
                var val = 0u;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    val |= (uint)((src[x] & 3) << (x << 2));
                }

                *(uint*)dest = val;
            }
        }

        private void GetTileData4BppSms(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src += 4)
            {
                var val1 = src[0];
                var val2 = src[1];
                var val3 = src[2];
                var val4 = src[3];
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
                }
            }
        }

        private void GetFormatData4BppSms(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest += 4)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((src[x] & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((src[x] & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((src[x] & 8) != 0)
                    {
                        val4 |= 1 << x;
                    }
                }
                dest[0] = (byte)val1;
                dest[1] = (byte)val2;
                dest[2] = (byte)val3;
                dest[3] = (byte)val4;
            }
        }

        private void GetTileData4BppMsx2(byte* src)
        {
            var dest = UnsafeData;
            for (var i = 0; i < Size; i += 2, src++)
            {
                dest[i] = (byte)((*src >> 4) & 0x0F);
                dest[i + 1] = (byte)(*src & 0x0F);
            }
        }

        private void GetFormatData4BppMsx2(byte* dest)
        {
            var src = UnsafeData;
            for (var i = 0; i < Size; i += 2, dest++)
            {
                var val1 = src[i] & 0x0F;
                var val2 = src[i + 1] & 0x0F;
                *dest = (byte)((val1 << 4) | val2);
            }
        }

        private void GetTileData4Bpp8x8(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0 * PlanesPerTile];
                var val2 = src[1 * PlanesPerTile];
                var val3 = src[2 * PlanesPerTile];
                var val4 = src[3 * PlanesPerTile];
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
                }
            }
        }

        private void GetFormatData4Bpp8x8(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((src[x] & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((src[x] & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((src[x] & 8) != 0)
                    {
                        val4 |= 1 << x;
                    }
                }
                dest[0 * PlanesPerTile] = (byte)val1;
                dest[1 * PlanesPerTile] = (byte)val2;
                dest[2 * PlanesPerTile] = (byte)val3;
                dest[3 * PlanesPerTile] = (byte)val4;
            }
        }

        private void GetTileData8BppSnes(byte* src)
        {
            var dest = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0 + (0 * PlanesPerTile)];
                var val2 = src[1 + (0 * PlanesPerTile)];
                var val3 = src[0 + (2 * PlanesPerTile)];
                var val4 = src[1 + (2 * PlanesPerTile)];
                var val5 = src[0 + (4 * PlanesPerTile)];
                var val6 = src[1 + (4 * PlanesPerTile)];
                var val7 = src[0 + (6 * PlanesPerTile)];
                var val8 = src[1 + (6 * PlanesPerTile)];
                for (var x = DotsPerPlane; --x >= 0; dest++)
                {
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val2 >> x) & 1) << 3) |
                        (((val3 >> x) & 1) << 4) |
                        (((val4 >> x) & 1) << 5) |
                        (((val2 >> x) & 1) << 6) |
                        (((val3 >> x) & 1) << 7));
                }
            }
        }

        private void GetFormatData8BppSnes(byte* dest)
        {
            var src = UnsafeData;
            for (var y = PlanesPerTile; --y >= 0; dest += 2)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                var val5 = 0;
                var val6 = 0;
                var val7 = 0;
                var val8 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1 << 0) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((src[x] & 1 << 1) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((src[x] & 1 << 2) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((src[x] & 1 << 3) != 0)
                    {
                        val4 |= 1 << x;
                    }

                    if ((src[x] & 1 << 4) != 0)
                    {
                        val5 |= 1 << x;
                    }

                    if ((src[x] & 1 << 5) != 0)
                    {
                        val6 |= 1 << x;
                    }

                    if ((src[x] & 1 << 6) != 0)
                    {
                        val7 |= 1 << x;
                    }

                    if ((src[x] & 1 << 7) != 0)
                    {
                        val8 |= 1 << x;
                    }
                }
                dest[0 + (0 * PlanesPerTile)] = (byte)val1;
                dest[1 + (0 * PlanesPerTile)] = (byte)val2;
                dest[0 + (2 * PlanesPerTile)] = (byte)val3;
                dest[1 + (2 * PlanesPerTile)] = (byte)val4;
                dest[0 + (4 * PlanesPerTile)] = (byte)val5;
                dest[1 + (4 * PlanesPerTile)] = (byte)val6;
                dest[0 + (6 * PlanesPerTile)] = (byte)val7;
                dest[1 + (6 * PlanesPerTile)] = (byte)val8;
            }
        }

        private void GetTileData8BppMode7(byte* src)
        {
            var dest = UnsafeData;
            for (var i = Size; --i >= 0;)
            {
                dest[i] = src[i];
            }
        }

        private void GetFormatData8BppMode7(byte* dest)
        {
            var src = UnsafeData;
            for (var i = Size; --i >= 0;)
            {
                dest[i] = src[i];
            }
        }

        public static bool operator ==(GfxTile left, GfxTile right)
        {
            for (var i = Size; --i >= 0;)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(GfxTile left, GfxTile right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is GfxTile))
            {
                return false;
            }

            return (GfxTile)obj == this;
        }

        public override int GetHashCode()
        {
            var code = 0;
            for (var i = Size; --i >= 0;)
            {
                code ^= (this[i] << (i & 0x1F));
            }

            return code;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var j = 0; j < PlanesPerTile; j++)
            {
                for (var i = 0; i < DotsPerPlane; i++)
                {
                    sb.Append(this[(j * 8) + i]);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
