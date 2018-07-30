// <copyright file="FormControlTileMapHelper1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls
{
    using System.Windows.Forms;
    using MushROMs;

    public class FormControlTileMapHelper1D : FormControlTileMapHelper
    {
        public new TileMap1D TileMap
        {
            get
            {
                return base.TileMap as TileMap1D;
            }
        }

        public FormControlTileMapHelper1D(
            TileMap tileMap,
            DesignForm form,
            DesignControl control,
            ScrollBar vScrollBar,
            ScrollBar hScrollBar) :
            base(tileMap, form, control, vScrollBar, hScrollBar)
        {
        }

        protected override void ResetHorizontalScrollBar()
        {
            if (HScrollBar is null)
            {
                return;
            }

            if (HScrollBar.Enabled = TileMap.ViewWidth > 1)
            {
                HScrollBar.SmallChange = 1;
                HScrollBar.LargeChange = TileMap.ViewWidth - 1;
                HScrollBar.Minimum = 0;
                HScrollBar.Maximum = ((TileMap.ViewWidth - 1) * 2) - 1;
                HScrollBar.Value = TileMap.ZeroTile % TileMap.ViewWidth;
            }
        }

        protected override void ResetVerticalScrollBar()
        {
            if (VScrollBar is null)
            {
                return;
            }

            var rows = TileMap.GridSize / TileMap.ViewWidth;
            var enabled = rows > TileMap.ViewHeight;

            if (enabled)
            {
                VScrollBar.Enabled = true;
                VScrollBar.Minimum = 0;
                VScrollBar.Maximum = rows - 1;
                VScrollBar.SmallChange = 1;
                VScrollBar.LargeChange = TileMap.ViewHeight;

                var value = TileMap.ZeroTile / TileMap.ViewWidth;
                if (rows <= value + TileMap.ViewHeight)
                {
                    value = rows - TileMap.ViewHeight;
                }

                VScrollBar.Value = value;
            }
            else
            {
                VScrollBar.Value = 0;
                VScrollBar.Enabled = false;
            }
        }

        protected override void AdjustScrollBarPositions()
        {
            if (HScrollBar != null)
            {
                HScrollBar.Value = TileMap.ZeroTile % TileMap.ViewWidth;
            }

            if (VScrollBar != null)
            {
                VScrollBar.Value = TileMap.ZeroTile / TileMap.ViewWidth;
            }
        }

        protected override void ScrollTileMapHorizontal(int value)
        {
            var zeroY = TileMap.ZeroTile / TileMap.ViewWidth;
            TileMap.ZeroTile = value + (zeroY * TileMap.ViewHeight);
        }

        protected override void ScrollTileMapVertical(int value)
        {
            var zeroX = TileMap.ZeroTile % TileMap.ViewWidth;
            TileMap.ZeroTile = (value * TileMap.ViewWidth) + zeroX;
        }
    }
}
