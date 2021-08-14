// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs;

/// <summary>
/// Creates a Process with additional options.
/// </summary>
public sealed class ProcessStartOptions
{
    /// <summary>
    /// Gets a value indicating whether the process is executing or not.
    /// False if executed already.
    /// </summary>
    public bool Executing { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the process is running or not.
    /// False if not running yet or if the process terminated.
    /// </summary>
    public bool Running { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the process start information to use when executing the process.
    /// </summary>
    public ProcessStartInfo StartInfo { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the process should wait indefinitely until exited.
    /// </summary>
    public bool WaitForProcessExit { get; set; }

    /// <summary>
    /// Adds start information to this process options instance.
    /// </summary>
    /// <param name="fileName">The file name to execute.</param>
    /// <param name="arguments">The arguments to execute the file with.</param>
    /// <param name="redirectStandardOutput">Redirect standard output on the executed file.</param>
    /// <param name="redirectStandardError">Redirect standard error on the executed file.</param>
    /// <param name="useShellExecute">To optionally use shell execute to execute the process.</param>
    /// <param name="createNoWindow">To optionally create no Window on the executed process.</param>
    /// <param name="windowStyle">The window style to use on the executed process.</param>
    /// <param name="workingDirectory">The working directory of the executed process.</param>
    /// <returns>This instance of <see cref="ProcessStartOptions" />.</returns>
    public ProcessStartOptions WithStartInformation(string fileName, string arguments, bool redirectStandardOutput, bool redirectStandardError, bool useShellExecute, bool createNoWindow, ProcessWindowStyle windowStyle, string workingDirectory)
    {
        this.StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = redirectStandardOutput,
            RedirectStandardError = redirectStandardError,
            UseShellExecute = useShellExecute,
            CreateNoWindow = createNoWindow,
            WindowStyle = windowStyle,
            WorkingDirectory = workingDirectory,
        };
        return this;
    }

    /// <summary>
    /// Executes the process.
    /// </summary>
    /// <returns>The process's redirected outputs.</returns>
    /// <exception cref="InvalidOperationException">When the instance's startup information is null.</exception>
    /// <exception cref="FileNotFoundException">When the file to the process to execute does not exist on disk.</exception>
    public string Start()
    {
        if (this.StartInfo is null)
        {
            throw new InvalidOperationException("StartInfo must not be null.");
        }

        if (!File.Exists(this.StartInfo.FileName))
        {
            throw new FileNotFoundException("File to execute does not exist.");
        }

        this.Executing = true;
        StringBuilder stdout = null;
        StringBuilder stderr = null;
        using var proc = Process.Start(this.StartInfo);
        proc.OutputDataReceived += (sender, e) =>
        {
            if (stdout is null)
            {
                stdout = new StringBuilder();
                stdout.Append(e.Data);
                stdout.AppendLine();
            }
            else
            {
                stdout.AppendLine(e.Data);
            }
        };
        proc.ErrorDataReceived += (sender, e) =>
        {
            if (stderr is null)
            {
                stderr = new StringBuilder();
                stderr.Append(e.Data);
                stderr.AppendLine();
            }
            else
            {
                stderr.AppendLine(e.Data);
            }
        };
        this.Running = true;
        this.Executing = false;
        if (this.StartInfo.RedirectStandardOutput)
        {
            proc.BeginOutputReadLine();
        }

        if (this.StartInfo.RedirectStandardError)
        {
            proc.BeginErrorReadLine();
        }

        if (this.WaitForProcessExit)
        {
            proc.WaitForExit();
        }

        this.Running = false;
        return (stdout is not null, stderr is not null) switch
        {
            (true, false) => $"{stdout}",
            (true, true) => $@"{stdout}
{stderr}",
            (false, false) => string.Empty,
            (false, true) => $"{stderr}",
        };
    }
}
