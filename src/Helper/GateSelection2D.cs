// <copyright file="GateSelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;

namespace Helper
{
    public sealed class GateSelection2D : Selection2D
    {
        private IReadOnlyList<Position2D> SelectedIndexes
        {
            get;
            set;
        }

        private HashSet<Position2D> HashIndexes
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

        public override Position2D this[int index]
        {
            get
            {
                return SelectedIndexes[index];
            }
        }

        public GateSelection2D(Selection2D left, Selection2D right, GateMethod rule)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            if (left is EmptySelection2D)
            {
                StartPosition = right.StartPosition;
            }
            else if (right is EmptySelection2D)
            {
                StartPosition = left.StartPosition;
            }
            else
            {
                StartPosition = Position2D.TopLeft(left.StartPosition, right.StartPosition);
            }

            InitializeSelectedIndexes(left, right, rule);
        }

        private GateSelection2D(GateSelection2D selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            SelectedIndexes = new List<Position2D>(selection);
            HashIndexes = new HashSet<Position2D>(selection);
        }

        public override Selection2D Copy()
        {
            return new GateSelection2D(this);
        }

        public override bool Contains(Position2D position)
        {
            return HashIndexes.Contains(position);
        }

        private void InitializeSelectedIndexes(Selection2D left, Selection2D right, GateMethod rule)
        {
            var result = new List<Position2D>(left.Count + right.Count);

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
            var hash = new HashSet<Position2D>(result);

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
            HashIndexes = new HashSet<Position2D>(SelectedIndexes);
        }

        public override IEnumerator<Position2D> GetEnumerator()
        {
            return SelectedIndexes.GetEnumerator();
        }
    }
}
