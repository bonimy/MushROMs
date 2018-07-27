// <copyright file="GfxTile.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Format = GraphicsFormat;

    public unsafe partial struct GfxTile
    {
        public const int DotsPerPlane = 8;
        public const int PlanesPerTile = DotsPerPlane;
        public const int DotsPerTile = DotsPerPlane * PlanesPerTile;
        public const int SizeOf = DotsPerTile * sizeof(byte);

        private static readonly IReadOnlyDictionary<Format, TileFunc>
            TileFromDataDictionary =
            new ReadOnlyDictionary<Format, TileFunc>(
                new Dictionary<Format, TileFunc>()
                {
                    { Format.Format1Bpp8x8, TileFromData1Bpp },
                    { Format.Format2BppNes, TileFromData2BppNes },
                    { Format.Format2BppGb, TileFromData2BppGb },
                    { Format.Format2BppNgp, TileFromData2BppNgp },
                    { Format.Format2BppVb, TileFromData2BppVb },
                    { Format.Format3BppSnes, TileFromData3BppSnes },
                    { Format.Format3Bpp8x8, TileFromData3Bpp8x8 },
                    { Format.Format4BppSnes, TileFromData4BppSnes },
                    { Format.Format4BppGba, TileFromData4BppGba },
                    { Format.Format4BppSms, TileFromData4BppSms },
                    { Format.Format4BppMsx2, TileFromData4BppMsx2 },
                    { Format.Format4Bpp8x8, TileFromData4Bpp8x8 },
                    { Format.Format8BppSnes, TileFromData8BppSnes },
                    { Format.Format8BppMode7, TileFromData8BppMode7 }
                });

        private static readonly IReadOnlyDictionary<Format, TileFunc>
            TileToDataDictionary =
            new ReadOnlyDictionary<Format, TileFunc>(
                new Dictionary<Format, TileFunc>()
                {
                    { Format.Format1Bpp8x8, TileToData1Bpp },
                    { Format.Format2BppNes, TileToData2BppNes },
                    { Format.Format2BppGb, TileToData2BppGb },
                    { Format.Format2BppNgp, TileToData2BppNgp },
                    { Format.Format2BppVb, TileToData2BppVb },
                    { Format.Format3BppSnes, TileToData3BppSnes },
                    { Format.Format3Bpp8x8, TileToData3Bpp8x8 },
                    { Format.Format4BppSnes, TileToData4BppSnes },
                    { Format.Format4BppGba, TileToData4BppGba },
                    { Format.Format4BppSms, TileToData4BppSms },
                    { Format.Format4BppMsx2, TileToData4BppMsx2 },
                    { Format.Format4Bpp8x8, TileToData4Bpp8x8 },
                    { Format.Format8BppSnes, TileToData8BppSnes },
                    { Format.Format8BppMode7, TileToData8BppMode7 }
                });

        private fixed byte components[DotsPerTile];

        public GfxTile(byte[] data, int index, Format format)
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

            fixed (byte* src = data)
            fixed (byte* dst = components)
            {
                TileFromData(src, dst, format);
            }
        }

        private delegate void TileFunc(
            byte* src,
            byte* dst);

        public byte this[int index]
        {
            get
            {
                if ((uint)index >= DotsPerTile)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index));
                }

                fixed (byte* ptr = components)
                {
                    return ptr[index];
                }
            }

            set
            {
                if ((uint)index >= DotsPerTile)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(index));
                }

                fixed (byte* ptr = components)
                {
                    ptr[index] = value;
                }
            }
        }

        public static int BitsPerPixel(Format format)
        {
            if (!Enum.IsDefined(typeof(Format), (int)format))
            {
                throw new InvalidEnumArgumentException(
                    nameof(format),
                    (int)format,
                    typeof(Format));
            }

            return (int)format & 0x0F;
        }

        public static int BytesPerPlane(Format format)
        {
            return BitsPerPixel(format);
        }

        public static int ColorsPerPixel(Format format)
        {
            return 1 << BitsPerPixel(format);
        }

        public static int BytesPerTile(Format format)
        {
            return BytesPerPlane(format) * PlanesPerTile;
        }

        public byte[] ToFormattedData(Format format)
        {
            var data = new byte[BytesPerTile(format)];

            fixed (byte* src = components)
            fixed (byte* dst = data)
            {
                TileToData(src, dst, format);
            }

            return data;
        }

        public GfxTile FlipX()
        {
            var result = default(GfxTile);

            fixed (byte* src = components)
            {
                var dst = result.components;

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

            return result;
        }

        public GfxTile FlipY()
        {
            var result = default(GfxTile);

            fixed (byte* src = components)
            {
                var dst = result.components;

                for (var x = DotsPerPlane; --x >= 0;)
                {
                    var srcPlane = src + x;
                    var dstPlane = dst + x;

                    const int HalfTileSize = DotsPerTile / 2;

                    for (var i = 0; i < HalfTileSize; i += DotsPerPlane)
                    {
                        var j = HalfTileSize - i - DotsPerPlane;
                        dstPlane[i] = srcPlane[j];
                        dstPlane[j] = srcPlane[i];
                    }
                }
            }

            return result;
        }

        public GfxTile Rotate90()
        {
            var result = default(GfxTile);

            fixed (byte* src = components)
            {
                var dst = result.components;

                var i = 0;
                var j = DotsPerTile;
                var k = PlanesPerTile;

                for (var y = 0; y < PlanesPerTile / 2; y++)
                {
                    k--;
                    j -= DotsPerPlane;

                    var n = 0;
                    var m = DotsPerTile;
                    var o = DotsPerPlane;

                    for (var x = 0; x < DotsPerPlane / 2; x++)
                    {
                        o--;
                        m -= DotsPerPlane;

                        dst[i + x] = src[m + y];
                        dst[m + y] = src[j + o];
                        dst[j + o] = src[n + k];
                        dst[n + k] = src[i + x];

                        n += DotsPerPlane;
                    }

                    i += DotsPerPlane;
                }
            }

            return result;
        }

        public GfxTile Rotate180()
        {
            var result = default(GfxTile);

            fixed (byte* src = components)
            {
                var dst = result.components;

                var i = 0;
                var j = DotsPerTile;

                for (var y = 0; y < PlanesPerTile / 2; y++)
                {
                    j -= DotsPerPlane;

                    var n = 0;
                    var o = DotsPerPlane;

                    for (var x = 0; x < DotsPerPlane / 2; x++)
                    {
                        o--;

                        dst[j + o] = src[i + x];
                        dst[i + x] = src[j + o];

                        n += DotsPerPlane;
                    }

                    i += DotsPerPlane;
                }
            }

            return result;
        }

        public GfxTile Rotate270()
        {
            var result = default(GfxTile);

            fixed (byte* src = components)
            {
                var dst = result.components;

                var i = 0;
                var j = DotsPerTile;
                var k = PlanesPerTile;

                for (var y = 0; y < PlanesPerTile / 2; y++)
                {
                    k--;
                    j -= DotsPerPlane;

                    var n = 0;
                    var m = DotsPerTile;
                    var o = DotsPerPlane;

                    for (var x = 0; x < DotsPerPlane / 2; x++)
                    {
                        o--;
                        m -= DotsPerPlane;

                        dst[m + y] = src[i + x];
                        dst[j + o] = src[m + y];
                        dst[n + k] = src[j + o];
                        dst[i + x] = src[n + k];

                        n += DotsPerPlane;
                    }

                    i += DotsPerPlane;
                }
            }

            return result;
        }

        public GfxTile ReplaceColor(byte original, byte replacement)
        {
            var result = default(GfxTile);

            fixed (byte* src = components)
            {
                var dst = result.components;

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

            return result;
        }

        public GfxTile SwapColors(byte color1, byte color2)
        {
            var result = default(GfxTile);

            fixed (byte* src = components)
            {
                var dst = result.components;

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

            return result;
        }

        public GfxTile RotateColors(byte first, byte last, byte shift)
        {
            var result = default(GfxTile);
            var length = (byte)(last - first + 1);

            fixed (byte* src = components)
            {
                var dst = result.components;

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

            return result;
        }

        private static unsafe void TileFromData(
            byte* src,
            byte* dst,
            Format format)
        {
            if (TryGetValue(out var getTileData))
            {
                throw new InvalidEnumArgumentException(
                    nameof(format),
                    (int)format,
                    typeof(Format));
            }

            getTileData(src, dst);

            bool TryGetValue(out TileFunc value)
            {
                return TileFromDataDictionary.TryGetValue(
                    format,
                    out value);
            }
        }

        private static unsafe void TileToData(
            byte* src,
            byte* dst,
            Format format)
        {
            if (TryGetValue(out var getFormatData))
            {
                throw new InvalidEnumArgumentException(
                    nameof(format),
                    (int)format,
                    typeof(Format));
            }

            getFormatData(src, dst);

            bool TryGetValue(out TileFunc value)
            {
                return TileToDataDictionary.TryGetValue(
                    format,
                    out value);
            }
        }
    }
}
