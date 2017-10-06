using System;

namespace Helper
{
    public abstract class StringModifierComparer : StringComparer
    {
        private StringComparer _baseComparer;

        public virtual StringComparer BaseComparer => _baseComparer;

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
