// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    /// <summary>
    /// A static class to let the program
    /// know that it needs to close even
    /// if it is not wanted because a crash
    /// dump was created by this library
    /// and we do not want the system itself
    /// to produce a second one.
    /// </summary>
    public static class ForceClosure
    {
        /// <summary>
        /// Gets a value indicating whether the application should force close.
        /// </summary>
        public static bool ForceClose { get; internal set; }
    }
}
