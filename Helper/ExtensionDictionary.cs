// <copyright file="ExtensionDictionary.cs>
//     Copyright (c) 2017 Nelson Garcia
// </copyright>

using System;

namespace Helper
{
    /// <summary>
    /// Represents a collection of keys and values where the keys are file extensions.
    /// </summary>
    /// <inheritdoc/>
    /// <remarks>
    /// This dictionary uses <see cref="ExtensionComparer"/> as its equality comparer. Keys can only be
    /// file extensions, or paths with file extensions.
    /// </remarks>
    /// <example>
    /// The following example shows the basic use of an <see cref="ExtensionDictionary{TValue}"/>. The code example
    /// initializes an <see cref="ExtensionDictionary{TValue}"/> with the default constructor and populates it with sample
    /// extensions and the programs that should open them. It then iterates through an array of file paths, passes the
    /// full path name to the <see cref="ExtensionDictionary{TValue}"/> (without having to call <see cref="Path.GetExtension(String)"/>
    /// first), and the prints which program opens each file.
    /// <code language="cs" title="Example" source="..\Examples.Helper\ExtensionDictionary.cs" region="example"/>
    /// </example>
    /// <seealso cref="Path.GetExtension(String)"/>
    /// <seealso cref="ExtensionComparer"/>
    /// <seealso cref="PathDictionary{TValue}"/>
    public class ExtensionDictionary<TValue> : AssertDictionary<string, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionComparer"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(IEqualityComparer{TKey})"/> as its base and passes
        /// <see cref="ExtensionComparer.DefaultComparer"/> as the equality comparer.
        /// </summary>
        /// <inheritdoc/>
        public ExtensionDictionary() :
            base(ExtensionComparer.DefaultComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(Int32, IEqualityComparer{TKey})"/> as its base and passes
        /// <see cref="ExtensionComparer.DefaultComparer"/> as the equality comparer.
        /// </summary>
        /// <inheritdoc/>
        public ExtensionDictionary(int capacity) :
            base(capacity, ExtensionComparer.DefaultComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(IEqualityComparer{TKey})"/> as its base.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="ExtensionComparer"/> to use when comparing keys, or <see langword="null"/> to use
        /// <see cref="ExtensionComparer.DefaultComparer"/>.
        /// </param>
        /// <inheritdoc/>
        public ExtensionDictionary(ExtensionComparer comparer) :
            base(comparer ?? throw new ArgumentNullException(nameof(comparer)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(AssertDictionary{TKey, TValue}, IEqualityComparer{TKey})"/>
        /// as its base and passes <see cref="ExtensionComparer.DefaultComparer"/> as the equality comparer.
        /// </summary>
        /// <param name="dictionary">
        /// The <see cref="ExtensionDictionary{TValue}"/> whose elements are copied to the new <see cref="ExtensionDictionary{TValue}"/>.
        /// </param>
        /// <inheritdoc/>
        public ExtensionDictionary(ExtensionDictionary<TValue> dictionary) :
            base(dictionary, ExtensionComparer.DefaultComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(Int32, IEqualityComparer{TKey})"/> as its base.
        /// </summary>
        /// <param name="capacity">
        /// <inheritdoc cref="ExtensionDictionary{TValue}(Int32)"/>
        /// </param>
        /// <param name="comparer">
        /// <inheritdoc cref="ExtensionDictionary{TValue}(ExtensionComparer)"/>
        /// </param>
        /// <inheritdoc/>
        public ExtensionDictionary(int capacity, ExtensionComparer comparer) :
            base(capacity, comparer ?? ExtensionComparer.DefaultComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionDictionary{TValue}"/> class. This constructor uses
        /// <see cref="AssertDictionary{TKey, TValue}(AssertDictionary{TKey, TValue}, IEqualityComparer{TKey})"/>
        /// as its base.
        /// </summary>
        /// <param name="dictionary">
        /// <inheritdoc cref="ExtensionDictionary{TValue}(ExtensionDictionary{TValue})"/>
        /// </param>
        /// <param name="comparer">
        /// <inheritdoc cref="ExtensionDictionary{TValue}(ExtensionComparer)"/>
        /// </param>
        /// <inheritdoc/>
        public ExtensionDictionary(ExtensionDictionary<TValue> dictionary, ExtensionComparer comparer) :
            base(dictionary, comparer ?? ExtensionComparer.DefaultComparer)
        {
        }

        /// <summary>
        /// Asserts that a key has an extension before adding it to this <see cref="ExtensionDictionary{TValue}"/>.
        /// </summary>
        /// <param name="key">
        /// <inheritdoc cref="AssertDictionary{TKey, TValue}.AssertKey(TKey)"/>
        /// </param>
        /// <remarks>
        /// When comparing extensions, <see cref="ExtensionComparer.StringModifier(String)"/> is called.
        /// We test now that it throws an exception before adding it to the dictionary and causing
        /// problems down the road.
        /// </remarks>
        protected override void AssertKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

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
