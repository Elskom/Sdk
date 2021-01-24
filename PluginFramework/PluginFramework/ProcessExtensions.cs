// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System.Diagnostics;

    /// <summary>
    /// A extension class for extensions to <see cref="Process"/>.
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the process is executing or not.
        /// False if executed already.
        /// </summary>
        public static bool Executing { get; internal set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the process should wait indefinately until exited.
        /// </summary>
        public static bool WaitForProcessExit { get; set; }

        /// <summary>
        /// Resets the <see cref="Executing"/> property to the default value.
        /// </summary>
        public static void Reset()
            => Executing = true;

        /// <summary>
        /// Overload for Shell() Function that Allows Overloading of the Working directory Variable.
        /// It must be a String but can be variables that returns strings.
        /// </summary>
        /// <param name="proc">The process instance for which to use to execute the process.</param>
        /// <param name="fileName">Process file name to execute.</param>
        /// <param name="arguments">Commands to pass to the process file to execute.</param>
        /// <param name="redirectStandardOutput">redirects stdout of the target process.</param>
        /// <param name="redirectStandardError">redirects stderr of the target process.</param>
        /// <param name="useShellExecute">uses shell execute instead.</param>
        /// <param name="createNoWindow">Creates no new window for the process.</param>
        /// <param name="windowStyle">Window style for the target process.</param>
        /// <param name="workingDirectory">Working directory for the target process.</param>
        /// <param name="waitForProcessExit">Waits for the target process to terminate.</param>
        /// <returns>empty string, process stdout data, process stderr data.</returns>
        public static string Shell(this Process proc, string fileName, string arguments, bool redirectStandardOutput, bool redirectStandardError, bool useShellExecute, bool createNoWindow, ProcessWindowStyle windowStyle, string workingDirectory, bool waitForProcessExit)
        {
            if (proc == null)
            {
                throw new System.ArgumentNullException(nameof(proc));
            }

            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.RedirectStandardOutput = redirectStandardOutput;
            proc.StartInfo.RedirectStandardError = redirectStandardError;
            proc.StartInfo.UseShellExecute = useShellExecute;
            proc.StartInfo.CreateNoWindow = createNoWindow;
            proc.StartInfo.WindowStyle = windowStyle;
            proc.StartInfo.WorkingDirectory = workingDirectory;
            WaitForProcessExit = waitForProcessExit;
            return proc.Shell();
        }

        /// <summary>
        /// Overload for Shell() Function that Allows Overloading of the Working directory Variable.
        /// It must be a String but can be variables that returns strings.
        /// </summary>
        /// <param name="proc">The process instance for which to use to execute the process.</param>
        /// <returns>empty string, process stdout data, process stderr data.</returns>
        public static string Shell(this Process proc)
        {
            if (proc == null)
            {
                throw new System.ArgumentNullException(nameof(proc));
            }

            if (!Executing)
            {
                Reset();
            }

            var ret = string.Empty;
            _ = proc.Start();
            if (Executing)
            {
                Executing = false;
            }

            if (proc.StartInfo.RedirectStandardError)
            {
                ret = proc.StandardError.ReadToEnd();
            }

            if (proc.StartInfo.RedirectStandardOutput)
            {
                ret = proc.StandardOutput.ReadToEnd();
            }

            if (WaitForProcessExit)
            {
                proc.WaitForExit();
            }

            return ret;
        }
    }
}
