// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Windows.Win32.System.Diagnostics.Debug
{
    using global::System;
    using Windows.Win32.Foundation;

    /// <summary>Contains the exception information written to the minidump file by the MiniDumpWriteDump function.</summary>
    /// <remarks>
    /// <para><see href = "https://docs.microsoft.com/windows/win32/api//minidumpapiset/ns-minidumpapiset-minidump_exception_information">Learn more about this API from docs.microsoft.com</see>.</para>
    /// </remarks>
    internal struct MINIDUMP_EXCEPTION_INFORMATION : IEquatable<MINIDUMP_EXCEPTION_INFORMATION>
    {
        /// <summary>The identifier of the thread throwing the exception.</summary>
        internal uint ThreadId;

        /// <summary>
        /// <para>A pointer to an <a href = "https://docs.microsoft.com/windows/desktop/api/winnt/ns-winnt-exception_pointers">EXCEPTION_POINTERS</a> structure specifying a computer-independent description of the exception and the processor context at the time of the exception.</para>
        /// <para><see href = "https://docs.microsoft.com/windows/win32/api//minidumpapiset/ns-minidumpapiset-minidump_exception_information#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        internal nint ExceptionPointers;

        /// <summary>Determines where to get the memory regions pointed to by the <b>ExceptionPointers</b> member. Set to <b>TRUE</b> if the memory resides in the process being debugged (the target process of the debugger). Otherwise, set to <b>FALSE</b> if the memory resides in the address space of the calling program (the debugger process). If you are accessing local memory (in the calling process) you should not set this member to <b>TRUE</b>.</summary>
        internal BOOL ClientPointers;

        public bool Equals(MINIDUMP_EXCEPTION_INFORMATION other)
            => throw new NotImplementedException();

        public override bool Equals(object obj)
            => obj is MINIDUMP_EXCEPTION_INFORMATION minidumpExceptionInformation
               && this.Equals(minidumpExceptionInformation);

        public override int GetHashCode()
            => throw new NotImplementedException();
    }
}
