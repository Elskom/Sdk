// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class MiniDump
    {
        internal static int ExceptionEventHandlerCode(Exception e, bool threadException)
        {
            var exceptionData = PrintExceptions(e);

            // do not dump or close if in a debugger.
            if (!Debugger.IsAttached)
            {
                if (!MiniDumpAttribute.ForceClose)
                {
                    MiniDumpAttribute.ForceClose = true;
                }

                // if this is not Windows, MiniDumpToFile(), which calls MiniDumpWriteDump() in dbghelp.dll
                // cannot be used as it does not exist. I need to figure out mini-dumping for
                // these platforms manually. In that case I guess it does not matter much anyway
                // with the world of debugging and running in a IDE.
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (string.IsNullOrEmpty(MiniDumpAttribute.CurrentInstance.DumpLogFileName))
                    {
                        MiniDumpAttribute.CurrentInstance.DumpLogFileName = SettingsFile.ErrorLogPath;
                    }

                    if (string.IsNullOrEmpty(MiniDumpAttribute.CurrentInstance.DumpFileName))
                    {
                        MiniDumpAttribute.CurrentInstance.DumpFileName = SettingsFile.MiniDumpPath;
                    }

                    File.WriteAllText(MiniDumpAttribute.CurrentInstance.DumpLogFileName, exceptionData);
                    MiniDumpToFile(MiniDumpAttribute.CurrentInstance.DumpFileName, MiniDumpAttribute.CurrentInstance.DumpType);
                    MessageEventArgs args = new(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            MiniDumpAttribute.CurrentInstance.Text,
                            MiniDumpAttribute.CurrentInstance.DumpLogFileName),
                        threadException ? MiniDumpAttribute.CurrentInstance.ThreadExceptionTitle : MiniDumpAttribute.CurrentInstance.ExceptionTitle,
                        ErrorLevel.Error);
                    MiniDumpAttribute.InvokeDumpMessage(typeof(MiniDump), args);
                    return args.ExitCode;
                }
            }

            return 1;
        }

        private static void MiniDumpToFile(string fileToDump, MinidumpTypes dumpType)
        {
            using var fsToDump = File.Open(fileToDump, FileMode.Create, FileAccess.ReadWrite, FileShare.Write);
            var error = MiniDumpWriteDump(fsToDump.SafeFileHandle, dumpType);
            if (error > 0)
            {
                MessageEventArgs args = new(
                    $"Mini-dumping failed with Code: {error}",
                    "Error!",
                    ErrorLevel.Error);
                MiniDumpAttribute.InvokeDumpMessage(typeof(MiniDump), args);
            }
        }

        private static unsafe int MiniDumpWriteDump(SafeHandle hFile, MinidumpTypes dumpType)
        {
            var exceptionInformation = GetExceptionInformation();
            _ = SafeNativeMethods.MiniDumpWriteDump(
                SafeNativeMethods.GetCurrentProcess_SafeHandle(),
                SafeNativeMethods.GetCurrentProcessId(),
                hFile,
                dumpType,
                &exceptionInformation,
                default,
                default);
            return Marshal.GetLastWin32Error();
        }

        private static MINIDUMP_EXCEPTION_INFORMATION GetExceptionInformation()
            => new()
            {
                ClientPointers = false,
                ExceptionPointers = GetExceptionPointers(),
                ThreadId = SafeNativeMethods.GetCurrentThreadId(),
            };

        private static IntPtr GetExceptionPointers()
        {
            // because we target .NET Standard 2.0 we need to probe for
            // Marshal.GetExceptionPointers using reflection then call
            // it if it exists but return a null pointer if it does not exist.
            var method = typeof(Marshal).GetMethod(
                "GetExceptionPointers",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly,
                null,
                Type.EmptyTypes,
                null);
            return (IntPtr?)method?.Invoke(null, null) ?? IntPtr.Zero;
        }

        private static string PrintExceptions(Exception exception)
        {
            StringBuilder sb = new();
            sb.AppendLine($"{exception.GetType()}: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
            var currException = exception.InnerException;
            while (currException is not null)
            {
                sb.AppendLine($"{currException.GetType()}: {currException.Message}{Environment.NewLine}{currException.StackTrace}");
                currException = currException.InnerException;
            }

            return sb.ToString();
        }
    }
}
