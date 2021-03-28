// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;

    /// <summary>Identifies the type of information that will be written to the minidump file by the MiniDumpWriteDump function.</summary>
    /// <remarks>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type">Learn more about this API from docs.microsoft.com</see>.</para>
    /// </remarks>
    [Flags]
    public enum MINIDUMP_TYPE : uint
    {
        /// <summary>Include just the information necessary to capture stack traces for all existing threads in a process.</summary>
        MiniDumpNormal = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpNormal,

        /// <summary>
        /// <para>Include the data sections from all loaded modules. This results in the inclusion of global variables, which can make the minidump file significantly larger. For per-module control, use the <b>ModuleWriteDataSeg</b> enumeration value from <a href="https://docs.microsoft.com/windows/desktop/api/minidumpapiset/ne-minidumpapiset-module_write_flags">MODULE_WRITE_FLAGS</a>.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithDataSegs = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithDataSegs,

        /// <summary>
        /// <para>Include all accessible memory in the process. The raw memory data is included at the end, so that the initial structures can be mapped directly without the raw memory information. This option can result in a very large file.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithFullMemory = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithFullMemory,

        /// <summary>
        /// <para>Include high-level information about the operating system handles that are active when the minidump is made.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithHandleData = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithHandleData,

        /// <summary>
        /// <para>Stack and backing store memory written to the minidump file should be filtered to remove all but the pointer values necessary to reconstruct a stack trace.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpFilterMemory = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpFilterMemory,

        /// <summary>
        /// <para>Stack and backing store memory should be scanned for pointer references to modules in the module list. If a module is referenced by stack or backing store memory, the <b>ModuleWriteFlags</b> member of the <a href="https://docs.microsoft.com/windows/win32/api/minidumpapiset/ns-minidumpapiset-minidump_callback_output">MINIDUMP_CALLBACK_OUTPUT</a> structure is set to <b>ModuleReferencedByMemory</b>.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpScanMemory = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpScanMemory,

        /// <summary>
        /// <para>Include information from the list of modules that were recently unloaded, if this information is maintained by the operating system.</para>
        /// <para><b>Windows Server 2003 and Windows XP:  </b>The operating system does not maintain information for unloaded modules until Windows Server 2003 with SP1 and Windows XP with SP2. <b>DbgHelp 5.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithUnloadedModules = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithUnloadedModules,

        /// <summary>
        /// <para>Include pages with data referenced by locals or other stack memory. This option can increase the size of the minidump file significantly.</para>
        /// <para><b>DbgHelp 5.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithIndirectlyReferencedMemory = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithIndirectlyReferencedMemory,

        /// <summary>
        /// <para>Filter module paths for information such as user names or important directories. This option may prevent the system from locating the image file and should be used only in special situations.</para>
        /// <para><b>DbgHelp 5.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpFilterModulePaths = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpFilterModulePaths,

        /// <summary>
        /// <para>Include complete per-process and per-thread information from the operating system.</para>
        /// <para><b>DbgHelp 5.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithProcessThreadData = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithProcessThreadData,

        /// <summary>
        /// <para>Scan the virtual address space for <b>PAGE_READWRITE</b> memory to be included.</para>
        /// <para><b>DbgHelp 5.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithPrivateReadWriteMemory = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithPrivateReadWriteMemory,

        /// <summary>
        /// <para>Reduce the data that is dumped by eliminating memory regions that are not essential to meet criteria specified for the dump. This can avoid dumping  memory that may contain data that is private to the user. However, it is not a guarantee that no private information will be present.</para>
        /// <para><b>DbgHelp 6.1 and earlier:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithoutOptionalData = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithoutOptionalData,

        /// <summary>
        /// <para>Include memory region information. For more information, see <a href="https://docs.microsoft.com/windows/win32/api/minidumpapiset/ns-minidumpapiset-minidump_memory_info_list">MINIDUMP_MEMORY_INFO_LIST</a>.</para>
        /// <para><b>DbgHelp 6.1 and earlier:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithFullMemoryInfo = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithFullMemoryInfo,

        /// <summary>
        /// <para>Include thread state information. For more information, see <a href="https://docs.microsoft.com/windows/win32/api/minidumpapiset/ns-minidumpapiset-minidump_thread_info_list">MINIDUMP_THREAD_INFO_LIST</a>.</para>
        /// <para><b>DbgHelp 6.1 and earlier:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithThreadInfo = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithThreadInfo,

        /// <summary>
        /// <para>Include all code and code-related sections from loaded modules to capture executable content. For per-module control, use the <b>ModuleWriteCodeSegs</b> enumeration value from <a href="https://docs.microsoft.com/windows/desktop/api/minidumpapiset/ne-minidumpapiset-module_write_flags">MODULE_WRITE_FLAGS</a>.</para>
        /// <para><b>DbgHelp 6.1 and earlier:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithCodeSegs = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithCodeSegs,

        /// <summary>Turns off secondary auxiliary-supported memory gathering.</summary>
        MiniDumpWithoutAuxiliaryState = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithoutAuxiliaryState,

        /// <summary>
        /// <para>Requests that auxiliary data providers include their state in the dump image; the state data that is included is provider dependent. This option can result in a large dump image.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithFullAuxiliaryState = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithFullAuxiliaryState,

        /// <summary>
        /// <para>Scans the virtual address space for <b>PAGE_WRITECOPY</b> memory to be included.</para>
        /// <para><b>Prior to DbgHelp 6.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithPrivateWriteCopyMemory = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithPrivateWriteCopyMemory,

        /// <summary>
        /// <para>If you specify <b>MiniDumpWithFullMemory</b>, the <a href="https://docs.microsoft.com/windows/desktop/api/minidumpapiset/nf-minidumpapiset-minidumpwritedump">MiniDumpWriteDump</a> function will fail if the function cannot read the memory regions; however, if you include <b>MiniDumpIgnoreInaccessibleMemory</b>, the <b>MiniDumpWriteDump</b> function will ignore the memory read failures and continue to generate the dump. Note that the inaccessible memory regions are not included in the dump. <b>Prior to DbgHelp 6.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpIgnoreInaccessibleMemory = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpIgnoreInaccessibleMemory,

        /// <summary>
        /// <para>Adds security token related data. This will make the "!token" extension work when processing a user-mode dump.</para>
        /// <para><b>Prior to DbgHelp 6.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithTokenInformation = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithTokenInformation,

        /// <summary>
        /// <para>Adds module header related data.</para>
        /// <para><b>Prior to DbgHelp 6.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithModuleHeaders = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithModuleHeaders,

        /// <summary>
        /// <para>Adds filter triage related data.</para>
        /// <para><b>Prior to DbgHelp 6.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpFilterTriage = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpFilterTriage,

        /// <summary>
        /// <para>Adds AVX crash state context registers. <b>Prior to DbgHelp 6.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithAvxXStateContext = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithAvxXStateContext,

        /// <summary>
        /// <para>Adds Intel Processor Trace related data. <b>Prior to DbgHelp 6.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpWithIptTrace = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpWithIptTrace,

        /// <summary>
        /// <para>Scans inaccessible partial memory pages. <b>Prior to DbgHelp 6.1:  </b>This value is not supported.</para>
        /// <para><see href="https://docs.microsoft.com/windows/win32/api//minidumpapiset/ne-minidumpapiset-minidump_type#members">Read more on docs.microsoft.com</see>.</para>
        /// </summary>
        MiniDumpScanInaccessiblePartialPages = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpScanInaccessiblePartialPages,

        /// <summary>Indicates which flags are valid.</summary>
        MiniDumpValidTypeFlags = Windows.Win32.System.Diagnostics.Debug.MINIDUMP_TYPE.MiniDumpValidTypeFlags,
    }
}
