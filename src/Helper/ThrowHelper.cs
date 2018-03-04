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
            object actualValue)
        {
            return ValueNotGreaterThan(
                paramName,
                actualValue,
                0);
        }

        public static ArgumentOutOfRangeException ValueNotGreaterThan(
            string paramName,
            object actualValue,
            object valid)
        {
            var message = ErrorValueNotGreaterThan(
                paramName,
                actualValue,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        public static ArgumentOutOfRangeException
            ValueNotGreaterThanEqualTo(
            string paramName,
            object actualValue)
        {
            return ValueNotGreaterThanEqualTo(
                paramName,
                actualValue,
                0);
        }

        public static ArgumentOutOfRangeException
            ValueNotGreaterThanEqualTo(
            string paramName,
            object actualValue,
            object valid)
        {
            var message = ErrorValueNotGreaterThanOrEqualTo(
                paramName,
                actualValue,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        public static ArgumentOutOfRangeException ValueNotLessThan(
            string paramName,
            object actualValue)
        {
            return ValueNotLessThan(
                paramName,
                actualValue,
                0);
        }

        public static ArgumentOutOfRangeException ValueNotLessThan(
            string paramName,
            object actualValue,
            object valid)
        {
            var message = ErrorValueNotLessThan(
                paramName,
                actualValue,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        public static ArgumentOutOfRangeException
            ValueNotLessThanEqualTo(
            string paramName,
            object actualValue)
        {
            return ValueNotLessThanEqualTo(
                paramName,
                actualValue,
                0);
        }

        public static ArgumentOutOfRangeException
            ValueNotLessThanEqualTo(
            string paramName,
            object actualValue,
            object valid)
        {
            var message = ErrorValueNotLessThanOrEqualTo(
                paramName,
                actualValue,
                valid);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        public static ArgumentOutOfRangeException
            ValueNotInArrayBounds(
            string paramName,
            int actualValue,
            int arraySize)
        {
            var message = ErrorValueNotInArrayBounds(
                paramName,
                actualValue,
                arraySize);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        public static ArgumentException ValueIsNaN(
            string paramName)
        {
            var message = ErrorValueIsNaN(paramName);

            return new ArgumentException(
                message,
                paramName);
        }

        public static ArgumentException ValueIsInfinite(
            string paramName)
        {
            var message = ErrorValueIsInfinite(
                paramName);

            return new ArgumentException(
                message,
                paramName);
        }

        public static ArgumentOutOfRangeException
            InvalidSubstringInfoParameter(
            string paramName,
            object actualValue,
            object specialValue)
        {
            return InvalidSubstringInfoParameter(
                paramName,
                actualValue,
                0,
                specialValue);
        }

        public static ArgumentOutOfRangeException
            InvalidSubstringInfoParameter(
            string paramName,
            object actualValue,
            object valid,
            object specialValue)
        {
            var message = ErrorSubstringInfoParameter(
                paramName,
                actualValue,
                valid,
                specialValue);

            return new ArgumentOutOfRangeException(
                paramName,
                actualValue,
                message);
        }

        private static string ErrorValueNotGreaterThanOrEqualTo(
            string paramName,
            object actualValue,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotGreaterThanOrEqualTo,
                paramName,
                actualValue,
                valid);
        }

        private static string ErrorValueNotLessThanOrEqualTo(
            string paramName,
            object actualValue,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotLessThanOrEqualTo,
                paramName,
                actualValue,
                valid);
        }

        private static string ErrorValueNotGreaterThan(
            string paramName,
            object actualValue,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotGreaterThan,
                paramName,
                actualValue,
                valid);
        }

        private static string ErrorValueNotLessThan(
            string paramName,
            object actualValue,
            object valid)
        {
            return GetString(
                Resources.ErrorValueNotLessThan,
                paramName,
                actualValue,
                valid);
        }

        private static string ErrorValueNotInArrayBounds(
            string paramName,
            int actualValue,
            int arraySize)
        {
            return GetString(
                Resources.ErrorValueNotInArrayBounds,
                paramName,
                actualValue,
                arraySize);
        }

        private static string ErrorValueIsNaN(
            string paramName)
        {
            return GetString(
                Resources.ErrorValueIsNaN,
                paramName);
        }

        private static string ErrorValueIsInfinite(
            string paramName)
        {
            return GetString(
                Resources.ErrorValueIsInfinity,
                paramName);
        }

        private static string ErrorSubstringInfoParameter(
            string paramName,
            object actualValue,
            object valid,
            object specialValue)
        {
            return GetString(
                Resources.ErrorSubstringInfoParameter,
                paramName,
                actualValue,
                valid,
                specialValue);
        }
    }
}
