// <copyright file="ErrorCodeException.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.Runtime.Serialization;
using System.Security;
using Controls.Properties;
using Helper;

namespace Controls
{
    [Serializable]
    public class ErrorCodeException : Exception
    {
        public int ErrorCode
        {
            get;
        }

        public ErrorCodeException() :
            this(UnsafeNativeMethods.LastWin32Error)
        {
        }

        public ErrorCodeException(int errorCode) :
            this(errorCode, GetErrorCodeMessage(errorCode))
        {
        }

        public ErrorCodeException(int errorCode, string message) :
            base(message)
        {
            ErrorCode = errorCode;
        }

        public ErrorCodeException(string message) :
            base(message)
        {
            ErrorCode = UnsafeNativeMethods.LastWin32Error;
        }

        public ErrorCodeException(string message, Exception innerException) :
            base(message, innerException)
        {
            ErrorCode = UnsafeNativeMethods.LastWin32Error;
        }

        protected ErrorCodeException(
            SerializationInfo info,
            StreamingContext context) :
            base(info, context)
        {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            ErrorCode = info.GetInt32(nameof(ErrorCode));
        }

        private static string GetErrorCodeMessage(int value)
        {
            return SR.GetString(Resources.ErrorCodeMessage, (uint)value);
        }

        [SecurityCritical]
        public override void GetObjectData(
            SerializationInfo info,
            StreamingContext context)
        {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            base.GetObjectData(info, context);
            info.AddValue(nameof(ErrorCode), ErrorCode, ErrorCode.GetType());
        }
    }
}
