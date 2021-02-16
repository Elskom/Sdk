// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;

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
    public class MiniDumpAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiniDumpAttribute"/> class.
        /// </summary>
        public MiniDumpAttribute()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);
            CurrentInstance = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniDumpAttribute"/> class.
        /// </summary>
        /// <param name="text">Exception message text.</param>
        [Obsolete("This version of the constrictor is only here for backwards compatibility and will be removed in a future version.")]
        public MiniDumpAttribute(string text)
            : this()
            => this.Text = text;

        /// <summary>
        /// Gets the current instance of this attribute.
        /// </summary>
        public static MiniDumpAttribute CurrentInstance { get; private set; }

        /// <summary>
        /// Gets or sets the Exception message text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the mini-dump type.
        /// </summary>
        public MinidumpTypes DumpType { get; set; }

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
        /// Pefect for unhandled to Thread Exceptions.
        ///
        /// Note: Attribute handles unhandled exceptions by default with the exception
        /// for any Thread Exceptions.
        /// </summary>
        /// <param name="exception">The exception to dump into the minidump.</param>
        /// <param name="threadException">Whether the exception is a thread exception or not.</param>
        public static void DumpException(Exception exception, bool threadException)
            => MiniDump.ExceptionEventHandlerCode(exception, threadException);

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
            => DumpException((Exception)args.ExceptionObject, false);
    }
}
