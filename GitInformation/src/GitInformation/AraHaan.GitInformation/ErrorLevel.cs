// Copyright (c) 2019-2020, AraHaan.
// https://github.com/AraHaan/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace System.Messaging
{
    /// <summary>
    /// An <see langword="enum"/> that provides the error level of a message.
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>
        /// Represents that the current error level is nothing.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents that the current error level is information.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Represents that the current error level is a warning.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Represents that the current error level is a error.
        /// </summary>
        Error = 3,
    }
}
