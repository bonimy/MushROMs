// <copyright file="RectangleEventArgs.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Controls
{
    using System;
    using System.Drawing;

    public class RectangleEventArgs : EventArgs
    {
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(Location, Size);
            }

            set
            {
                Location = value.Location;
                Size = value.Size;
            }
        }

        public Point Location
        {
            get;
            set;
        }

        public Size Size
        {
            get;
            set;
        }

        public RectangleEventArgs(Point location)
        {
            Location = location;
        }

        public RectangleEventArgs(Size size)
        {
            Size = size;
        }

        public RectangleEventArgs(Rectangle rectangle)
        {
            Location = rectangle.Location;
            Size = rectangle.Size;
        }
    }
}
