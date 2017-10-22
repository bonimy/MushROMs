// <copyright file="PathComparer.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.IO;

namespace Helper
{
    public sealed class PathComparer : StringModifierComparer
    {
        private static readonly PathComparer Default = new PathComparer();

        public static PathComparer DefaultComparer
        {
            get
            {
                return Default;
            }
        }

        private PathComparer() : this(OrdinalIgnoreCase)
        {
        }

        public PathComparer(StringComparer baseComparer) : base(baseComparer)
        {
        }

        public override string StringModifier(string value)
        {
            return Path.GetFullPath(value) ??
throw new ArgumentNullException(nameof(BaseComparer));
        }
    }
}
