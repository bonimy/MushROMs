// <copyright file="PaletteFormEditor.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Helper.PixelFormat;
using MushROMs;
using Snes;

namespace Controls.Editors
{
    public class PaletteFormEditor : EditorFormHelper
    {
        public event EventHandler StartAddressChanged;

        public PaletteForm PaletteForm
        {
            get;
        }

        public PaletteEditor PaletteEditor
        {
            get;
        }

        private int _startAddress;

        public int StartAddress
        {
            get
            {
                return _startAddress;
            }

            set
            {
                if (StartAddress == value)
                {
                    return;
                }

                _startAddress = value;
                OnStartAddressChanged(EventArgs.Empty);
            }
        }

        private int Offset
        {
            get
            {
                return StartAddress % Color15BppBgr.SizeOf;
            }
        }

        private TileMapControl1D TileMapControl
        {
            get
            {
                return PaletteForm.TileMapControl as TileMapControl1D;
            }
        }

        private int GridLength
        {
            get
            {
                return TileMapControl.GridSize;
            }

            set
            {
                TileMapControl.GridSize = value;
            }
        }

        public PaletteFormEditor(
            PaletteForm paletteForm,
            PaletteEditor paletteEditor) :
            base(paletteForm, paletteEditor)
        {
            PaletteForm = paletteForm ??
                throw new ArgumentNullException(nameof(paletteForm));

            PaletteEditor = paletteEditor ??
                throw new ArgumentNullException(nameof(paletteEditor));

            PaletteForm.Text = Path.GetFileName(PaletteEditor.Path);
            PaletteForm.DrawPalette += DrawPalette;
            PaletteForm.NextByte += NextByte;
            PaletteForm.LastByte += LastByte;
            TileMapControl.ZeroIndexChanged += TileMapControl_ZeroIndexChanged;

            _startAddress = 0;
        }

        protected virtual void OnStartAddressChanged(EventArgs e)
        {
            var size = PaletteEditor.Palette.Count - Offset;
            GridLength = size / Color15BppBgr.SizeOf;
            TileMapControl.ZeroTile = StartAddress / Color15BppBgr.SizeOf;

            StartAddressChanged?.Invoke(this, e);
            TileMapControl.Invalidate();
        }

        private void TileMapControl_ZeroIndexChanged(object sender, EventArgs e)
        {
            StartAddress = Offset + TileMapControl.ZeroTile * Color15BppBgr.SizeOf;
        }

        private void LastByte(object sender, EventArgs e)
        {
            if (StartAddress <= 0)
            {
                PaletteForm.Status = "Cannot go any further.";
                return;
            }

            StartAddress--;
        }

        private void NextByte(object sender, EventArgs e)
        {
            if (StartAddress + 1 >= PaletteEditor.Palette.Count)
            {
                PaletteForm.Status = "Cannot go any further.";
                return;
            }

            StartAddress++;
        }

        private void DrawPalette(object sender, PaintEventArgs e)
        {
            var window = sender as TileMapControl1D;
            var width = window.TileMapWidth;
            var height = window.TileMapHeight;

            using (var image = new Bitmap(width, height))
            {
                var bitmapData = image.LockBits(
                    new Rectangle(Point.Empty, image.Size),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format32bppArgb);

                var drawer = new PaletteDrawer(PaletteEditor.Palette);
                drawer.DrawPalette(
                    bitmapData.Scan0,
                    bitmapData.Stride * bitmapData.Height,
                    StartAddress,
                    window.ViewWidth,
                    window.ViewHeight,
                    window.ZoomWidth,
                    window.ZoomHeight,
                    Selection1D.Empty);

                image.UnlockBits(bitmapData);

                e.Graphics.DrawImageUnscaled(image, Point.Empty);
            }
        }
    }
}
