using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Helper
{
    /// <summary>
    /// Represents a collection of keys and values where the keys are file path locations.
    /// </summary>
    /// <inheritdoc/>
    /// <remarks>
    /// This dictionary uses <see cref="PathComparer"/> as its equality comparer. Keys can only be
    /// relative or full paths that the user has permission to access. When
    /// </remarks>
    /// <seealso cref="Path.GetFullPath(String)"/>
    /// <seealso cref="PathComparer"/>
    /// <seealso cref="ExtensionDictionary{TValue}"/>
    public class PathDictionary<TValue> : AssertDictionary<string, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(IEqualityComparer{TKey})"/> as its base and passes
        /// <see cref="PathComparer.DefaultComparer"/> as the equality comparer.
        /// </summary>
        /// <inheritdoc/>
        public PathDictionary() :
            base(PathComparer.DefaultComparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(Int32, IEqualityComparer{TKey})"/> as its base and passes
        /// <see cref="PathComparer.DefaultComparer"/> as the equality comparer.
        /// </summary>
        /// <inheritdoc/>
        public PathDictionary(int capacity) :
            base(capacity, PathComparer.DefaultComparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(IEqualityComparer{TKey})"/> as its base.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="PathComparer"/> to use when comparing keys, or <see langword="null"/> to use
        /// <see cref="PathComparer.DefaultComparer"/>.
        /// </param>
        /// <inheritdoc/>
        public PathDictionary(PathComparer comparer) :
            base(comparer ?? PathComparer.DefaultComparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(AssertDictionary{TKey, TValue}, IEqualityComparer{TKey})"/>
        /// as its base and passes <see cref="PathComparer.DefaultComparer"/> as the equality comparer.
        /// </summary>
        /// <param name="dictionary">
        /// The <see cref="PathDictionary{TValue}"/> whose elements are copied to the new <see cref="PathDictionary{TValue}"/>.
        /// </param>
        /// <inheritdoc/>
        public PathDictionary(PathDictionary<TValue> dictionary) :
            base(dictionary, PathComparer.DefaultComparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(Int32, IEqualityComparer{TKey})"/> as its base.
        /// </summary>
        /// <param name="capacity">
        /// <inheritdoc cref="PathDictionary{TValue}(Int32)"/>
        /// </param>
        /// <param name="comparer">
        /// <inheritdoc cref="PathDictionary{TValue}(PathComparer)"/>
        /// </param>
        /// <inheritdoc/>
        public PathDictionary(int capacity, PathComparer comparer) :
            base(capacity, comparer ?? PathComparer.DefaultComparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(AssertDictionary{TKey, TValue}, IEqualityComparer{TKey})"/>
        /// as its base.
        /// </summary>
        /// <param name="dictionary">
        /// <inheritdoc cref="PathDictionary{TValue}(PathDictionary{TValue})"/>
        /// </param>
        /// <param name="comparer">
        /// <inheritdoc cref="PathDictionary{TValue}(PathComparer)"/>
        /// </param>
        /// <inheritdoc/>
        public PathDictionary(PathDictionary<TValue> dictionary, ExtensionComparer comparer) :
            base(dictionary, comparer)
        {
        }

        /// <summary>
        /// Asserts that a key is formatted to obtain a full path, and that the user has sufficient
        /// permission to access the path.
        /// </summary>
        /// <param name="key">
        /// <inheritdoc cref="AssertDictionary{TKey, TValue}.AssertKey(TKey)"/>
        /// </param>
        /// <remarks>
        /// When comparing paths, <see cref="Path.GetFullPath(String)"/> is called. We test now that
        /// the method will let us get the path before adding it to the dictionary.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// </exception>
        /// <exception cref="SecurityException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// </exception>
        protected override void AssertKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

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
