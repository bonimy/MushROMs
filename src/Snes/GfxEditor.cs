// <copyright file="GfxEditor.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Snes
{
    using System;
    using MushROMs;
    using static System.IO.Path;

    public class GfxEditor : Editor
    {
        public const string FallbackExtension = ".chr";

        private static int _untitledNumber = 0;

        private static string _defaultExtension = FallbackExtension;

        public GfxEditor(Gfx gfx)
            : this(gfx, NextUntitledPath())
        {
        }

        public GfxEditor(Gfx gfx, string path)
            : base(path)
        {
            Gfx = gfx ??
                throw new ArgumentNullException(nameof(gfx));
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

        public Gfx Gfx
        {
            get;
        }

        public IGfxSelection CurrentSelection
        {
            get;
            set;
        }

        public GfxData CopyData
        {
            get;
            private set;
        }

        public override void Copy()
        {
            Copy(CurrentSelection);
        }

        public void Copy(IGfxSelection selection)
        {
            CopyData = CreateData(selection);
        }

        public override void Delete()
        {
            Delete(CurrentSelection);
        }

        public void Delete(IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(gfxData.Empty, gfxData);
        }

        public void FlipX()
        {
            FlipX(CurrentSelection);
        }

        public void FlipX(IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(gfxData.FlipX, gfxData);
        }

        public void FlipY()
        {
            FlipY(CurrentSelection);
        }

        public void FlipY(IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(gfxData.FlipY, gfxData);
        }

        public void Rotate90()
        {
            Rotate90(CurrentSelection);
        }

        public void Rotate90(IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(gfxData.Rotate90, gfxData);
        }

        public void Rotate180()
        {
            Rotate180(CurrentSelection);
        }

        public void Rotate180(IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(gfxData.Rotate180, gfxData);
        }

        public void Rotate270()
        {
            Rotate270(CurrentSelection);
        }

        public void Rotate270(IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(gfxData.Rotate270, gfxData);
        }

        public void ReplaceColor(
            byte original,
            byte replacement)
        {
            ReplaceColor(original, replacement, CurrentSelection);
        }

        public void ReplaceColor(
            byte original,
            byte replacement,
            IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(ReplaceColor, gfxData);

            void ReplaceColor()
            {
                gfxData.ReplaceColor(original, replacement);
            }
        }

        public void SwapColors(
            byte color1,
            byte color2)
        {
            SwapColors(color1, color2, CurrentSelection);
        }

        public void SwapColors(
            byte color1,
            byte color2,
            IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(SwapColors, gfxData);

            void SwapColors()
            {
                gfxData.SwapColors(color1, color2);
            }
        }

        public void RotateColors(
            byte first,
            byte last,
            byte shift)
        {
            RotateColors(first, last, shift, CurrentSelection);
        }

        public void RotateColors(
            byte first,
            byte last,
            byte shift,
            IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(RotateColors, gfxData);

            void RotateColors()
            {
                gfxData.RotateColors(first, last, shift);
            }
        }

        public void AlterTiles(
            Func<GfxTile, GfxTile> alterTile)
        {
            AlterTiles(alterTile, CurrentSelection);
        }

        public void AlterTiles(
            Func<GfxTile, GfxTile> alterTile,
            IGfxSelection selection)
        {
            var gfxData = CreateData(selection);
            Write(AlterTiles, gfxData);

            void AlterTiles()
            {
                gfxData.AlterTiles(alterTile);
            }
        }

        public void Write(
            Action<GfxData> write,
            GfxData gfxData)
        {
            if (write is null)
            {
                throw new ArgumentNullException(nameof(gfxData));
            }

            Write(() => write(gfxData), gfxData);
        }

        private static string NextUntitledPath()
        {
            return NextUntitledPath(
                "GFX",
                ".chr",
                ref _untitledNumber);
        }

        private void Write(Action write, GfxData gfxData)
        {
            var undo = GenerateUndo(gfxData);
            WriteData(Write, undo);

            void Write()
            {
                write();
                Gfx.WriteGfxData(gfxData);
            }
        }

        private Action GenerateUndo(GfxData gfxData)
        {
            if (gfxData is null)
            {
                throw new ArgumentNullException(nameof(gfxData));
            }

            var copy = gfxData.Copy();
            return () => Gfx.WriteGfxData(copy);
        }

        private GfxData CreateData(IGfxSelection selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            return Gfx.CreateGfxFromSelection(selection);
        }
    }
}
