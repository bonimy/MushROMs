// <copyright file="ArgumentNaNException.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Helper
{
    public class ArgumentNaNException : ArgumentException
    {
        public ArgumentNaNException() : base()
        {
        }

        public ArgumentNaNException(string paramName) :
            base(SR.ErrorValueIsNaN(paramName), paramName)
        {
        }

        public ArgumentNaNException(string paramName, Exception innerException) :
            base(SR.ErrorValueIsNaN(paramName), paramName, innerException)
        {
        }

        protected ArgumentNaNException(
            SerializationInfo info,
            StreamingContext context) :
            base(info, context)
        {
        }
    }
}
