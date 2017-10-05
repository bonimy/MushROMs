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
    public class ExtensionComparer : StringComparer
    {
        /// <summary>
        /// Gets a new instance of the default <see cref="ExtensionComparer"/> class.
        /// </summary>
        /// <remarks>
        /// Each call to <see cref="DefaultComparer"/> returns a new instance. Therefore, be sure
        /// to only call it once and assign it to a variable, then call that variable to continue
        /// referencing the same instance.
        /// </remarks>
        public static ExtensionComparer DefaultComparer => new ExtensionComparer();
        /// <summary>
        /// The <see cref="StringComparer"/> used for the extensions of paths supplied to
        /// this <see cref="ExtensionComparer"/>.
        /// </summary>
        private StringComparer _comparer;

        /// <summary>
        /// Gets the <see cref="StringComparer"/> used for the extensions of paths supplied to
        /// this <see cref="ExtensionComparer"/>.
        /// </summary>
        /// <remarks>
        /// The default value of this property is <see cref="StringComparer.OrdinalIgnoreCase"/>,
        /// but it can be overridden.
        /// </remarks>
        protected virtual StringComparer Comparer => _comparer;
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionComparer"/> class.
        /// </summary>
        protected ExtensionComparer()
        {
            // extension strings are case-insensitive and culture-neutral.
            // This may be different on non-windows systems, however.
            _comparer = OrdinalIgnoreCase;
        }

        /// <summary>
        /// Compares two paths and returns an indication of the relative sort order of their
        /// extensions.
        /// </summary>
        /// <param name="x">
        /// A path to compare to <paramref name="y"/>.
        /// </param>
        /// <param name="y">
        /// A path to compare to <paramref name="x"/>.
        /// </param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>
        /// as shown in the following table.
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <description>Less than zero</description>
        /// <description>
        /// <paramref name="x"/> precedes <paramref name="y"/> based on their extensions.
        /// <para/><para/>-or-<para/>
        /// <paramref name="x"/> is <see langword="null"/> and <paramref name="y"/> is not <see langword="null"/>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>Zero</description>
        /// <description>
        /// <paramref name="x"/> is equal to <paramref name="y"/> based on their extensions.
        /// <para/><para/>-or-<para/>
        /// <paramref name="x"/> and <paramref name="y"/> are both <see langword="null"/>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>Greater than zero</description>
        /// <description>
        /// <paramref name="x"/> follows <paramref name="y"/> based on their extensions.
        /// <para/><para/>-or-<para/>
        /// <paramref name="y"/> is <see langword="null"/> and <paramref name="x"/> is not <see langword="null"/>.
        /// </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="Path.GetExtension(String)"/>
        /// </exception>
        /// <remarks>
        /// This method calls <see cref="Path.GetExtension(String)"/> on <paramref name="x"/>
        /// and <paramref name="y"/> and then compares their values using <see cref="Comparer"/>,
        /// whose default value is <see cref="StringComparer.OrdinalIgnoreCase"/>.
        /// </remarks>
        public override int Compare(string x, string y)
        {
            var ext1 = Path.GetExtension(x);
            var ext2 = Path.GetExtension(y);

            return Comparer.Compare(ext1, ext2);
        }

        /// <summary>
        /// Indicates whether two extensions are equal.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <paramref name="x"/> and <paramref name="y"/> have equal
        /// extensions, or <paramref name="x"/> and <paramref name="y"/> are
        /// <see langword="null"/>; otherwise <see langword="false"/>.
        /// </returns>
        /// <inheritdoc cref="Compare(String, String)"/>
        public override bool Equals(string x, string y)
        {
            var ext1 = Path.GetExtension(x);
            var ext2 = Path.GetExtension(y);

            return Comparer.Equals(ext1, ext2);
        }

        /// <summary>
        /// Gets the hash code of the extension of the specified string.
        /// </summary>
        /// <param name="obj">
        /// A path string.
        /// </param>
        /// <returns>
        /// A 32-bit signed hash code calculated from the path extension of the <paramref name="obj"/>
        /// parameter.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <inheritdoc cref="StringComparer.GetHashCode(String)"/>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="Path.GetExtension(String)"/>
        /// <para/><para/>-or-<para/>
        /// <inheritdoc cref="StringComparer.GetHashCode(String)"/>
        /// </exception>
        /// <remarks>
        /// See <see cref="StringComparer.GetHashCode(String)"/> to understand how an insufficient
        /// memory error can happen.
        /// </remarks>
        public override int GetHashCode(string obj)
        {
            var ext = Path.GetExtension(obj);
            return Comparer.GetHashCode(ext);
        }
    }
}
