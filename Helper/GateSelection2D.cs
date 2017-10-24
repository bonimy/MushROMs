// <copyright file="GateSelection2D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;

namespace Helper
{
    public sealed class GateSelection2D : Selection2D
    {
        private Selection2D Left
        {
            get;
            set;
        }

        private Selection2D Right
        {
            get;
            set;
        }

        private GateMethod Rule
        {
            get;
            set;
        }

        private List<Position2D> SelectedIndexes
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

        public GateSelection2D(Selection2D left, Selection2D right, GateMethod rule)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));

            if (Left is EmptySelection2D)
            {
                StartPosition = Right.StartPosition;
            }
            else if (Right is EmptySelection2D)
            {
                StartPosition = Left.StartPosition;
            }
            else
            {
                StartPosition = Position2D.TopLeft(Left.StartPosition, Right.StartPosition);
            }

            InitializeSelectedIndexes();
        }

        public override bool Contains(Position2D position)
        {
            var left = Left.Contains(position);
            var right = Right.Contains(position);

            return Rule(left, right);
        }

        private void InitializeSelectedIndexes()
        {
            var result = new List<Position2D>(Left.Count + Right.Count);

            // Add the left indexes.
            foreach (var index in Left)
            {
                // Check if right selection contains an index from left selection.
                var contains = Right.Contains(index);

                // Add index if fits the binary selection rule.
                if (Rule(true, contains))
                {
                    result.Add(index);
                }
            }

            // The hash set saves all indexes we've added in the first comparison.
            var hash = new HashSet<Position2D>(result);

            // Add the right indexes.
            foreach (var index in Right)
            {
                // Skip indexes we've already checked
                if (hash.Contains(index))
                {
                    continue;
                }

                // Check if left selection contains an index from right selection.
                var contains = Left.Contains(index);

                // Add index if fits the binary selection rule.
                if (Rule(contains, true))
                {
                    result.Add(index);
                }
            }

            SelectedIndexes = result;
        }

        public override IEnumerator<Position2D> GetEnumerator()
        {
            return SelectedIndexes.GetEnumerator();
        }
    }
}
