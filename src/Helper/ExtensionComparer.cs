// <copyright file="ExtensionComparer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.IO;

namespace Helper
{
    public sealed class ExtensionComparer : StringModifierComparer
    {
        public static readonly ExtensionComparer DefaultComparer = new ExtensionComparer();

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
