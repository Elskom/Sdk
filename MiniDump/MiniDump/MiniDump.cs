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
    using System.Runtime.InteropServices;

    /// <summary>
    /// A class that can generate mini-dumps.
    /// </summary>
    public static class MiniDump
    {
        /// <summary>
        /// Occurs when a mini-dump is generated or fails.
        /// </summary>
        public static event EventHandler<MessageEventArgs> DumpMessage;

        internal static void ExceptionEventHandlerCode(Exception e, bool threadException)
        {
            var exceptionData = $"{e.GetType()}: {e.Message}{Environment.NewLine}{e.StackTrace}{Environment.NewLine}";
            PrintInnerExceptions(e, ref exceptionData);

            // do not dump or close if in a debugger.
            if (!Debugger.IsAttached)
            {
                ForceClosure.ForceClose = true;

                // if this is not Windows, MiniDumpToFile(), which calls MiniDumpWriteDump() in dbghelp.dll
                // cannot be used as it does not exist. I need to figure out mini-dumping for
                // these platforms manually. In that case I guess it does not matter much anyway
                // with the world of debugging and running in a IDE.
#if NET5_0_OR_GREATER
                if (OperatingSystem.IsWindows())
#else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
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
                    DumpMessage?.Invoke(typeof(MiniDump), new MessageEventArgs(string.Format(CultureInfo.InvariantCulture, MiniDumpAttribute.CurrentInstance.Text, MiniDumpAttribute.CurrentInstance.DumpLogFileName), threadException ? MiniDumpAttribute.CurrentInstance.ThreadExceptionTitle : MiniDumpAttribute.CurrentInstance.ExceptionTitle, ErrorLevel.Error));
                }
            }
        }

        private static void MiniDumpToFile(string fileToDump, MinidumpTypes dumpType)
        {
            using var fsToDump = File.Open(fileToDump, FileMode.Create, FileAccess.ReadWrite, FileShare.Write);
            using var thisProcess = Process.GetCurrentProcess();
            var mINIDUMP_EXCEPTION_INFORMATION = new MINIDUMP_EXCEPTION_INFORMATION
            {
                ClientPointers = false,
#if WITH_EXCEPTIONPOINTERS
                ExceptionPointers = Marshal.GetExceptionPointers(),
#else
                ExceptionPointers = IntPtr.Zero,
#endif
                ThreadId = SafeNativeMethods.GetCurrentThreadId(),
            };
            var error = SafeNativeMethods.MiniDumpWriteDump(
                thisProcess.Handle,
                thisProcess.Id,
                fsToDump.SafeFileHandle,
                dumpType,
                ref mINIDUMP_EXCEPTION_INFORMATION,
                IntPtr.Zero,
                IntPtr.Zero);
            if (error > 0)
            {
                DumpMessage?.Invoke(typeof(MiniDump), new MessageEventArgs($"Mini-dumping failed with Code: {error}", "Error!", ErrorLevel.Error));
            }
        }

        private static void PrintInnerExceptions(Exception exception, ref string result)
        {
            if (exception.InnerException != null)
            {
                result += $"{exception.InnerException.GetType()}: {exception.InnerException.Message}{Environment.NewLine}{exception.InnerException.StackTrace}{Environment.NewLine}";
                PrintInnerExceptions(exception.InnerException, ref result);
            }
        }
    }
}
