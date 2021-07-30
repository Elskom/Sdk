// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs;

using System;
using Microsoft.Diagnostics.NETCore.Client;

// maybe something like this could be added to the framework.
// do not use this attribute for anything but classes, the assembly, or the Main() method.

/// <summary>
/// Attribute for creating MiniDumps.
///
/// This registers Unhandled exception handlers to do it.
/// For Thread Exceptions use register one manually then call the static method:
/// DumpException(/* exception object inside the event args*/, true);
///
/// This will then allow minidumps from them too.
///
/// The api was changed so that way the library would not need separate configurations
/// targeting Windows Forms to bring thread exception support and making build harder to maintain.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly | AttributeTargets.Method)]
public sealed class MiniDumpAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MiniDumpAttribute"/> class.
    /// </summary>
    public MiniDumpAttribute()
    {
        var currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += (o, args)
            => _ = DumpException((Exception)args.ExceptionObject, false);
        CurrentInstance = this;
    }

    /// <summary>
    /// Occurs when a mini-dump is generated or fails.
    /// </summary>
    public static event EventHandler<MessageEventArgs> DumpMessage;

    /// <summary>
    /// Gets the current instance of this attribute.
    /// </summary>
    public static MiniDumpAttribute CurrentInstance { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the application should force close.
    /// <para>Lets the program know that it needs to close even if it is
    /// not wanted because a crash dump was created by this library and we
    /// do not want the system itself to produce a second one.</para>
    /// </summary>
    public static bool ForceClose { get; internal set; }

    /// <summary>
    /// Gets or sets the Exception message text.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the mini-dump type.
    /// </summary>
    public DumpType DumpType { get; set; }

    /// <summary>
    /// Gets or sets the title of the unhandled exception messagebox.
    /// </summary>
    public string ExceptionTitle { get; set; } = "Unhandled Exception!";

    /// <summary>
    /// Gets or sets the title of the unhandled thread exception messagebox.
    /// </summary>
    public string ThreadExceptionTitle { get; set; } = "Unhandled Thread Exception!";

    /// <summary>
    /// Gets or sets the mini-dump file name.
    /// </summary>
    public string DumpFileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mini-dump log file name.
    /// </summary>
    public string DumpLogFileName { get; set; } = string.Empty;

    /// <summary>
    /// Dumps an exception into an minidump file.
    /// Perfect for unhandled to Thread Exceptions.
    ///
    /// Note: Attribute handles unhandled exceptions by default with the exception
    /// for any Thread Exceptions.
    /// </summary>
    /// <param name="exception">The exception to dump into the minidump.</param>
    /// <param name="threadException">Whether the exception is a thread exception or not.</param>
    /// <returns>The ExitCode for the application.</returns>
    public static int DumpException(Exception exception, bool threadException)
        => MiniDump.ExceptionEventHandlerCode(exception, threadException);

    internal static void InvokeDumpMessage(MessageEventArgs e)
        => DumpMessage?.Invoke(null, e);
}
