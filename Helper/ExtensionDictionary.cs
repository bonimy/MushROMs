// <copyright file="ExtensionDictionary.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Collections.Generic;

namespace Helper
{
    public sealed class ExtensionDictionary<TValue> : AssertDictionary<string, TValue>
    {
        public ExtensionDictionary() :
            base(ExtensionComparer.DefaultComparer)
        {
        }

        public ExtensionDictionary(int capacity) :
            base(capacity, ExtensionComparer.DefaultComparer)
        {
        }

        public ExtensionDictionary(ExtensionComparer comparer) :
            base(comparer ?? throw new ArgumentNullException(nameof(comparer)))
        {
        }

        public ExtensionDictionary(ExtensionDictionary<TValue> dictionary)
        {
            BaseDictionary = new Dictionary<string, TValue>(dictionary, ExtensionComparer.DefaultComparer);
        }

        public ExtensionDictionary(int capacity, ExtensionComparer comparer) :
            base(capacity, comparer ?? ExtensionComparer.DefaultComparer)
        {
        }

        public ExtensionDictionary(ExtensionDictionary<TValue> dictionary, ExtensionComparer comparer)
        {
            BaseDictionary = new Dictionary<string, TValue>(dictionary, comparer);
        }

        protected override void AssertKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var comparer = Comparer as ExtensionComparer;

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
