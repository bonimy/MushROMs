using System;
using System.IO;

namespace Helper
{
    /// <summary>
    /// Represents a <see cref="String"/> comparison operation that performs a comparison of the extensions
    /// of paths.
    /// </summary>
    /// <seealso cref="Path.GetExtension(String)"/>
    /// <seealso cref="StringComparer.OrdinalIgnoreCase"/>
    /// <seealso cref="PathComparer"/>
    /// <seealso cref="ExtensionDictionary{TValue}"/>
    public class ExtensionComparer : StringModifierComparer
    {
        private static readonly ExtensionComparer _default = new ExtensionComparer();

        /// <summary>
        /// Gets a new instance of the default <see cref="ExtensionComparer"/> class.
        /// </summary>
        /// <returns>
        /// A new <see cref="ExtensionComparer"/> object.
        /// </returns>
        public static ExtensionComparer DefaultComparer => _default;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionComparer"/> class and uses
        /// <see cref="StringComparer.OrdinalIgnoreCase"/> as the base comparer for the path
        /// extensions.
        /// </summary>
        protected ExtensionComparer() : this(OrdinalIgnoreCase)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionComparer"/> using the given
        /// base comparer for the string extenions.
        /// </summary>
        /// <param name="baseComparer">
        /// The <see cref="StringComparer"/> to use when comparing extensions.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseComparer"/> is <c>null</c>.
        /// </exception>
        public ExtensionComparer(StringComparer baseComparer) : base(baseComparer)
        { }

        public override string StringModifier(string value)
        {
            return Path.GetExtension(value) ?? throw new ArgumentNullException(nameof(BaseComparer));
        }
    }
}
