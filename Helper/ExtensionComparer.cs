// <copyright file="ExtensionComparer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.IO;

namespace Helper
{
    public sealed class ExtensionComparer : StringModifierComparer
    {
        private static readonly ExtensionComparer Default = new ExtensionComparer();

        public static ExtensionComparer DefaultComparer
        {
            get
            {
                return Default;
            }
        }

        private ExtensionComparer() : this(OrdinalIgnoreCase)
        {
        }

        public ExtensionComparer(StringComparer baseComparer) : base(baseComparer)
        {
        }

        public override string StringModifier(string value)
        {
            return Path.GetExtension(value) ??
throw new ArgumentNullException(nameof(BaseComparer));
        }
    }
}
