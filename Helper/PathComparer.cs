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
    public class PathComparer : StringComparer
    {
        /// <summary>
        /// Gets a new instance of the default <see cref="PathComparer"/> class.
        /// </summary>
        /// <remarks>
        /// Each call to <see cref="DefaultComparer"/> returns a new instance. Therefore, be sure
        /// to only call it once and assign it to a variable, then call that variable to continue
        /// referencing the same instance.
        /// </remarks>
        public static PathComparer DefaultComparer => new PathComparer();
        /// <summary>
        /// The <see cref="StringComparer"/> used for the full paths supplied to
        /// this <see cref="PathComparer"/>.
        /// </summary>
        private StringComparer _comparer;

        /// <summary>
        /// Gets the <see cref="StringComparer"/> used for the full paths supplied to
        /// this <see cref="PathComparer"/>.
        /// </summary>
        /// <remarks>
        /// The default value of this property is <see cref="StringComparer.OrdinalIgnoreCase"/>,
        /// but it can be overridden.
        /// </remarks>
        /// <seealso cref="StringComparer.OrdinalIgnoreCase"/>
        protected virtual StringComparer Comparer => _comparer;
        /// <summary>
        /// Initializes a new instance of the <see cref="PathComparer"/> class.
        /// </summary>
        protected PathComparer()
        {
            _comparer = OrdinalIgnoreCase;
        }

        /// <summary>
        /// Compares two paths and returns an indication of the relative sort order of their
        /// absolute paths.
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
        /// <paramref name="x"/> precedes <paramref name="y"/> based on their absolute paths.
        /// <para/><para/>-or-<para/>
        /// <paramref name="x"/> is <see langword="null"/> and <paramref name="y"/> is not <see langword="null"/>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>Zero</description>
        /// <description>
        /// <paramref name="x"/> is equal to <paramref name="y"/> based on their absolute paths.
        /// <para/><para/>-or-<para/>
        /// <paramref name="x"/> and <paramref name="y"/> are both <see langword="null"/>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>Greater than zero</description>
        /// <description>
        /// <paramref name="x"/> follows <paramref name="y"/> based on their absolute paths.
        /// <para/><para/>-or-<para/>
        /// <paramref name="y"/> is <see langword="null"/> and <paramref name="x"/> is not <see langword="null"/>.
        /// </description>
        /// </item>
        /// </list>
        /// </returns>
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
        /// <remarks>
        /// This method calls <see cref="Path.GetFullPath(String)"/> on <paramref name="x"/>
        /// and <paramref name="y"/> and then compares their values using <see cref="Comparer"/>,
        /// whose default value is <see cref="StringComparer.OrdinalIgnoreCase"/>.
        /// </remarks>
        /// <permission cref="FileIOPermission">
        /// <inheritdoc cref="PathComparer"/>
        /// </permission>
        /// <seealso cref="Path.GetFullPath(String)"/>
        /// <seealso cref="StringComparer.OrdinalIgnoreCase"/>
        public override int Compare(string x, string y)
        {
            var path1 = Path.GetFullPath(x);
            var path2 = Path.GetFullPath(y);

            return Comparer.Compare(path1, path2);
        }

        /// <summary>
        /// Indicates whether two paths point to the same location.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <paramref name="x"/> and <paramref name="y"/> have equal
        /// absolute paths, or <paramref name="x"/> and <paramref name="y"/> are
        /// <see langword="null"/>; otherwise <see langword="false"/>.
        /// </returns>
        /// <inheritdoc cref="Compare(String, String)"/>
        public override bool Equals(string x, string y)
        {
            return Compare(x, y) == 0;
        }

        /// <summary>
        /// Gets the hash code of the full path.
        /// </summary>
        /// <param name="obj">
        /// A path string.
        /// </param>
        /// <returns>
        /// A 32-bit signed hash code calculated from the full path of the <paramref name="obj"/>
        /// parameter.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <inheritdoc cref="StringComparer.GetHashCode(String)"/>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// <para/>-or-<para/>
        /// <inheritdoc cref="StringComparer.GetHashCode(String)"/>
        /// </exception>
        /// <exception cref="SecurityException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// <inheritdoc cref="Path.GetFullPath(String)"/>
        /// </exception>
        /// <remarks>
        /// See <see cref="StringComparer.GetHashCode(String)"/> to understand how an insufficient
        /// memory error can happen.
        /// </remarks>
        /// <seealso cref="Path.GetFullPath(String)"/>
        public override int GetHashCode(string obj)
        {
            var path = Path.GetFullPath(obj);
            return Comparer.GetHashCode(path);
        }
    }
}
