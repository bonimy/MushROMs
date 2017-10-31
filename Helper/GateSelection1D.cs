// <copyright file="GateSelection1D.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;

namespace Helper
{
    public delegate bool GateMethod(bool left, bool right);

    public sealed class GateSelection1D : Selection1D
    {
        private Selection1D Left
        {
            get;
            set;
        }

        private Selection1D Right
        {
            get;
            set;
        }

        private GateMethod Rule
        {
            get;
            set;
        }

        private IReadOnlyList<int> SelectedIndexes
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
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));

            if (Left is EmptySelection1D)
            {
                StartIndex = Right.StartIndex;
            }
            else if (Right is EmptySelection1D)
            {
                StartIndex = Left.StartIndex;
            }
            else
            {
                StartIndex = Math.Min(Left.StartIndex, Right.StartIndex);
            }

            InitializeSelectedIndexes();
        }

        public override bool Contains(int index)
        {
            var left = Left.Contains(index);
            var right = Right.Contains(index);

            return Rule(left, right);
        }

        private void InitializeSelectedIndexes()
        {
            var result = new List<int>(Left.Count + Right.Count);

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
            var hash = new HashSet<int>(result);

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

        public override IEnumerator<int> GetEnumerator()
        {
            return SelectedIndexes.GetEnumerator();
        }
    }
}
