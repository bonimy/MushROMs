// <copyright file="AssertDictionary.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
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
    public abstract class AssertDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>,
        ICollection<KeyValuePair<TKey, TValue>>,
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        IEnumerable,
        IDictionary,
        ICollection
    {
        protected internal Dictionary<TKey, TValue> BaseDictionary
        {
            get;
            set;
        }

        public int Count
        {
            get
            {
                return BaseDictionary.Count;
            }
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                return BaseDictionary.Keys;
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                return BaseDictionary.Values;
            }
        }

        public IEqualityComparer<TKey> Comparer
        {
            get
            {
                return BaseDictionary.Comparer;
            }
        }

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

        protected AssertDictionary()
        {
            BaseDictionary = new Dictionary<TKey, TValue>();
        }

        protected AssertDictionary(int capacity)
        {
            BaseDictionary = new Dictionary<TKey, TValue>(capacity);
        }

        protected AssertDictionary(IEqualityComparer<TKey> comparer)
        {
            BaseDictionary = new Dictionary<TKey, TValue>(comparer);
        }

        protected AssertDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            BaseDictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public bool ContainsKey(TKey key)
        {
            AssertKey(key);
            return BaseDictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return BaseDictionary.ContainsValue(value);
        }

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

        public void Clear()
        {
            BaseDictionary.Clear();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            AssertKey(key);
            return BaseDictionary.TryGetValue(key, out value);
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return BaseDictionary.GetEnumerator();
        }

        protected abstract void AssertKey(TKey key);

        private IDictionary<TKey, TValue> IGenericDictionary
        {
            get
            {
                return BaseDictionary;
            }
        }

        private IDictionary IDictionary
        {
            get
            {
                return BaseDictionary;
            }
        }

        private ICollection ICollection
        {
            get
            {
                return BaseDictionary;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return IDictionary.IsReadOnly;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return IGenericDictionary.IsReadOnly;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return IDictionary.IsFixedSize;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return ICollection.IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return ICollection.SyncRoot;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return Keys;
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                return Keys;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                return Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return Values;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                return Values;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                return Values;
            }
        }

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

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            IGenericDictionary.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ICollection.CopyTo(array, index);
        }

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

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
