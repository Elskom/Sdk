// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The flags for the type of mini-dumps to generate.
    /// </summary>
    [Flags]
    [SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"", Justification = "To match the native flag names to pass into MiniDumpWriteDump.")]
    public enum MinidumpTypes
    {
        /// <summary>
        /// A normal mini-dump.
        /// </summary>
        Normal = 0x00000000,

        /// <summary>
        /// Include data segments.
        /// </summary>
        WithDataSegs = 0x00000001,

        /// <summary>
        /// Include full memory.
        /// </summary>
        WithFullMemory = 0x00000002,

        /// <summary>
        /// Include handle data.
        /// </summary>
        WithHandleData = 0x00000004,

        /// <summary>
        /// Filter the memory.
        /// </summary>
        FilterMemory = 0x00000008,

        /// <summary>
        /// Scan the memory.
        /// </summary>
        ScanMemory = 0x00000010,

        /// <summary>
        /// Include unloaded dll's.
        /// </summary>
        WithUnloadedModules = 0x00000020,

        /// <summary>
        /// Include indirectly referenced memory.
        /// </summary>
        WithIndirectlyReferencedMemory = 0x00000040,

        /// <summary>
        /// Filter dll paths.
        /// </summary>
        FilterModulePaths = 0x00000080,

        /// <summary>
        /// Include process thread data.
        /// </summary>
        WithProcessThreadData = 0x00000100,

        /// <summary>
        /// Include private read and write memory.
        /// </summary>
        WithPrivateReadWriteMemory = 0x00000200,

        /// <summary>
        /// Include optional data.
        /// </summary>
        WithoutOptionalData = 0x00000400,

        /// <summary>
        /// Include full memory information.
        /// </summary>
        WithFullMemoryInfo = 0x00000800,

        /// <summary>
        /// Include thread information.
        /// </summary>
        WithThreadInfo = 0x00001000,

        /// <summary>
        /// Include code segments.
        /// </summary>
        WithCodeSegs = 0x00002000,

        /// <summary>
        /// Exclude Auxiliary state.
        /// </summary>
        WithoutAuxiliaryState = 0x00004000,

        /// <summary>
        /// Include full Auxiliary state.
        /// </summary>
        WithFullAuxiliaryState = 0x00008000,

        /// <summary>
        /// Include private write and copy memory.
        /// </summary>
        WithPrivateWriteCopyMemory = 0x00010000,

        /// <summary>
        /// Ignore inaccessible memory.
        /// </summary>
        IgnoreInaccessibleMemory = 0x00020000,

        // From minidumpapiset.h in the Windows 10 v10.0.17763.0 SDK.

        /// <summary>
        /// Include some sort of token information.
        /// </summary>
        WithTokenInformation = 0x00040000,

        /// <summary>
        /// Include module file headers.
        /// </summary>
        WithModuleHeaders = 0x00080000,

        /// <summary>
        /// Some sort of filter.
        /// </summary>
        FilterTriage = 0x00100000,

        /// <summary>
        /// Include some sort of AvxXState stuff.
        /// </summary>
        WithAvxXStateContext = 0x00200000,

        /// <summary>
        /// Include some sort of Ipt Trace. Maybe some sort of stack trace?
        /// </summary>
        WithIptTrace = 0x00400000,

        // Updated from the above. Could be why mini dump fails with error code 87 sometimes?

        /// <summary>
        /// Include Valid types?.
        /// </summary>
        ValidTypeFlags = 0x007fffff,
    }
}
