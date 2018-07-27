// <copyright file="StringHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using System.Globalization;

    public static class StringHelper
    {
        private static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        private static CultureInfo CurrentUICulture
        {
            get
            {
                return CultureInfo.CurrentUICulture;
            }
        }

        public static string GetString(
            string format,
            params object[] args)
        {
            return String.Format(CurrentCulture, format, args);
        }

        public static string GetString(IFormattable value)
        {
            return GetString(value, null);
        }

        public static string GetString(
            IFormattable value,
            string format)
        {
            if (value is null)
            {
                return null;
            }

            return value.ToString(format, CurrentCulture);
        }

        public static string GetUIString(
            string format,
            params object[] args)
        {
            return String.Format(CurrentUICulture, format, args);
        }

        public static string GetUIString(IFormattable value)
        {
            return GetUIString(value, null);
        }

        public static string GetUIString(
            IFormattable value,
            string format)
        {
            if (value is null)
            {
                return null;
            }

            return value.ToString(format, CurrentUICulture);
        }
    }
}
