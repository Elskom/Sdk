// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
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
