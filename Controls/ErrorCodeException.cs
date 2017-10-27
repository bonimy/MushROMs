// <copyright file="ErrorCodeException.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
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
            private set;
        }

        public ErrorCodeException() :
            this(Marshal.GetLastWin32Error())
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
            ErrorCode = Marshal.GetLastWin32Error();
        }

        public ErrorCodeException(string message, Exception innerException) :
            base(message, innerException)
        {
            ErrorCode = Marshal.GetLastWin32Error();
        }

        protected ErrorCodeException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            ErrorCode = info.GetInt32(nameof(ErrorCode));
        }

        private static string GetErrorCodeMessage(int value)
        {
            return SR.GetString(Resources.ErrorCodeMessage, (uint)value);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            base.GetObjectData(info, context);
            info.AddValue(nameof(ErrorCode), ErrorCode, ErrorCode.GetType());
        }
    }
}
