// <copyright file="AssertDictionary.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

/*
 This class is used for extension and path dictionaries. Strings that aren't "path-type" shouldn't be added to the dictionary. So this class first checks that a key has a certain format before using it. If the key isn't in that format, an exception is thrown. Typically, this is an ArgumentException, but the overrider can do whatever they please.

 This class cannot override dictionary, because its methods are not override methods, so if this class is cast as a Dictionary object, the wrong methods would be called and the assertions would not be checked.
 */

namespace Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class AssertDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>,
        ICollection<KeyValuePair<TKey, TValue>>,
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        IEnumerable,
        IDictionary,
        ICollection,
        ISerializable,
        IDeserializationCallback
    {
        protected AssertDictionary()
        {
            BaseDictionary = new Dictionary<TKey, TValue>();
        }

        protected AssertDictionary(AssertDictionary<TKey, TValue> dictionary)
        {
            BaseDictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        protected AssertDictionary(IEqualityComparer<TKey> comparer)
        {
            BaseDictionary = new Dictionary<TKey, TValue>(comparer);
        }

        protected AssertDictionary(int capacity)
        {
            BaseDictionary = new Dictionary<TKey, TValue>(capacity);
        }

        protected AssertDictionary(
            AssertDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TKey> comparer)
        {
            BaseDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        protected AssertDictionary(
            int capacity,
            IEqualityComparer<TKey> comparer)
        {
            BaseDictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        protected AssertDictionary(
            SerializationInfo info,
            StreamingContext context)
        {
            BaseDictionary = new SerializationDictionary(info, context);
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

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                return BaseDictionary.Values;
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

        public IEqualityComparer<TKey> Comparer
        {
            get
            {
                return BaseDictionary.Comparer;
            }
        }

        private Dictionary<TKey, TValue> BaseDictionary
        {
            get;
            set;
        }

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

        public bool ContainsKey(TKey key)
        {
            AssertKey(key);
            return BaseDictionary.ContainsKey(key);
        }

        bool IDictionary.Contains(object key)
        {
            if (key is TKey t)
            {
                AssertKey(t);
            }

            return IDictionary.Contains(key);
        }

        public bool ContainsValue(TValue value)
        {
            return BaseDictionary.ContainsValue(value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(
            KeyValuePair<TKey, TValue> item)
        {
            AssertKey(item.Key);
            return IGenericDictionary.Contains(item);
        }

        public void Add(TKey key, TValue value)
        {
            AssertKey(key);
            BaseDictionary.Add(key, value);
        }

        void IDictionary.Add(object key, object value)
        {
            if (key is TKey t)
            {
                AssertKey(t);
            }

            IDictionary.Add(key, value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(
            KeyValuePair<TKey, TValue> item)
        {
            AssertKey(item.Key);
            IGenericDictionary.Add(item);
        }

        public bool Remove(TKey key)
        {
            AssertKey(key);
            return BaseDictionary.Remove(key);
        }

        void IDictionary.Remove(object key)
        {
            if (key is TKey t)
            {
                AssertKey(t);
            }

            IDictionary.Remove(key);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(
            KeyValuePair<TKey, TValue> item)
        {
            AssertKey(item.Key);
            return IGenericDictionary.Remove(item);
        }

        public void Clear()
        {
            BaseDictionary.Clear();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
            KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            IGenericDictionary.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ICollection.CopyTo(array, index);
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

        public virtual void GetObjectData(
            SerializationInfo info,
            StreamingContext context)
        {
            BaseDictionary.GetObjectData(info, context);
        }

        public virtual void OnDeserialization(object sender)
        {
            BaseDictionary.OnDeserialization(sender);
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

        protected abstract void AssertKey(TKey key);

        private protected class SerializationDictionary : Dictionary<TKey, TValue>
        {
            public SerializationDictionary(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}
