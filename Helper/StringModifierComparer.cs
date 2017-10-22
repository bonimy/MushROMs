// <copyright file="StringModifierComparer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;

namespace Helper
{
    public abstract class StringModifierComparer : StringComparer
    {
        private StringComparer _baseComparer;

        public virtual StringComparer BaseComparer
        {
            get
            {
                return _baseComparer;
            }
        }

        protected StringModifierComparer(StringComparer baseComparer)
        {
            _baseComparer = baseComparer ??
throw new ArgumentNullException(nameof(baseComparer));
        }

        public override sealed int Compare(string x, string y)
        {
            x = StringModifier(x);
            y = StringModifier(y);

            return BaseComparer.Compare(x, y);
        }

        public override sealed bool Equals(string x, string y)
        {
            x = StringModifier(x);
            y = StringModifier(y);

            return BaseComparer.Equals(x, y);
        }

        public override sealed int GetHashCode(string obj)
        {
            obj = StringModifier(obj);
            return BaseComparer.GetHashCode(obj);
        }

        public abstract string StringModifier(string value);
    }
}
