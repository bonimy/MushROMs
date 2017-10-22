// <copyright file="AssertDictionary.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

/*
 This class is used for extension and path dictionaries. Strings that aren't "path-type" shouldn't be added to the dictionary. So this class first checks that a key has a certain format before using it. If the key isn't in that format, an exception is thrown. Typically, this is an ArgumentException, but the overrider can do whatever they please.

 This class cannot override dictionary, because its methods are not override methods, so if this class is cast as a Dictionary object, the wrong methods would be called and the assertions would not be checked.
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public abstract class AssertDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary
    {
        internal Dictionary<TKey, TValue> BaseDictionary
        {
            get;
            set;
        }

        public int Count => BaseDictionary.Count;

        public Dictionary<TKey, TValue>.KeyCollection Keys => BaseDictionary.Keys;

        public Dictionary<TKey, TValue>.ValueCollection Values => BaseDictionary.Values;

        public IEqualityComparer<TKey> Comparer => BaseDictionary.Comparer;

        public TValue this[TKey key]
        {
            get
            {
                AssertKey(key);
                return BaseDictionary[key];
            }

            set
            {
                AssertKey(key);
                BaseDictionary[key] = value;
            }
        }

        protected AssertDictionary() =>
            BaseDictionary = new Dictionary<TKey, TValue>();

        protected AssertDictionary(int capacity) =>
            BaseDictionary = new Dictionary<TKey, TValue>(capacity);

        protected AssertDictionary(IEqualityComparer<TKey> comparer) =>
            BaseDictionary = new Dictionary<TKey, TValue>(comparer);

        protected AssertDictionary(int capacity, IEqualityComparer<TKey> comparer) =>
            BaseDictionary = new Dictionary<TKey, TValue>(capacity, comparer);

        public bool ContainsKey(TKey key)
        {
            AssertKey(key);
            return BaseDictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value) =>
            BaseDictionary.ContainsValue(value);

        public void Add(TKey key, TValue value)
        {
            AssertKey(key);
            BaseDictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            AssertKey(key);
            return BaseDictionary.Remove(key);
        }

        public void Clear() => BaseDictionary.Clear();

        public bool TryGetValue(TKey key, out TValue value)
        {
            AssertKey(key);
            return BaseDictionary.TryGetValue(key, out value);
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator() =>
            BaseDictionary.GetEnumerator();

        protected abstract void AssertKey(TKey key);

        private IDictionary<TKey, TValue> IGenericDictionary =>
            BaseDictionary;

        private IDictionary IDictionary => BaseDictionary;

        private ICollection ICollection => BaseDictionary;

        bool IDictionary.IsReadOnly => IDictionary.IsReadOnly;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly =>
            IGenericDictionary.IsReadOnly;

        bool IDictionary.IsFixedSize => IDictionary.IsFixedSize;

        bool ICollection.IsSynchronized => ICollection.IsSynchronized;

        object ICollection.SyncRoot => ICollection.SyncRoot;

        ICollection IDictionary.Keys => Keys;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        ICollection IDictionary.Values => Values;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        object IDictionary.this[object key]
        {
            get
            {
                if (key is TKey t)
                {
                    AssertKey(t);
                }

                return IDictionary[key];
            }

            set
            {
                if (key is TKey t)
                {
                    AssertKey(t);
                }

                IDictionary[key] = value;
            }
        }

        void IDictionary.Add(object key, object value)
        {
            if (key is TKey t)
            {
                AssertKey(t);
            }

            IDictionary.Add(key, value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            AssertKey(item.Key);
            IGenericDictionary.Add(item);
        }

        bool IDictionary.Contains(object key)
        {
            if (key is TKey t)
            {
                AssertKey(t);
            }

            return IDictionary.Contains(key);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            AssertKey(item.Key);
            return IGenericDictionary.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            IGenericDictionary.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) =>
            ICollection.CopyTo(array, index);

        void IDictionary.Remove(object key)
        {
            if (key is TKey t)
            {
                AssertKey(t);
            }

            IDictionary.Remove(key);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            AssertKey(item.Key);
            return IGenericDictionary.Remove(item);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
            GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IDictionaryEnumerator IDictionary.GetEnumerator() => GetEnumerator();
    }
}
