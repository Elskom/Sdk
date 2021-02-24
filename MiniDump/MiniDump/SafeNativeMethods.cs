// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Windows Native Methods.
    /// </summary>
    internal static class SafeNativeMethods
    {
        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        internal static extern uint GetCurrentThreadId();

        internal static int MiniDumpWriteDump(IntPtr hProcess, int ProcessId, SafeHandle hFile, MinidumpTypes DumpType, ref MINIDUMP_EXCEPTION_INFORMATION ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam)
        {
            _ = MiniDumpWriteDump_internal(hProcess, ProcessId, hFile, DumpType, ref ExceptionParam, UserStreamParam, CallackParam);
            return Marshal.GetLastWin32Error();
        }

        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        private static extern bool MiniDumpWriteDump_internal(IntPtr hProcess, int ProcessId, SafeHandle hFile, MinidumpTypes DumpType, ref MINIDUMP_EXCEPTION_INFORMATION ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);
    }
}
