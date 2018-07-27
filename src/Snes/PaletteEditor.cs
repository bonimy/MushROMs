// <copyright file="PaletteEditor.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using Helper;
    using Helper.PixelFormat;
    using MushROMs;
    using static System.IO.Path;

    public class PaletteEditor : Editor
    {
        public const string FallbackExtension = ".rpf";

        private static int _untitledNumber = 0;

        private static string _defaultExtension = FallbackExtension;

        public PaletteEditor(Palette palette)
            : this(palette, NextUntitledPath())
        {
        }

        public PaletteEditor(Palette palette, string path)
            : base(path)
        {
            Palette = palette ??
                throw new ArgumentNullException(nameof(palette));
        }

        public static string DefaultExtension
        {
            get
            {
                return _defaultExtension;
            }

            set
            {
                try
                {
                    _defaultExtension = GetExtension(value);
                }
                catch (ArgumentException)
                {
                    _defaultExtension = FallbackExtension;
                }
            }
        }

        public Palette Palette
        {
            get;
        }

        public ISelection1D CurrentSelection
        {
            get;
            set;
        }

        public PaletteData CopyData
        {
            get;
            private set;
        }

        public override void Copy()
        {
            Copy(CurrentSelection);
        }

        public void Copy(ISelection1D selection)
        {
            CopyData = CreateData(selection);
        }

        public override void Delete()
        {
            Delete(CurrentSelection);
        }

        public void Delete(ISelection1D selection)
        {
            var paletteData = CreateData(selection);
            Write(paletteData.Empty, paletteData);
        }

        public void InvertColors()
        {
            InvertColors(CurrentSelection);
        }

        public void InvertColors(ISelection1D selection)
        {
            var paletteData = CreateData(selection);
            Write(paletteData.InvertColors, paletteData);
        }

        public void Blend(BlendMode blendMode, ColorF bottom)
        {
            Blend(blendMode, bottom, CurrentSelection);
        }

        public void Blend(
            BlendMode blendMode,
            ColorF bottom,
            ISelection1D selection)
        {
            var paletteData = CreateData(selection);
            Write(Blend, paletteData);

            void Blend()
            {
                paletteData.Blend(blendMode, bottom);
            }
        }

        public void Colorize(ColorF amount)
        {
            Colorize(CurrentSelection, amount);
        }

        public void Colorize(ISelection1D selection, ColorF amount)
        {
            Blend(BlendMode.Hue, amount, selection);
        }

        public void Grayscale(ColorF weight)
        {
            Grayscale(CurrentSelection, weight);
        }

        public void Grayscale(ISelection1D selection, ColorF weight)
        {
            Blend(BlendMode.Grayscale, weight);
        }

        public void AlterColors(
            Func<Color15BppBgr, Color15BppBgr> alterTile)
        {
            AlterColors(alterTile, CurrentSelection);
        }

        public void AlterColors(
            Func<Color15BppBgr, Color15BppBgr> alterTile,
            ISelection1D selection)
        {
            var paletteData = CreateData(selection);
            Write(RotateColors, paletteData);

            void RotateColors()
            {
                paletteData.AlterColors(alterTile);
            }
        }

        public void Write(
            Action<PaletteData> write,
            PaletteData paletteData)
        {
            if (write is null)
            {
                throw new ArgumentNullException(nameof(paletteData));
            }

            Write(() => write(paletteData), paletteData);
        }

        private static string NextUntitledPath()
        {
            return NextUntitledPath(
                "Palette",
                ".rpf",
                ref _untitledNumber);
        }

        private void Write(Action write, PaletteData paletteData)
        {
            var undo = GenerateUndo(paletteData);
            WriteData(Write, undo);

            void Write()
            {
                write();
                Palette.WritePaletteData(paletteData);
            }
        }

        private Action GenerateUndo(PaletteData paletteData)
        {
            if (paletteData is null)
            {
                throw new ArgumentNullException(nameof(paletteData));
            }

            var copy = paletteData.Copy();
            return () => Palette.WritePaletteData(copy);
        }

        private PaletteData CreateData(ISelection1D selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            return Palette.CreatePaletteFromSelection(selection);
        }
    }
}
