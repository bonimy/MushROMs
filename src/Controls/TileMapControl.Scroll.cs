﻿// <copyright file="TileMapControl.Scroll.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Controls
{
    partial class TileMapControl
    {
        private ScrollBar _verticalScrollBar;
        private ScrollBar _horizontalScrollBar;

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The vertical scroll bar to associate with this tilemap.")]
        public ScrollBar VerticalScrollBar
        {
            get
            {
                return _verticalScrollBar;
            }

            set
            {
                if (VerticalScrollBar == value)
                {
                    return;
                }

                RemoveVerticalScrollBarEvents();
                _verticalScrollBar = value;
                AddVerticalScrollBarEvents();
                ResetVerticalScrollBar();
            }
        }

        [Browsable(true)]
        [Category("Tilemap")]
        [Description("The horizontal scroll bar to associate with this tilemap.")]
        public ScrollBar HorizontalScrollBar
        {
            get
            {
                return _horizontalScrollBar;
            }

            set
            {
                if (HorizontalScrollBar == value)
                {
                    return;
                }

                RemoveHorizontalScrollBarEvents();
                _horizontalScrollBar = value;
                AddHorizontalScrollBarEvents();
                ResetHorizontalScrollBar();
            }
        }

        private void AddHorizontalScrollBarEvents()
        {
            if (HorizontalScrollBar is null)
            {
                return;
            }

            HorizontalScrollBar.Scroll += HorizontalScrollBar_Scroll;
            HorizontalScrollBar.ValueChanged += HorizontalScrollBar_ValueChanged;
        }

        private void RemoveHorizontalScrollBarEvents()
        {
            if (HorizontalScrollBar is null)
            {
                return;
            }

            HorizontalScrollBar.Scroll -= HorizontalScrollBar_Scroll;
            HorizontalScrollBar.ValueChanged -= HorizontalScrollBar_ValueChanged;
        }

        private void AddVerticalScrollBarEvents()
        {
            if (VerticalScrollBar is null)
            {
                return;
            }

            VerticalScrollBar.Scroll += VerticalScrollBar_Scroll;
            VerticalScrollBar.ValueChanged += VerticalScrollBar_ValueChanged;
        }

        private void RemoveVerticalScrollBarEvents()
        {
            if (VerticalScrollBar is null)
            {
                return;
            }

            VerticalScrollBar.Scroll -= VerticalScrollBar_Scroll;
            VerticalScrollBar.ValueChanged -= VerticalScrollBar_ValueChanged;
        }

        public void ResetScrollBars()
        {
            ResetVerticalScrollBar();
            ResetHorizontalScrollBar();
        }

        protected abstract void ResetHorizontalScrollBar();

        protected abstract void ResetVerticalScrollBar();

        protected abstract void AdjustScrollBarPositions();

        protected abstract void ScrollTileMapVertical(int value);

        protected abstract void ScrollTileMapHorizontal(int value);

        private void HorizontalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            ScrollTileMapHorizontal(e.NewValue);
            Invalidate();
        }

        private void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            ScrollTileMapVertical(e.NewValue);
            Invalidate();
        }

        private void VerticalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapVertical(VerticalScrollBar.Value);
            Invalidate();
        }

        private void HorizontalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapHorizontal(HorizontalScrollBar.Value);
            Invalidate();
        }
    }
}
