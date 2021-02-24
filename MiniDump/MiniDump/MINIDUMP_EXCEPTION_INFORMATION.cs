// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Runtime.InteropServices;

    // Pack=4 is important! So it works also for x64!
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct MINIDUMP_EXCEPTION_INFORMATION
    {
        internal uint ThreadId;
        internal IntPtr ExceptionPointers;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool ClientPointers;
    }
}
