using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Helper.PixelFormats;

namespace Controls
{
    partial class DesignControl
    {
        public static readonly CheckerPattern FallbackCheckerPattern = new CheckerPattern(
            SystemColors.ControlLightLight,
            SystemColors.ControlDarkDark,
            new Size(4, 4));

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckerPattern BackgroundPattern
        {
            get;
            set;
        }

        public void SetTiledBackground(CheckerPattern pattern)
        {
            if (pattern.IsEmpty)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var size = pattern.Size;
            var size2 = size + size;
            BackgroundImage = new Bitmap(
                size2.Width,
                size2.Height,
                PixelFormat.Format32bppRgb);

            var bmp = (Bitmap)BackgroundImage;
            var data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            var color1 = (Color32BppArgb)pattern.Color1;
            var color2 = (Color32BppArgb)pattern.Color2;

            unsafe
            {
                var pixels = (Color32BppArgb*)data.Scan0;

                for (var y = size.Height; --y >= 0;)
                {
                    for (var x = size.Width; --x >= 0;)
                    {
                        pixels[(y * size2.Width) + x] = color1;
                        pixels[(y * size2.Width) + (x + size.Width)] = color2;
                        pixels[((y + size.Height) * size2.Width) + x] = color2;
                        pixels[((y + size.Height) * size2.Width) + (x + size.Width)] = color1;
                    }
                }
            }

            bmp.UnlockBits(data);
            BackgroundImageLayout = ImageLayout.Tile;
        }
    }
}
