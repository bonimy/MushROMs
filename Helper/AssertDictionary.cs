using System.Collections.Generic;

namespace Helper
{
    /// <summary>
    /// A dictionary class that runs an assert method on keys before using them.
    /// </summary>
    /// <inheritdoc/>
    /// <remarks>
    /// The <see cref="AssertDictionary{TKey, TValue}"/> class overrides the <see cref="this[TKey]"/>, <see cref="Add(TKey, TValue)"/>,
    /// <see cref="ContainsKey(TKey)"/>, <see cref="Remove(TKey)"/>, <see cref="TryGetValue(TKey, out TValue)"/> methods with
    /// a prophylactic call to the abstract <see cref="AssertKey(TKey)"/> method. When overridden, <see cref="AssertKey(TKey)"/>
    /// serves to test that the passed key value is a well-formatted key. This prevents keys that, while of the type <typeparamref name="TKey"/>,
    /// do not enter the <see cref="AssertDictionary{TKey, TValue}"/> collection if they are not well-formed.
    /// <para/><para/>
    /// For additional information, examples, and sample code on dictionaries, see <see cref="Dictionary{TKey, TValue}"/>.
    /// </remarks>
    /// <seealso cref="AssertKey(TKey)"/>
    /// <seealso cref="Dictionary{TKey, TValue}"/>
    public abstract class AssertDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertDictionary{TKey, TValue}"/> class. This constructor calls
        /// <see cref="Dictionary{TKey, TValue}()"/> as its base.
        /// </summary>
        /// <inheritdoc/>
        /// <remarks>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}()"/>.
        /// </remarks>
        /// <overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// </overloads>
        /// <seealso cref="Dictionary{TKey, TValue}()"/>
        protected AssertDictionary() :
            base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertDictionary{TKey, TValue}"/> class. This constructor calls
        /// <see cref="Dictionary{TKey, TValue}(Int32)"/> as its base constructor.
        /// </summary>
        /// <inheritdoc/>
        /// <remarks>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}(Int32)"/>.
        /// </remarks>
        /// <seealso cref="Dictionary{TKey, TValue}(Int32)"/>
        protected AssertDictionary(int capacity) :
            base(capacity)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertDictionary{TKey, TValue}"/> class. This constructor calls
        /// <see cref="Dictionary{TKey, TValue}(IEqualityComparer{TKey})"/> as its base constructor.
        /// </summary>
        /// <inheritdoc/>
        /// <remarks>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}(IEqualityComparer{TKey})"/>.
        /// </remarks>
        /// <seealso cref="Dictionary{TKey, TValue}(IEqualityComparer{TKey})"/>
        protected AssertDictionary(IEqualityComparer<TKey> comparer) :
            base(comparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertDictionary{TKey, TValue}"/> class. This constructor calls
        /// <see cref="Dictionary{TKey, TValue}(IDictionary{TKey, TValue})"/> as its base constructor.
        /// </summary>
        /// <param name="dictionary">
        /// The <see cref="AssertDictionary{TKey, TValue}"/> whose elements are copied to the new <see cref="AssertDictionary{TKey, TValue}"/>.
        /// None of the values in <paramref name="dictionary"/> are tested against this instance's <see cref="AssertKey(TKey)"/>
        /// method, so it is advised that this instance of <see cref="AssertDictionary{TKey, TValue}"/> has the same overloading
        /// type as <paramref name="dictionary"/>.
        /// </param>
        /// <inheritdoc/>
        /// <remarks>
        /// It is important to ensure that the overloaded type of <paramref name="dictionary"/> has the same
        /// <see cref="AssertKey(TKey)"/> implementation (or a subset assertion) of this
        /// <see cref="AssertDictionary{TKey, TValue}"/> overload. If the overloaded type of
        /// <paramref name="dictionary"/> has an <see cref="AssertKey(TKey)"/> method that accepts keys that this
        /// instance would reject, then it causes undefined behavior.
        /// <para/><para/>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}(IDictionary{TKey, TValue})"/>.
        /// <para/>
        /// This problem cannot be solved by calling <see cref="AssertKey(TKey)"/> in this constructor because the method
        /// is marked <see langword="abstract"/>. See <see href="https://msdn.microsoft.com/en-us/library/ms182331.aspx">CA2214</see> for
        /// more information on why virtual methods should not be called in their class's constructors.
        /// </remarks>
        /// <seealso cref="Dictionary{TKey, TValue}(IDictionary{TKey, TValue})"/>
        protected AssertDictionary(AssertDictionary<TKey, TValue> dictionary) :
            base(dictionary)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertDictionary{TKey, TValue}"/> class. This constructor calls
        /// <see cref="Dictionary{TKey, TValue}(Int32, IEqualityComparer{TKey})"/> as its base constructor.
        /// </summary>
        /// <inheritdoc/>
        /// <remarks>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}(Int32, IEqualityComparer{TKey})"/>.
        /// </remarks>
        /// <seealso cref="Dictionary{TKey, TValue}(Int32, IEqualityComparer{TKey})"/>
        protected AssertDictionary(int capacity, IEqualityComparer<TKey> comparer) :
            base(capacity, comparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertDictionary{TKey, TValue}"/> class. This constructor calls
        /// <see cref="Dictionary{TKey, TValue}(IDictionary{TKey, TValue}, IEqualityComparer{TKey})"/> as its base constructor.
        /// </summary>
        /// <param name="dictionary">
        /// <inheritdoc cref="AssertDictionary{TKey, TValue}(AssertDictionary{TKey, TValue})"/>
        /// </param>
        /// <param name="comparer">
        /// <inheritdoc cref="AssertDictionary{TKey, TValue}(IEqualityComparer{TKey})"/>
        /// </param>
        /// <inheritdoc cref="AssertDictionary{TKey, TValue}(AssertDictionary{TKey, TValue})" select="remarks"/>
        /// <seealso cref="Dictionary{TKey, TValue}(IDictionary{TKey, TValue}, IEqualityComparer{TKey})"/>
        protected AssertDictionary(AssertDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) :
            base(dictionary, comparer)
        {
        }

        /// <inheritdoc cref="P:System.Collections.Generic.Dictionary`2.Item(`0)"/>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="AssertKey(TKey)"/>
        /// </exception>
        /// <remarks>
        /// Before calling this method, <paramref name="key"/> is passed to <see cref="AssertKey(TKey)"/>. If
        /// <paramref name="key"/> is not well-formed, then <see cref="AssertKey(TKey)"/> throws an <see cref="ArgumentException"/>.
        /// </remarks>
        /// <seealso cref="P:System.Collections.Generic.Dictionary`2.Item(`0)"/>
        public new TValue this[TKey key]
        {
            get
            {
                AssertKey(key);
                return base[key];
            }
            set
            {
                AssertKey(key);
                base[key] = value;
            }
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
        /// <remarks>
        /// <inheritdoc cref="this[TKey]"/>
        /// <para/><para/>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
        /// <para/><para/>
        /// -or-
        /// <para/>
        /// <inheritdoc cref="AssertKey(TKey)"/>
        /// </exception>
        /// <seealso cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
        public new void Add(TKey key, TValue value)
        {
            AssertKey(key);
            base.Add(key, value);
        }
        /// <inheritdoc cref="Dictionary{TKey, TValue}.ContainsKey(TKey)"/>
        /// <remarks>
        /// <inheritdoc cref="this[TKey]"/>
        /// <para/><para/>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}.ContainsKey(TKey)"/>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="AssertKey(TKey)"/>
        /// </exception>
        /// <seealso cref="Dictionary{TKey, TValue}.ContainsKey(TKey)"/>
        public new bool ContainsKey(TKey key)
        {
            AssertKey(key);
            return base.ContainsKey(key);
        }
        /// <inheritdoc cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        /// <remarks>
        /// <inheritdoc cref="this[TKey]"/>
        /// <para/><para/>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}.Remove(TKey)"/>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="AssertKey(TKey)"/>
        /// </exception>
        /// <seealso cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
        public new bool Remove(TKey key)
        {
            AssertKey(key);
            return base.Remove(key);
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
        /// <remarks>
        /// <inheritdoc cref="this[TKey]"/>
        /// <para/><para/>
        /// For additional information, examples, and sample code, see <see cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="AssertKey(TKey)"/>
        /// </exception>
        /// <seealso cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
        public new bool TryGetValue(TKey key, out TValue value)
        {
            AssertKey(key);
            return base.TryGetValue(key, out value);
        }

        /// <summary>
        /// When overridden in a derived class, asserts that a key is well-formed before adding it to this
        /// <see cref="AssertDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">
        /// The key to test
        /// </param>
        /// <remarks>
        /// Use this method to ensure that <paramref name="key"/> is structured in a way that this
        /// <see cref="AssertDictionary{TKey, TValue}"/> would demand. If the overload of this method encounters
        /// an unsatisfactory <paramref name="key"/>, then the overload should through an <see cref="ArgumentException"/>
        /// (or one of its overloaded exceptions) specifying the specific error of <paramref name="key"/>.
        /// </remarks>
        /// <seealso cref="PathDictionary{TValue}.AssertKey(String)"/>
        /// <seealso cref="ExtensionDictionary{TValue}.AssertKey(String)"/>
        /// <exception cref="ArgumentException">
        /// <paramref name="key"/> is not formatted correctly for this <see cref="AssertDictionary{TKey, TValue}"/>.
        /// </exception>
        protected abstract void AssertKey(TKey key);
    }
}
