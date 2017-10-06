using System;
using System.IO;

namespace Helper
{
    /// <summary>
    /// Represents a <see cref="String"/> comparison operation that performs a comparison of the full path
    /// of two strings.
    /// </summary>
    /// <seealso cref="Path.GetFullPath(String)"/>
    /// <seealso cref="StringComparer.OrdinalIgnoreCase"/>
    /// <seealso cref="ExtensionComparer"/>
    /// <seealso cref="PathDictionary{TValue}"/>
    /// <permission cref="FileIOPermission">
    /// for access to the path.
    /// </permission>
    public class PathComparer : StringModifierComparer
    {
        /// <summary>
        /// Gets a new instance of the default <see cref="PathComparer"/> class.
        /// </summary>
        /// <returns>
        /// A new <see cref="DefaultComparer"/> object.
        /// </returns>
        public static PathComparer DefaultComparer => new PathComparer();

        /// <summary>
        /// Initializes a new instance of the <see cref="PathComparer"/> class.
        /// </summary>
        protected PathComparer() : this(OrdinalIgnoreCase)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathComparer"/> using the given
        /// base comparer for the string extenions.
        /// </summary>
        /// <param name="baseComparer">
        /// The <see cref="StringComparer"/> to use when comparing extensions.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseComparer"/> is <c>null</c>.
        /// </exception>
        public PathComparer(StringComparer baseComparer) : base(baseComparer)
        { }

        public override string StringModifier(string value)
        {
            return Path.GetFullPath(value) ?? throw new ArgumentNullException(nameof(BaseComparer));
        }
    }
}
