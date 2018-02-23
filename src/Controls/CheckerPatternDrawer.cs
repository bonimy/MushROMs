// <copyright file="CheckerPatternDrawer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Helper.PixelFormat;
using static Helper.ThrowHelper;

namespace Controls
{
    public class CheckerPatternDrawer : Component
    {
        private int _width;
        private int _height;

        [DefaultValue(typeof(Color), "Black")]
        public Color Color1
        {
            get;
            set;
        }

        [DefaultValue(typeof(Color), "White")]
        public Color Color2
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (value <= 0)
                {
                    throw ValueNotGreaterThan(
                        nameof(value),
                        value);
                }

                _width = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (value <= 0)
                {
                    throw ValueNotGreaterThan(
                        nameof(value),
                        value);
                }

                _height = value;
            }
        }

        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }

            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public CheckerPatternDrawer()
            : this(null)
        {
        }

        public CheckerPatternDrawer(IContainer container)
            : this(container, Color.Black, Color.White, 4, 4)
        {
        }

        public CheckerPatternDrawer(
            Color color1,
            Color color2,
            Size size)
            : this(color1, color1, size.Width, size.Height)
        {
        }

        public CheckerPatternDrawer(
            Color color1,
            Color color2,
            int width,
            int height)
            : this(null, color1, color2, width, height)
        {
        }

        public CheckerPatternDrawer(
            IContainer container,
            Color color1,
            Color color2,
            int width,
            int height)
        {
            if (width <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(width),
                    width);
            }

            if (height <= 0)
            {
                throw ValueNotLessThan(
                    nameof(height),
                    height);
            }

            Color1 = color1;
            Color2 = color2;
            Size = new Size(width, height);

            container?.Add(this);
        }

        public Image CreateCheckerImage()
        {
            var width2 = Width * 2;
            var height2 = Height * 2;
            var result = new Bitmap(
                width2,
                height2,
                PixelFormat.Format32bppArgb);

            var data = result.LockBits(
                new Rectangle(Point.Empty, result.Size),
                ImageLockMode.ReadWrite,
                result.PixelFormat);

            var color1 = (Color32BppArgb)Color1;
            var color2 = (Color32BppArgb)Color2;

            unsafe
            {
                var pixels = (Color32BppArgb*)data.Scan0;

                for (var y = Height; --y >= 0;)
                {
                    var y1 = y * width2;
                    var y2 = (y + Height) * width2;

                    for (var x = Width; --x >= 0;)
                    {
                        var x1 = x;
                        var x2 = x + Width;

                        pixels[y1 + x1] = color1;
                        pixels[y1 + x2] = color2;
                        pixels[y2 + x1] = color2;
                        pixels[y1 + x1] = color1;
                    }
                }
            }

            result.UnlockBits(data);
            return result;
        }
    }
}
