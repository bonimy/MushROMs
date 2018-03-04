// <copyright file="PaletteFormatter.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Helper.PixelFormat;

    public abstract class PaletteFormatter
    {
        public static readonly PaletteFormatter Rpf = new RpfFormatter();

        public static readonly PaletteFormatter Tpl = new TplFormatter();

        public static readonly PaletteFormatter Pal = new PalFormatter();

        public abstract Palette CreatePalette(byte[] data);

        public abstract byte[] CreateData(Palette palette);

        private class RpfFormatter : PaletteFormatter
        {
            public override Palette CreatePalette(byte[] data)
            {
                return new Palette(data);
            }

            public override byte[] CreateData(Palette palette)
            {
                return new List<byte>(palette).ToArray();
            }
        }

        private class TplFormatter : PaletteFormatter
        {
            private static readonly string TplHeader = "TPL\x02";

            public override Palette CreatePalette(byte[] data)
            {
                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                if (data.Length < TplHeader.Length)
                {
                    throw new ArgumentException();
                }

                if (!CompareHeader(data))
                {
                    throw new ArgumentException();
                }

                var size = data.Length - TplHeader.Length;
                if ((size % Color15BppBgr.SizeOf) != 0)
                {
                    throw new ArgumentException();
                }

                return new Palette(data, TplHeader.Length, size);
            }

            public override byte[] CreateData(Palette palette)
            {
                if (palette is null)
                {
                    throw new ArgumentNullException(nameof(palette));
                }

                if ((palette.Count % Color15BppBgr.SizeOf) != 0)
                {
                    throw new ArgumentException();
                }

                var result = new List<byte>();
                result.AddRange(Encoding.ASCII.GetBytes(TplHeader));
                result.AddRange(palette);
                return result.ToArray();
            }

            private bool CompareHeader(byte[] data)
            {
                var header = Encoding.ASCII.GetString(data, 0, TplHeader.Length);

                return String.Equals(header, TplHeader);
            }
        }

        private class PalFormatter : PaletteFormatter
        {
            public override Palette CreatePalette(byte[] data)
            {
                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                if ((data.Length % Color24BppRgb.SizeOf) != 0)
                {
                    throw new ArgumentException();
                }

                var size = data.Length / Color24BppRgb.SizeOf;
                var palette = new Palette(size * Color15BppBgr.SizeOf);
                for (var i = size; --i >= 0;)
                {
                    var index3 = i * Color24BppRgb.SizeOf;
                    var r = data[index3 + Color24BppRgb.RedIndex];
                    var g = data[index3 + Color24BppRgb.GreenIndex];
                    var b = data[index3 + Color24BppRgb.BlueIndex];
                    var color24 = new Color24BppRgb(r, g, b);

                    var index2 = i * Color15BppBgr.SizeOf;
                    var color16 = (Color15BppBgr)color24;
                    palette[index2 + 0] = color16.Low;
                    palette[index2 + 1] = color16.High;
                }

                return palette;
            }

            public override byte[] CreateData(Palette palette)
            {
                if (palette is null)
                {
                    throw new ArgumentNullException(nameof(palette));
                }

                if ((palette.Count % Color15BppBgr.SizeOf) != 0)
                {
                    throw new ArgumentException();
                }

                var size = palette.Count / Color24BppRgb.SizeOf;
                var data = new byte[size * Color24BppRgb.SizeOf];
                for (var i = size; --i >= 0;)
                {
                    var index2 = i * Color15BppBgr.SizeOf;
                    var color16 = new Color15BppBgr(
                    palette[index2 + 0],
                    palette[index2 + 1]);

                    var index3 = i * Color24BppRgb.SizeOf;
                    Color24BppRgb color24 = color16;
                    data[index3 + Color24BppRgb.RedIndex] = color24.Red;
                    data[index3 + Color24BppRgb.GreenIndex] = color24.Green;
                    data[index3 + Color24BppRgb.BlueIndex] = color24.Blue;
                }

                return data;
            }
        }
    }
}
