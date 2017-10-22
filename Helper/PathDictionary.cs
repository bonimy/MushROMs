// <copyright file="PathDictionary.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;

namespace Helper
{
    public sealed class PathDictionary<TValue> : AssertDictionary<string, TValue>
    {
        public PathDictionary() :
            base(PathComparer.DefaultComparer)
        {
        }

        public PathDictionary(int capacity) :
            base(capacity, PathComparer.DefaultComparer)
        {
        }

        public PathDictionary(PathComparer comparer) :
            base(comparer ?? throw new ArgumentNullException(nameof(comparer)))
        {
        }

        public PathDictionary(PathDictionary<TValue> dictionary) =>
            BaseDictionary = new Dictionary<string, TValue>(dictionary, PathComparer.DefaultComparer);

        public PathDictionary(int capacity, PathComparer comparer) :
            base(capacity, comparer ?? PathComparer.DefaultComparer)
        {
        }

        public PathDictionary(PathDictionary<TValue> dictionary, ExtensionComparer comparer) =>
            BaseDictionary = new Dictionary<string, TValue>(dictionary, comparer);

        protected override void AssertKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var comparer = Comparer as PathComparer;

            try
            {
                comparer.StringModifier(key);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, nameof(key), ex);
            }
        }
    }
}
