// <copyright file="GateSelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public sealed class GateSelection2D : Selection2D
    {
        public GateSelection2D(
            Selection2D left,
            Selection2D right,
            Func<bool, bool, bool> rule)
            : base(GetStartPosition(left, right))
        {
            SelectedIndexes = GetSelectedIndexes(left, right, rule);
            HashIndexes = new HashSet<Point>(SelectedIndexes);
        }

        private GateSelection2D(GateSelection2D selection)
            : base(selection.StartPosition)
        {
            SelectedIndexes = new List<Point>(selection);
            HashIndexes = new HashSet<Point>(selection);
        }

        public override int Count
        {
            get
            {
                return SelectedIndexes.Count;
            }
        }

        private IReadOnlyList<Point> SelectedIndexes
        {
            get;
        }

        private ICollection<Point> HashIndexes
        {
            get;
        }

        public override Point this[int index]
        {
            get
            {
                return SelectedIndexes[index];
            }
        }

        public override Selection2D Copy()
        {
            return new GateSelection2D(this);
        }

        public override bool Contains(Point position)
        {
            return HashIndexes.Contains(position);
        }

        public override IEnumerator<Point> GetEnumerator()
        {
            return SelectedIndexes.GetEnumerator();
        }

        private static Point GetStartPosition(
            Selection2D left,
            Selection2D right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.Count == 0)
            {
                return right.StartPosition;
            }

            if (right.Count == 0)
            {
                return left.StartPosition;
            }

            var x = Math.Min(left.StartPosition.X, right.StartPosition.X);
            var y = Math.Min(left.StartPosition.Y, right.StartPosition.Y);
            return new Point(x, y);
        }

        private static IReadOnlyList<Point> GetSelectedIndexes(
            Selection2D left,
            Selection2D right,
            Func<bool, bool, bool> rule)
        {
            var result = GetLeftIndexes(left, right, rule);

            // The hash set saves all indexes we've added in the first comparison.
            var hash = new HashSet<Point>(result);

            // Add the right indexes.
            foreach (var index in right)
            {
                // Skip indexes we've already checked
                if (hash.Contains(index))
                {
                    continue;
                }

                // Check if left selection contains an index from right selection.
                var contains = left.Contains(index);

                // Add index if fits the binary selection rule.
                if (rule(contains, true))
                {
                    result.Add(index);
                }
            }

            return result;
        }

        private static List<Point> GetLeftIndexes(
            Selection2D left,
            Selection2D right,
            Func<bool, bool, bool> rule)
        {
            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            var result = new List<Point>(left.Count + right.Count);

            // Add the left indexes.
            foreach (var index in left)
            {
                // Check if right selection contains an index from left selection.
                var contains = right.Contains(index);

                // Add index if fits the binary selection rule.
                if (rule(true, contains))
                {
                    result.Add(index);
                }
            }

            return result;
        }
    }
}
