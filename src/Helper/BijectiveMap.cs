// <copyright file="BijectiveMap.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class BijectiveMap<TKey, TValue> : IBijectiveMap<TKey, TValue>
    {
        public BijectiveMap()
            : this(0, null, null)
        {
        }

        public BijectiveMap(int capacity)
            : this(capacity, null, null)
        {
        }

        public BijectiveMap(
            IEqualityComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueComparer)
            : this(0, keyComparer, valueComparer)
        {
        }

        public BijectiveMap(
            int capacity,
            IEqualityComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueComparer)
        {
            KeyDictionary = new Dictionary<TKey, TValue>(
                capacity,
                keyComparer);

            ValueDictionary = new Dictionary<TValue, TKey>(
                capacity,
                valueComparer);

            KeyComparer = keyComparer ??
                EqualityComparer<TKey>.Default;

            ValueComparer = ValueComparer ??
                EqualityComparer<TValue>.Default;

            Reverse = new BijectiveMap<TValue, TKey>(this);
        }

        public BijectiveMap(BijectiveMap<TKey, TValue> bijectiveMap)
            : this(bijectiveMap, null, null)
        {
        }

        public BijectiveMap(
            BijectiveMap<TKey, TValue> bijectiveMap,
            IEqualityComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueComparer)
        {
            if (bijectiveMap is null)
            {
                throw new ArgumentNullException(nameof(bijectiveMap));
            }

            KeyDictionary = new Dictionary<TKey, TValue>(
                bijectiveMap.KeyDictionary,
                keyComparer);

            ValueDictionary = new Dictionary<TValue, TKey>(
                bijectiveMap.ValueDictionary,
                valueComparer);

            KeyComparer = keyComparer ??
                EqualityComparer<TKey>.Default;

            ValueComparer = ValueComparer ??
                EqualityComparer<TValue>.Default;

            Reverse = new BijectiveMap<TValue, TKey>(this);
        }

        private BijectiveMap(BijectiveMap<TValue, TKey> reverse)
        {
            ValueDictionary = reverse.KeyDictionary;
            KeyDictionary = reverse.ValueDictionary;
            Reverse = reverse;
        }

        public IEqualityComparer<TKey> KeyComparer
        {
            get;
        }

        public IEqualityComparer<TValue> ValueComparer
        {
            get;
        }

        public BijectiveMap<TValue, TKey> Reverse
        {
            get;
        }

        IBijectiveMap<TValue, TKey> IBijectiveMap<TKey, TValue>.Reverse
        {
            get
            {
                return Reverse;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return KeyDictionary.Keys;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                return Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return ValueDictionary.Keys;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                return Values;
            }
        }

        public int Count
        {
            get
            {
                return KeyDictionary.Count;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        private IDictionary<TKey, TValue> KeyDictionary
        {
            get;
        }

        private IDictionary<TValue, TKey> ValueDictionary
        {
            get;
        }

        public TValue this[TKey key]
        {
            get
            {
                return KeyDictionary[key];
            }

            set
            {
                Remove(key);
                Reverse.Remove(value);

                KeyDictionary[key] = value;
                ValueDictionary[value] = key;
            }
        }

        public void Add(TKey key, TValue value)
        {
            KeyDictionary.Add(key, value);
            ValueDictionary.Add(value, key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(
            KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return KeyDictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return ValueDictionary.ContainsKey(value);
        }

        public bool Contains(TKey key, TValue value)
        {
            return ContainsKey(key) && ContainsValue(value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(
            KeyValuePair<TKey, TValue> item)
        {
            return Contains(item.Key, item.Value);
        }

        public bool Remove(TKey key)
        {
            if (TryGetValue(key, out var value))
            {
                KeyDictionary.Remove(key);
                ValueDictionary.Remove(value);
                return true;
            }

            return false;
        }

        public bool Remove(TKey key, TValue value)
        {
            if (Contains(key, value))
            {
                KeyDictionary.Remove(key);
                ValueDictionary.Remove(value);
                return true;
            }

            return false;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(
            KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key, item.Value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return KeyDictionary.TryGetValue(key, out value);
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            return ValueDictionary.TryGetValue(value, out key);
        }

        public void Clear()
        {
            KeyDictionary.Clear();
            ValueDictionary.Clear();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
            KeyValuePair<TKey, TValue>[] array,
            int arrayIndex)
        {
            KeyDictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return KeyDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
