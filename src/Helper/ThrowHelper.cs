// <copyright file="ThrowHelper.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Helper
{
    using System;
    using Helper.Properties;
    using static Helper.SR;

    public static class ThrowHelper
    {
        public static ArgumentOutOfRangeException ValueNotGreaterThan(
            string paramName,
            object value)
        {
            return ValueNotGreaterThan(paramName, value, 0);
        }

        public static ArgumentOutOfRangeException ValueNotGreaterThan(
            string paramName,
            object value,
            object valid)
        {
            var message = ErrorValueNotGreaterThan(
                paramName,
                value,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                message);
        }

        public static ArgumentOutOfRangeException ValueNotGreaterThanEqualTo(
            string paramName,
            object value)
        {
            return ValueNotGreaterThanEqualTo(paramName, value, 0);
        }

        public static ArgumentOutOfRangeException ValueNotGreaterThanEqualTo(
            string paramName,
            object value,
            object valid)
        {
            var message = ErrorValueNotGreaterThanOrEqualTo(
                paramName,
                value,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                message);
        }

        public static ArgumentOutOfRangeException ValueNotLessThan(
            string paramName,
            object value)
        {
            return ValueNotLessThan(paramName, value, 0);
        }

        public static ArgumentOutOfRangeException ValueNotLessThan(
            string paramName,
            object value,
            object valid)
        {
            var message = ErrorValueNotLessThan(
                paramName,
                value,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                message);
        }

        public static ArgumentOutOfRangeException ValueNotLessThanEqualTo(
            string paramName,
            object value)
        {
            return ValueNotLessThanEqualTo(paramName, value, 0);
        }

        public static ArgumentOutOfRangeException ValueNotLessThanEqualTo(
            string paramName,
            object value,
            object valid)
        {
            var message = ErrorValueNotLessThanOrEqualTo(
                paramName,
                value,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                message);
        }

        public static ArgumentOutOfRangeException ValueNotInArrayBounds(
            string paramName,
            int value,
            int arraySize)
        {
            var message = ErrorValueNotInArrayBounds(
                paramName,
                value,
                arraySize);

            throw new ArgumentOutOfRangeException(
                paramName,
                message);
        }

        public static ArgumentException ValueIsNaN(string paramName)
        {
            var message = ErrorValueIsNaN(paramName);

            throw new ArgumentException(
                message,
                paramName);
        }

        public static ArgumentException ValueIsInfinite(string paramName)
        {
            var message = ErrorValueIsInfinite(paramName);

            throw new ArgumentException(
                message,
                paramName);
        }

        private static string ErrorValueNotGreaterThanOrEqualTo(
            string paramName,
            object value,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotGreaterThanOrEqualTo,
                paramName,
                value,
                valid);
        }

        private static string ErrorValueNotLessThanOrEqualTo(
            string paramName,
            object value,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotLessThanOrEqualTo,
                paramName,
                value,
                valid);
        }

        private static string ErrorValueNotGreaterThan(
            string paramName,
            object value,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotGreaterThan,
                paramName,
                value,
                valid);
        }

        private static string ErrorValueNotLessThan(
            string paramName,
            object value,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotLessThan,
                paramName,
                value,
                valid);
        }

        private static string ErrorValueNotInArrayBounds(
            string paramName,
            int value,
            int arraySize)
        {
            return GetString(
                Resources.ErrorValueNotInArrayBounds,
                paramName,
                value,
                arraySize);
        }

        private static string ErrorStringSubstringSize(
            string paramName,
            int startIndex,
            int length)
        {
            return GetString(
                Resources.ErrorStringSubstringSize,
                paramName,
                startIndex,
                length);
        }

        private static string ErrorSubstringPointerLength(
            string paramName,
            int value)
        {
            return GetString(
                Resources.ErrorSubstringPointerLength,
                paramName,
                value);
        }

        private static string ErrorValueIsNaN(string paramName)
        {
            return GetString(Resources.ErrorValueIsNaN, paramName);
        }

        private static string ErrorValueIsInfinite(string paramName)
        {
            return GetString(Resources.ErrorValueIsInfinity, paramName);
        }
    }
}
