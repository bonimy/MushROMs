// <copyright file="EnumerableSelection.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;

namespace Helper
{
    public class EnumerableSelection : Selection1D
    {
        private ICollection<int> Indexes
        {
            get;
            set;
        }

        public override int Count
        {
            get
            {
                return Indexes.Count;
            }
        }

        public EnumerableSelection(IEnumerable<int> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            Indexes = new HashSet<int>(collection);
        }

        public override bool Contains(int index)
        {
            return Indexes.Contains(index);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return Indexes.GetEnumerator();
        }
    }
}
