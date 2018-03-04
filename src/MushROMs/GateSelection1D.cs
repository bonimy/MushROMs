// <copyright file="GateSelection1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs
{
    using System;
    using System.Collections.Generic;

    public sealed class GateSelection1D : Selection1D
    {
        public GateSelection1D(
            ISelection1D left,
            ISelection1D right,
            Func<bool, bool, bool> rule)
            : base(GetStartIndex(left, right))
        {
            SelectedIndexes = GetSelectedIndexes(left, right, rule);
            HashIndexes = new HashSet<int>(SelectedIndexes);
        }

        private GateSelection1D(GateSelection1D selection)
            : base(selection.StartIndex)
        {
            SelectedIndexes = new List<int>(selection);
            HashIndexes = new HashSet<int>(selection);
        }

        public override int Count
        {
            get
            {
                return SelectedIndexes.Count;
            }
        }

        private IReadOnlyList<int> SelectedIndexes
        {
            get;
        }

        private ICollection<int> HashIndexes
        {
            get;
        }

        public override int this[int index]
        {
            get
            {
                return SelectedIndexes[index];
            }
        }

        public override ISelection1D Copy()
        {
            return new GateSelection1D(this);
        }

        public override bool Contains(int index)
        {
            return HashIndexes.Contains(index);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return SelectedIndexes.GetEnumerator();
        }

        private static int GetStartIndex(
            ISelection1D left,
            ISelection1D right)
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
                return right.StartIndex;
            }

            if (right.Count == 0)
            {
                return left.StartIndex;
            }

            return Math.Min(left.StartIndex, right.StartIndex);
        }

        private static IReadOnlyList<int> GetSelectedIndexes(
            ISelection1D left,
            ISelection1D right,
            Func<bool, bool, bool> rule)
        {
            // Get all indexes from left selection that fit the binary rule.
            var result = GetLeftIndexes(left, right, rule);

            // Hash the current results.
            var hash = new HashSet<int>(result);

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

                // Add index if it fits the binary selection rule.
                if (rule(contains, true))
                {
                    result.Add(index);
                }
            }

            return result;
        }

        private static List<int> GetLeftIndexes(
            ISelection1D left,
            ISelection1D right,
            Func<bool, bool, bool> rule)
        {
            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            var result = new List<int>(left.Count + right.Count);

            // Add the left indexes.
            foreach (var index in left)
            {
                // Check if right selection contains an index from left.
                var contains = right.Contains(index);

                // Add index if it fits the binary selection rule.
                if (rule(true, contains))
                {
                    result.Add(index);
                }
            }

            return result;
        }
    }
}
