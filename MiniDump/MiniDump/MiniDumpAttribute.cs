// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
#if WITH_WINFORMS
    using System.Threading;
    using System.Windows.Forms;
#endif

    // maybe something like this could be added to the framework.
    // do not use this attribute for anything but classes, the assembly, or the Main() method.

    /// <summary>
    /// Attribute for creating MiniDumps.
    ///
    /// This registers Unhandled exception and Thread exception (if availible)
    /// handlers to do it.
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
#if WITH_WINFORMS
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionHandler);
#endif
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

#if WITH_WINFORMS
        /// <summary>
        /// Gets or sets the title of the unhandled thread exception messagebox.
        /// </summary>
        public string ThreadExceptionTitle { get; set; } = "Unhandled Thread Exception!";
#endif

        /// <summary>
        /// Gets or sets the mini-dump file name.
        /// </summary>
        public string DumpFileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mini-dump log file name.
        /// </summary>
        public string DumpLogFileName { get; set; } = string.Empty;

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
            => MiniDump.ExceptionEventHandlerCode((Exception)args.ExceptionObject, false);

#if WITH_WINFORMS
        private static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
            => MiniDump.ExceptionEventHandlerCode(e.Exception, true);
#endif
    }
}
