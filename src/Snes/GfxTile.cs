// <copyright file="GfxTile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Snes
{
    public struct GfxTile
    {
        public const int DotsPerPlane = 8;
        public const int PlanesPerTile = DotsPerPlane;
        public const int DotsPerTile = DotsPerPlane * PlanesPerTile;
        public const int SizeOf = DotsPerTile * sizeof(byte);

        private unsafe fixed byte Components[DotsPerTile];

        public byte this[int index]
        {
            get
            {
                if ((uint)index >= (uint)DotsPerTile)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                unsafe
                {
                    fixed (byte* ptr = Components)
                    {
                        return ptr[index];
                    }
                }
            }

            set
            {
                if ((uint)index >= (uint)DotsPerTile)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                unsafe
                {
                    fixed (byte* ptr = Components)
                    {
                        ptr[index] = value;
                    }
                }
            }
        }

        public GfxTile(byte[] data, int index, GraphicsFormat format)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (data.Length - index < BytesPerTile(format))
            {
                throw new ArgumentException();
            }

            unsafe
            {
                fixed (byte* src = data)
                fixed (byte* dst = Components)
                {
                    TileFromData(src, dst, format);
                }
            }
        }

        public byte[] ToFormattedData(GraphicsFormat format)
        {
            var data = new byte[BytesPerTile(format)];

            unsafe
            {
                fixed (byte* src = Components)
                fixed (byte* dst = data)
                {
                    TileToData(src, dst, format);
                }
            }

            return data;
        }

        public GfxTile FlipX()
        {
            var result = new GfxTile();

            unsafe
            {
                fixed (byte* src = Components)
                {
                    var dst = result.Components;

                    for (var y = PlanesPerTile; --y >= 0;)
                    {
                        var srcRow = src + (y * DotsPerPlane);
                        var dstRow = dst + (y * DotsPerPlane);

                        var i = 0;
                        for (var x = DotsPerPlane / 2; --x >= 0; i++)
                        {
                            dstRow[i] = srcRow[x];
                            dstRow[x] = srcRow[i];
                        }
                    }
                }
            }

            return result;
        }

        public GfxTile FlipY()
        {
            var result = new GfxTile();

            unsafe
            {
                fixed (byte* src = Components)
                {
                    var dst = result.Components;

                    for (var x = DotsPerPlane; --x >= 0;)
                    {
                        var srcPlane = src + x;
                        var dstPlane = dst + x;

                        var i = 0;
                        for (var j = DotsPerTile / 2; (j -= DotsPerPlane) >= 0; i += DotsPerPlane)
                        {
                            dstPlane[i] = srcPlane[j];
                            dstPlane[j] = srcPlane[i];
                        }
                    }
                }
            }

            return result;
        }

        public GfxTile Rotate90()
        {
            var result = new GfxTile();

            unsafe
            {
                fixed (byte* src = Components)
                {
                    var dst = result.Components;

                    var i = 0;
                    var j = DotsPerTile;
                    var k = PlanesPerTile;

                    for (var y = 0; y < (PlanesPerTile / 2); y++, i += DotsPerPlane)
                    {
                        k--;
                        j -= DotsPerPlane;

                        var n = 0;
                        var m = DotsPerTile;
                        var o = DotsPerPlane;

                        for (var x = 0; x < (DotsPerPlane / 2); x++, n += DotsPerPlane)
                        {
                            o--;
                            m -= DotsPerPlane;

                            dst[i + x] = src[m + y];
                            dst[m + y] = src[j + o];
                            dst[j + o] = src[n + k];
                            dst[n + k] = src[i + x];
                        }
                    }
                }
            }

            return result;
        }

        public GfxTile Rotate180()
        {
            var result = new GfxTile();

            unsafe
            {
                fixed (byte* src = Components)
                {
                    var dst = result.Components;

                    var i = 0;
                    var j = DotsPerTile;

                    for (var y = 0; y < (PlanesPerTile / 2); y++, i += DotsPerPlane)
                    {
                        j -= DotsPerPlane;

                        var n = 0;
                        var o = DotsPerPlane;

                        for (var x = 0; x < (DotsPerPlane / 2); x++, n += DotsPerPlane)
                        {
                            o--;

                            dst[j + o] = src[i + x];
                            dst[i + x] = src[j + o];
                        }
                    }
                }
            }

            return result;
        }

        public GfxTile Rotate270()
        {
            var result = new GfxTile();

            unsafe
            {
                fixed (byte* src = Components)
                {
                    var dst = result.Components;

                    var i = 0;
                    var j = DotsPerTile;
                    var k = PlanesPerTile;

                    for (var y = 0; y < (PlanesPerTile / 2); y++, i += DotsPerPlane)
                    {
                        k--;
                        j -= DotsPerPlane;

                        var n = 0;
                        var m = DotsPerTile;
                        var o = DotsPerPlane;

                        for (var x = 0; x < (DotsPerPlane / 2); x++, n += DotsPerPlane)
                        {
                            o--;
                            m -= DotsPerPlane;

                            dst[m + y] = src[i + x];
                            dst[j + o] = src[m + y];
                            dst[n + k] = src[j + o];
                            dst[i + x] = src[n + k];
                        }
                    }
                }
            }

            return result;
        }

        public GfxTile ReplaceColor(byte original, byte replacement)
        {
            var result = new GfxTile();

            unsafe
            {
                fixed (byte* src = Components)
                {
                    var dst = result.Components;

                    for (var i = DotsPerTile; --i >= 0;)
                    {
                        var value = src[i];

                        if (value == original)
                        {
                            dst[i] = replacement;
                        }
                        else
                        {
                            dst[i] = value;
                        }
                    }
                }
            }

            return result;
        }

        public GfxTile SwapColors(byte color1, byte color2)
        {
            var result = new GfxTile();

            unsafe
            {
                fixed (byte* src = Components)
                {
                    var dst = result.Components;

                    for (var i = DotsPerTile; --i >= 0;)
                    {
                        var value = src[i];

                        if (value == color1)
                        {
                            dst[i] = color2;
                        }
                        else if (value == color2)
                        {
                            dst[i] = color1;
                        }
                        else
                        {
                            dst[i] = value;
                        }
                    }
                }
            }

            return result;
        }

        public GfxTile RotateColors(byte first, byte last, byte shift)
        {
            var result = new GfxTile();
            var length = (byte)(last - first + 1);

            unsafe
            {
                fixed (byte* src = Components)
                {
                    var dst = result.Components;

                    for (var i = DotsPerTile; --i >= 0;)
                    {
                        var value = src[i];

                        if (value >= first && value <= last)
                        {
                            value -= first;
                            value += shift;
                            if (length != 0)
                            {
                                value %= length;
                            }

                            value += first;
                        }

                        dst[i] = value;
                    }
                }
            }

            return result;
        }

        public static int BitsPerPixel(GraphicsFormat format)
        {
            var bpp = (int)format & 0x0F;
            if (bpp == 0)
            {
                throw new InvalidEnumArgumentException(
                    nameof(format),
                    (int)format,
                    typeof(GraphicsFormat));
            }

            return bpp;
        }

        public static int BytesPerPlane(GraphicsFormat format)
        {
            return BitsPerPixel(format);
        }

        public static int ColorsPerPixel(GraphicsFormat format)
        {
            return 1 << BitsPerPixel(format);
        }

        public static int BytesPerTile(GraphicsFormat format)
        {
            return BytesPerPlane(format) * PlanesPerTile;
        }

        private static unsafe void TileFromData(byte* src, byte* dst, GraphicsFormat format)
        {
            if (TileFromDataDictionary.TryGetValue(format, out var getTileData))
            {
                throw new InvalidEnumArgumentException(
                    nameof(format),
                    (int)format,
                    typeof(GraphicsFormat));
            }

            getTileData(src, dst);
        }

        private static unsafe void TileToData(byte* src, byte* dst, GraphicsFormat format)
        {
            if (TileToDataDictionary.TryGetValue(format, out var getFormatData))
            {
                throw new InvalidEnumArgumentException(
                    nameof(format),
                    (int)format,
                    typeof(GraphicsFormat));
            }

            getFormatData(src, dst);
        }

        private unsafe delegate void DataConverter(byte* src, byte* dst);

        private static unsafe readonly IReadOnlyDictionary<GraphicsFormat, DataConverter> TileFromDataDictionary = new Dictionary<GraphicsFormat, DataConverter>()
        {
            { GraphicsFormat.Format1Bpp8x8, TileFromData1Bpp },
            { GraphicsFormat.Format2BppNes, TileFromData2BppNes },
            { GraphicsFormat.Format2BppGb, TileFromData2BppGb },
            { GraphicsFormat.Format2BppNgp, TileFromData2BppNgp },
            { GraphicsFormat.Format2BppVb, TileFromData2BppVb },
            { GraphicsFormat.Format3BppSnes, TileFromData3BppSnes },
            { GraphicsFormat.Format3Bpp8x8, TileFromData3Bpp8x8 },
            { GraphicsFormat.Format4BppSnes, TileFromData4BppSnes },
            { GraphicsFormat.Format4BppGba, TileFromData4BppGba },
            { GraphicsFormat.Format4BppSms, TileFromData4BppSms },
            { GraphicsFormat.Format4BppMsx2, TileFromData4BppMsx2 },
            { GraphicsFormat.Format4Bpp8x8, TileFromData4Bpp8x8 },
            { GraphicsFormat.Format8BppSnes, TileFromData8BppSnes },
            { GraphicsFormat.Format8BppMode7, TileFromData8BppMode7 }
        };

        private static unsafe readonly IReadOnlyDictionary<GraphicsFormat, DataConverter> TileToDataDictionary = new Dictionary<GraphicsFormat, DataConverter>()
        {
            { GraphicsFormat.Format1Bpp8x8, TileToData1Bpp },
            { GraphicsFormat.Format2BppNes, TileToData2BppNes },
            { GraphicsFormat.Format2BppGb, TileToData2BppGb },
            { GraphicsFormat.Format2BppNgp, TileToData2BppNgp },
            { GraphicsFormat.Format2BppVb, TileToData2BppVb },
            { GraphicsFormat.Format3BppSnes, TileToData3BppSnes },
            { GraphicsFormat.Format3Bpp8x8, TileToData3Bpp8x8 },
            { GraphicsFormat.Format4BppSnes, TileToData4BppSnes },
            { GraphicsFormat.Format4BppGba, TileToData4BppGba },
            { GraphicsFormat.Format4BppSms, TileToData4BppSms },
            { GraphicsFormat.Format4BppMsx2, TileToData4BppMsx2 },
            { GraphicsFormat.Format4Bpp8x8, TileToData4Bpp8x8 },
            { GraphicsFormat.Format8BppSnes, TileToData8BppSnes },
            { GraphicsFormat.Format8BppMode7, TileToData8BppMode7 }
        };

        private static unsafe void TileFromData1Bpp(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val = *src;
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)((val >> x) & 1);
                }
            }
        }

        private static unsafe void TileToData1Bpp(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst++)
            {
                var val = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    if (*src != 0)
                    {
                        val |= 1 << x;
                    }
                }

                *dst = (byte)val;
            }
        }

        private static unsafe void TileFromData2BppNes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0];
                var val2 = src[PlanesPerTile];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        ((val2 >> x) & 1) << 1);
                }
            }
        }

        private static unsafe void TileToData2BppNes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst++)
            {
                var val1 = 0;
                var val2 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }
                }

                dst[0] = (byte)val1;
                dst[PlanesPerTile] = (byte)val2;
            }
        }

        private static unsafe void TileFromData2BppGb(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0];
                var val2 = src[1];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        ((val2 >> x) & 1) << 1);
                }
            }
        }

        private static unsafe void TileToData2BppGb(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += 2)
            {
                var val1 = 0;
                var val2 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }
                }

                dst[0] = (byte)val1;
                dst[1] = (byte)val2;
            }
        }

        private static unsafe void TileFromData2BppNgp(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += sizeof(ushort))
            {
                var val = *(ushort*)src;
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)((val >> (x << 1)) & 3);
                }
            }
        }

        private static unsafe void TileToData2BppNgp(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += sizeof(ushort))
            {
                var val = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    val |= (*src & 3) << (x << 1);
                }

                *(ushort*)dst = (ushort)val;
            }
        }

        private static unsafe void TileFromData2BppVb(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += DotsPerPlane, src += sizeof(ushort))
            {
                var val = *(ushort*)src;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    dst[x] = (byte)((val >> (x << 1)) & 3);
                }
            }
        }

        private static unsafe void TileToData2BppVb(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += sizeof(ushort))
            {
                var val = 0;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    val |= (src[x] & 3) << (x << 1);
                }

                *(ushort*)dst = (ushort)val;
            }
        }

        private static unsafe void TileFromData3BppSnes(byte* src, byte* dst)
        {
            for (var y = 0; y < PlanesPerTile; y++)
            {
                var val1 = src[y << 1];
                var val2 = src[(y << 1) + 1];
                var val3 = src[y + (PlanesPerTile << 1)];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2));
                }
            }
        }

        private static unsafe void TileToData3BppSnes(byte* src, byte* dst)
        {
            for (var y = 0; y < PlanesPerTile; y++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((value & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }
                }

                dst[y << 1] = (byte)val1;
                dst[(y << 1) + 1] = (byte)val2;
                dst[y + (PlanesPerTile << 1)] = (byte)val3;
            }
        }

        private static unsafe void TileFromData3Bpp8x8(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0 * PlanesPerTile];
                var val2 = src[1 * PlanesPerTile];
                var val3 = src[2 * PlanesPerTile];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2));
                }
            }
        }

        private static unsafe void TileToData3Bpp8x8(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((value & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }
                }

                dst[0 * PlanesPerTile] = (byte)val1;
                dst[1 * PlanesPerTile] = (byte)val2;
                dst[2 * PlanesPerTile] = (byte)val3;
            }
        }

        private static unsafe void TileFromData4BppSnes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0];
                var val2 = src[1];
                var val3 = src[0 + (2 * PlanesPerTile)];
                var val4 = src[1 + (2 * PlanesPerTile)];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
                }
            }
        }

        private static unsafe void TileToData4BppSnes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += 2)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((value & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((value & 8) != 0)
                    {
                        val4 |= 1 << x;
                    }
                }

                dst[0] = (byte)val1;
                dst[1] = (byte)val2;
                dst[0 + (2 * PlanesPerTile)] = (byte)val3;
                dst[1 + (2 * PlanesPerTile)] = (byte)val4;
            }
        }

        private static unsafe void TileFromData4BppGba(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += sizeof(uint), dst += PlanesPerTile)
            {
                var val = *(uint*)src;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    dst[x] = (byte)((val >> (x << 2)) & 0x0F);
                }
            }
        }

        private static unsafe void TileToData4BppGba(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += sizeof(uint), src += PlanesPerTile)
            {
                var val = 0u;
                for (var x = DotsPerPlane; --x >= 0;)
                {
                    val |= (uint)((src[x] & 3) << (x << 2));
                }

                *(uint*)dst = val;
            }
        }

        private static unsafe void TileFromData4BppSms(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src += 4)
            {
                var val1 = src[0];
                var val2 = src[1];
                var val3 = src[2];
                var val4 = src[3];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
                }
            }
        }

        private static unsafe void TileToData4BppSms(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += 4)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((value & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((value & 8) != 0)
                    {
                        val4 |= 1 << x;
                    }
                }

                dst[0] = (byte)val1;
                dst[1] = (byte)val2;
                dst[2] = (byte)val3;
                dst[3] = (byte)val4;
            }
        }

        private static unsafe void TileFromData4BppMsx2(byte* src, byte* dst)
        {
            for (var i = 0; i < DotsPerTile; i += 2, src++)
            {
                dst[i] = (byte)((*src >> 4) & 0x0F);
                dst[i + 1] = (byte)(*src & 0x0F);
            }
        }

        private static unsafe void TileToData4BppMsx2(byte* src, byte* dst)
        {
            for (var i = 0; i < DotsPerTile; i += 2, dst++)
            {
                var val1 = src[i] & 0x0F;
                var val2 = src[i + 1] & 0x0F;
                *dst = (byte)((val1 << 4) | val2);
            }
        }

        private static unsafe void TileFromData4Bpp8x8(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0 * PlanesPerTile];
                var val2 = src[1 * PlanesPerTile];
                var val3 = src[2 * PlanesPerTile];
                var val4 = src[3 * PlanesPerTile];
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
                }
            }
        }

        private static unsafe void TileToData4Bpp8x8(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (var x = DotsPerPlane; --x >= 0; src++)
                {
                    var value = src[x];

                    if ((value & 1) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 2) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((value & 4) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((value & 8) != 0)
                    {
                        val4 |= 1 << x;
                    }
                }

                dst[0 * PlanesPerTile] = (byte)val1;
                dst[1 * PlanesPerTile] = (byte)val2;
                dst[2 * PlanesPerTile] = (byte)val3;
                dst[3 * PlanesPerTile] = (byte)val4;
            }
        }

        private static unsafe void TileFromData8BppSnes(byte* src, byte* dst)
        {
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
                for (var x = DotsPerPlane; --x >= 0; dst++)
                {
                    *dst = (byte)(((val1 >> x) & 1) |
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

        private static unsafe void TileToData8BppSnes(byte* src, byte* dst)
        {
            for (var y = PlanesPerTile; --y >= 0; dst += 2)
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
                    var value = src[x];

                    if ((value & 1 << 0) != 0)
                    {
                        val1 |= 1 << x;
                    }

                    if ((value & 1 << 1) != 0)
                    {
                        val2 |= 1 << x;
                    }

                    if ((value & 1 << 2) != 0)
                    {
                        val3 |= 1 << x;
                    }

                    if ((value & 1 << 3) != 0)
                    {
                        val4 |= 1 << x;
                    }

                    if ((value & 1 << 4) != 0)
                    {
                        val5 |= 1 << x;
                    }

                    if ((value & 1 << 5) != 0)
                    {
                        val6 |= 1 << x;
                    }

                    if ((value & 1 << 6) != 0)
                    {
                        val7 |= 1 << x;
                    }

                    if ((value & 1 << 7) != 0)
                    {
                        val8 |= 1 << x;
                    }
                }

                dst[0 + (0 * PlanesPerTile)] = (byte)val1;
                dst[1 + (0 * PlanesPerTile)] = (byte)val2;
                dst[0 + (2 * PlanesPerTile)] = (byte)val3;
                dst[1 + (2 * PlanesPerTile)] = (byte)val4;
                dst[0 + (4 * PlanesPerTile)] = (byte)val5;
                dst[1 + (4 * PlanesPerTile)] = (byte)val6;
                dst[0 + (6 * PlanesPerTile)] = (byte)val7;
                dst[1 + (6 * PlanesPerTile)] = (byte)val8;
            }
        }

        private static unsafe void TileFromData8BppMode7(byte* src, byte* dst)
        {
            Buffer.MemoryCopy(src, dst, DotsPerTile, DotsPerTile);
        }

        private static unsafe void TileToData8BppMode7(byte* src, byte* dst)
        {
            Buffer.MemoryCopy(src, dst, DotsPerTile, DotsPerTile);
        }
    }
}
