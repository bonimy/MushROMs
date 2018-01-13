﻿// <copyright file="SR.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Globalization;
using System.IO;
using Helper.Properties;

namespace Helper
{
    public static class SR
    {
        public static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        public static string ErrorInt24Overflow
        {
            get
            {
                return Resources.ErrorInt24Overflow;
            }
        }

        public static string ErrorUInt24Overflow
        {
            get
            {
                return Resources.ErrorUInt24Overflow;
            }
        }

        public static string ErrorLowerBoundInclusive(
            string paramName,
            object value,
            object valid)
        {
            return GetString(
                Resources.ErrorLowerBoundInclusive,
                paramName,
                value,
                valid);
        }

        public static string ErrorUpperBoundInclusive(
            string paramName,
            object value,
            object valid)
        {
            return GetString(
                Resources.ErrorUpperBoundInclusive,
                paramName,
                value,
                valid);
        }

        public static string ErrorLowerBoundExclusive(
            string paramName,
            object value,
            object valid)
        {
            return GetString(
                Resources.ErrorLowerBoundExclusive,
                paramName,
                value,
                valid);
        }

        public static string ErrorUpperBoundExclusive(
            string paramName,
            object value,
            object valid)
        {
            return GetString(
                Resources.ErrorUpperBoundExclusive,
                paramName,
                value,
                valid);
        }

        public static string ErrorArrayBounds(
            string paramName,
            int value,
            int valid)
        {
            return GetString(Resources.ErrorArrayBounds, paramName, value, valid);
        }

        public static string ErrorArrayRange(
            string paramName,
            int value,
            string arrayName,
            int arrayLength,
            int startIndex)
        {
            return GetString(
                Resources.ErrorArrayRange,
                paramName,
                value,
                arrayName,
                arrayLength,
                startIndex);
        }

        public static string ErrorEmptyOrNullArray(string paramName)
        {
            return GetString(Resources.ErrorEmptyOrNullArray, paramName);
        }

        public static string ErrorFileFormat(string path)
        {
            var name = Path.GetFileName(path);
            if (String.IsNullOrEmpty(name))
            {
                name = Resources.ErrorFileFormatName;
            }

            return GetString(Resources.ErrorFileFormat, name);
        }

        internal static string ErrorStringSubstringSize(
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

        internal static string ErrorSubstringPointerLength(
            string paramName,
            int value)
        {
            return GetString(
                Resources.ErrorSubstringPointerLength,
                paramName,
                value);
        }

        public static string ErrorValueIsNaN(string paramName)
        {
            return GetString(Resources.ErrorValueIsNaN, paramName);
        }

        public static string ErrorValueIsInfinite(string paramName)
        {
            return GetString(Resources.ErrorValueIsInfinity, paramName);
        }

        public static string ErrorCannotGetPathExtension(
            string paramName,
            string value,
            Exception innerException)
        {
            if (innerException == null)
            {
                throw new ArgumentNullException(nameof(innerException));
            }

            return GetString(
                Resources.ErrorCannotGetPathExtension,
                paramName,
                value,
                innerException.Message,
                innerException.GetType());
        }

        public static string ErrorCannotGetFullPath(
            string paramName,
            string value,
            Exception innerException)
        {
            if (innerException == null)
            {
                throw new ArgumentNullException(nameof(innerException));
            }

            return GetString(
                Resources.ErrorCannotGetFullPath,
                paramName,
                value,
                innerException.Message,
                innerException.GetType());
        }

        public static string ErrorInvalidExtensionName(string ext)
        {
            return GetString(Resources.ErrorInvalidExtensionName, ext);
        }

        public static string ErrorInvalidPathName(string path)
        {
            return GetString(Resources.ErrorInvalidPathName, path);
        }

        public static string GetUntitledName(int number, string ext)
        {
            if (ext == null)
            {
                ext = String.Empty;
            }

            return GetString(Resources.UntitledName, GetString(number)) + ext;
        }

        public static string GetString(string format, params object[] args)
        {
            return String.Format(CurrentCulture, format, args);
        }

        public static string GetString(IFormattable value)
        {
            return GetString(value, null);
        }

        public static string GetString(IFormattable value, string format)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString(format, CurrentCulture);
        }
    }
}
