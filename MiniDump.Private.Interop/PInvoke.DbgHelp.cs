// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

#if NEVER_DEFINED
namespace Windows.Win32
{
    using global::System.Runtime.InteropServices;
    using Windows.Win32.System.Diagnostics.Debug;

    /// <content>
    /// Contains extern methods from "DbgHelp.dll".
    /// </content>
    internal static partial class PInvoke
    {
        /// <inheritdoc cref = "MiniDumpWriteDump(SafeHandle, uint, SafeHandle, MINIDUMP_TYPE, MINIDUMP_EXCEPTION_INFORMATION*, nint, nint)"/>
        internal static unsafe bool MiniDumpWriteDump(SafeHandle hProcess, uint processId, SafeHandle hFile, MINIDUMP_TYPE dumpType, MINIDUMP_EXCEPTION_INFORMATION? exceptionParam, nint userStreamParam, nint callbackParam)
        {
            var hProcessAddRef = false;
            var hFileAddRef = false;
            try
            {
                hProcess?.DangerousAddRef(ref hProcessAddRef);
                hFile?.DangerousAddRef(ref hFileAddRef);
                var exceptionParamLocal = exceptionParam ?? default;
                return MiniDumpWriteDump(hProcess, processId, hFile, dumpType, exceptionParam.HasValue ? &exceptionParamLocal : null, userStreamParam, callbackParam);
            }
            finally
            {
                if (hProcessAddRef)
                {
                    hProcess?.DangerousRelease();
                }

                if (hFileAddRef)
                {
                    hFile?.DangerousRelease();
                }
            }
        }

        /// <summary>Writes user-mode minidump information to the specified file.</summary>
        /// <param name = "hProcess">
        /// <para>A handle to the process for which the information is to be generated.</para>
        /// <para>This handle must have <b>PROCESS_QUERY_INFORMATION</b> and <b>PROCESS_VM_READ</b> access to the process. If handle information is to be collected then <b>PROCESS_DUP_HANDLE</b> access is also required. For more information, see <a href = "https://docs.microsoft.com/windows/desktop/ProcThread/process-security-and-access-rights">Process Security and Access Rights</a>. The caller must also be able to get <b>THREAD_ALL_ACCESS</b> access to the threads in the process. For more information, see <a href = "https://docs.microsoft.com/windows/desktop/ProcThread/thread-security-and-access-rights">Thread Security and Access Rights</a>.</para>
        /// <para><see href = "https://docs.microsoft.com/windows/win32/api//minidumpapiset/nf-minidumpapiset-minidumpwritedump#parameters">Read more on docs.microsoft.com</see>.</para>
        /// </param>
        /// <param name = "processId">The identifier of the process for which the information is to be generated.</param>
        /// <param name = "hFile">A handle to the file in which the information is to be written.</param>
        /// <param name = "dumpType">
        /// <para>The type of information to be generated. This parameter can be one or more of the values from the <a href = "https://docs.microsoft.com/windows/desktop/api/minidumpapiset/ne-minidumpapiset-minidump_type">MINIDUMP_TYPE</a> enumeration.</para>
        /// <para><see href = "https://docs.microsoft.com/windows/win32/api//minidumpapiset/nf-minidumpapiset-minidumpwritedump#parameters">Read more on docs.microsoft.com</see>.</para>
        /// </param>
        /// <param name = "exceptionParam">
        /// <para>A pointer to a <a href = "https://docs.microsoft.com/windows/win32/api/minidumpapiset/ns-minidumpapiset-minidump_exception_information">MINIDUMP_EXCEPTION_INFORMATION</a> structure describing the client exception that caused the minidump to be generated. If the value of this parameter is <b>NULL</b>, no exception information is included in the minidump file.</para>
        /// <para><see href = "https://docs.microsoft.com/windows/win32/api//minidumpapiset/nf-minidumpapiset-minidumpwritedump#parameters">Read more on docs.microsoft.com</see>.</para>
        /// </param>
        /// <param name = "userStreamParam">
        /// <para>A pointer to a <a href = "https://docs.microsoft.com/windows/win32/api/minidumpapiset/ns-minidumpapiset-minidump_user_stream_information">MINIDUMP_USER_STREAM_INFORMATION</a> structure. If the value of this parameter is <b>NULL</b>, no user-defined information is included in the minidump file.</para>
        /// <para><see href = "https://docs.microsoft.com/windows/win32/api//minidumpapiset/nf-minidumpapiset-minidumpwritedump#parameters">Read more on docs.microsoft.com</see>.</para>
        /// </param>
        /// <param name = "callbackParam">
        /// <para>A pointer to a <a href = "https://docs.microsoft.com/windows/win32/api/minidumpapiset/ns-minidumpapiset-minidump_callback_information">MINIDUMP_CALLBACK_INFORMATION</a> structure that specifies a callback routine which is to receive extended minidump information. If the value of this parameter is <b>NULL</b>, no callbacks are performed.</para>
        /// <para><see href = "https://docs.microsoft.com/windows/win32/api//minidumpapiset/nf-minidumpapiset-minidumpwritedump#parameters">Read more on docs.microsoft.com</see>.</para>
        /// </param>
        /// <returns>
        /// <para>If the function succeeds, the return value is <b>TRUE</b>; otherwise, the return value is <b>FALSE</b>. To retrieve extended error information, call <a href = "/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>. Note that the last error will be an <b>HRESULT</b> value.</para>
        /// <para>If the operation is canceled, the last error code is <b>ERROR_CANCELLED</b>.</para>
        /// </returns>
        /// <remarks>
        /// <para><see href = "https://docs.microsoft.com/windows/win32/api//minidumpapiset/nf-minidumpapiset-minidumpwritedump">Learn more about this API from docs.microsoft.com</see>.</para>
        /// </remarks>
        [DllImport("DbgHelp", ExactSpelling = true, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        private static extern unsafe bool MiniDumpWriteDump(SafeHandle hProcess, uint processId, SafeHandle hFile, MINIDUMP_TYPE dumpType, [Optional] MINIDUMP_EXCEPTION_INFORMATION* exceptionParam, [Optional] nint userStreamParam, [Optional] nint callbackParam);
    }
}
#endif
