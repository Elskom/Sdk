// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Runtime.InteropServices;

    internal static class SafeNativeMethods
    {
        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        internal static extern uint GetCurrentThreadId();

        [DllImport("Kernel32", ExactSpelling = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        internal static extern uint GetCurrentProcessId();

        internal static Microsoft.Win32.SafeHandles.SafeFileHandle GetCurrentProcess_SafeHandle()
            => new(GetCurrentProcess(), true);

        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        internal static extern unsafe bool MiniDumpWriteDump(SafeHandle hProcess, uint processId, SafeHandle hFile, MinidumpTypes DumpType, MINIDUMP_EXCEPTION_INFORMATION* ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);

        [DllImport("Kernel32", ExactSpelling = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        private static extern IntPtr GetCurrentProcess();
    }
}
