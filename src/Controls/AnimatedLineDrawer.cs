// <copyright file="AnimatedLineDrawer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static Helper.ThrowHelper;

namespace Controls
{
    public class AnimatedLineDrawer : Component
    {
        private int _length1;
        private int _length2;

        private int Offset
        {
            get;
            set;
        }

        public int Length1
        {
            get
            {
                return _length1;
            }

            set
            {
                if (value <= 0)
                {
                    throw ValueNotGreaterThan(
                        nameof(value),
                        value);
                }

                _length1 = value;
            }
        }

        public int Length2
        {
            get
            {
                return _length2;
            }

            set
            {
                if (value <= 0)
                {
                    throw ValueNotGreaterThan(
                        nameof(value),
                        value);
                }

                _length2 = value;
            }
        }

        public Color Color1
        {
            get;
            set;
        }

        public Color Color2
        {
            get;
            set;
        }

        private Timer Timer
        {
            get;
        }

        public int Interval
        {
            get
            {
                return Timer.Interval;
            }

            set
            {
                Timer.Interval = value;
            }
        }

        public override ISite Site
        {
            get
            {
                return Timer.Site;
            }

            set
            {
                Timer.Site = value;
            }
        }

        private object SyncRoot
        {
            get;
        }

        public AnimatedLineDrawer() : this(null)
        {
        }

        public AnimatedLineDrawer(IContainer container) :
            this(container, 1, 1, Color.Black, Color.White, 1000)
        {
        }

        public AnimatedLineDrawer(
            int length1,
            int length2,
            Color color1,
            Color color2,
            int interval) :
            this(null, length1, length2, color1, color2, interval)
        {
        }

        public AnimatedLineDrawer(
            IContainer container,
            int length1,
            int length2,
            Color color1,
            Color color2,
            int interval)
        {
            Length1 = length1;
            Length2 = length2;
            Color1 = color1;
            Color2 = color2;
            SyncRoot = new object();

            if (container is null)
            {
                Timer = new Timer();
            }
            else
            {
                Timer = new Timer(container);
            }

            Timer.Interval = interval;
            Timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Offset++;
        }

        public void DrawPath(Graphics graphics, GraphicsPath path)
        {
            if (graphics is null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            lock (SyncRoot)
            {
                using (var pen1 = new Pen(Color1))
                using (var pen2 = new Pen(Color2))
                {
                    pen1.DashStyle = DashStyle.Custom;
                    pen2.DashStyle = DashStyle.Custom;

                    pen1.DashOffset = Offset;
                    pen2.DashOffset = Offset + Length1;

                    pen1.DashPattern = new float[] { Length1, Length2 };
                    pen2.DashPattern = new float[] { Length2, Length1 };

                    graphics.DrawPath(pen1, path);
                    graphics.DrawPath(pen2, path);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Timer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
