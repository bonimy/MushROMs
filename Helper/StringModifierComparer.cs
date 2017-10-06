using System;

namespace Helper
{
    /// <summary>
    /// Represents a <see cref="String"/> comparison operation on strings that are first
    /// modified by the abstract <see cref="StringModifier(String)"/> method before
    /// being compared.
    /// </summary>
    /// <seealso cref="PathComparer"/>
    /// <seealso cref="ExtensionComparer"/>
    /// <seealso cref="ExtensionDictionary{TValue}"/>
    public abstract class StringModifierComparer : StringComparer
    {
        /// <summary>
        /// The <see cref="StringComparer"/> to use when comparing two strings after they
        /// have been altered by the <see cref="StringModifier(String)"/> method.
        /// </summary>
        private StringComparer _baseComparer;

        /// <summary>
        /// Gets the <see cref="StringComparer"/> to use when comparing two strings after they
        /// have been altered by the <see cref="StringModifier(String)"/> method.
        /// </summary>
        public virtual StringComparer BaseComparer => _baseComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringModifierComparer"/> class with
        /// </summary>
        /// <param name="baseComparer">
        /// The <see cref="StringComparer"/> to use as the base.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseComparer"/> is <see langword="null"/>.
        /// </exception>
        protected StringModifierComparer(StringComparer baseComparer)
        {
            _baseComparer = baseComparer ?? throw new ArgumentNullException(nameof(baseComparer));
        }

        public override int Compare(string x, string y)
        {
            x = StringModifier(x);
            y = StringModifier(y);

            return BaseComparer.Compare(x, y);
        }

        public override bool Equals(string x, string y)
        {
            x = StringModifier(x);
            y = StringModifier(y);

            return BaseComparer.Equals(x, y);
        }

        public override int GetHashCode(string obj)
        {
            obj = StringModifier(obj);
            return BaseComparer.GetHashCode(obj);
        }

        public abstract string StringModifier(string value);
    }
}
