// <copyright file="GateSelection1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;

namespace MushROMs
{
    public delegate bool GateMethod(bool left, bool right);

    public sealed class GateSelection1D : Selection1D
    {
        private IReadOnlyList<int> SelectedIndexes
        {
            get;
            set;
        }

        private HashSet<int> HashIndexes
        {
            get;
            set;
        }

        public override int Count
        {
            get
            {
                return SelectedIndexes.Count;
            }
        }

        public override int this[int index]
        {
            get
            {
                return SelectedIndexes[index];
            }
        }

        public GateSelection1D(Selection1D left, Selection1D right, GateMethod rule)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            if (left is EmptySelection1D)
            {
                StartIndex = right.StartIndex;
            }
            else if (right is EmptySelection1D)
            {
                StartIndex = left.StartIndex;
            }
            else
            {
                StartIndex = Math.Min(left.StartIndex, right.StartIndex);
            }

            InitializeSelectedIndexes(left, right, rule);
        }

        private GateSelection1D(GateSelection1D selection)
        {
            if (selection == null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            StartIndex = selection.StartIndex;

            SelectedIndexes = new List<int>(selection);
            HashIndexes = new HashSet<int>(selection);
        }

        public override Selection1D Copy()
        {
            return new GateSelection1D(this);
        }

        public override bool Contains(int index)
        {
            return HashIndexes.Contains(index);
        }

        private void InitializeSelectedIndexes(Selection1D left, Selection1D right, GateMethod rule)
        {
            var result = new List<int>(left.Count + right.Count);

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

            // The hash set saves all indexes we've added in the first comparison.
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

                // Add index if fits the binary selection rule.
                if (rule(contains, true))
                {
                    result.Add(index);
                }
            }

            SelectedIndexes = result;
            HashIndexes = new HashSet<int>(SelectedIndexes);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return SelectedIndexes.GetEnumerator();
        }
    }
}
