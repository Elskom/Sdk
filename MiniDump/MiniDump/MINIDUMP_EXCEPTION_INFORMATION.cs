// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    // Pack=4 is important! So it works also for x64!
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "To match the native type names to pass into MiniDumpWriteDump.")]
    internal struct MINIDUMP_EXCEPTION_INFORMATION
    {
        internal uint ThreadId;
        internal IntPtr ExceptionPointers;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool ClientPointers;
    }
}
