// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs;

/// <summary>
/// A class that handles the settings for any application.
/// </summary>
public static class SettingsFile
{
    private static int thisProcessId;
    private static string thisProcessName;
    private static string localApplicationDataFolder;

    static SettingsFile()
    {
    }

    /// <summary>
    /// Gets or sets the Json settings file instance.
    ///
    /// This is designed so there is globally only
    /// a single instance to save time, and memory.
    /// </summary>
    /// <value>
    /// The Json settings file instance.
    ///
    /// This is designed so there is globally only
    /// a single instance to save time, and memory.
    /// </value>
    public static JsonSettings SettingsJson { get; set; } = JsonSettings.OpenFile();

    /// <summary>
    /// Gets or sets the Application's name to use for Resolving the paths for the
    /// Settings file, error logs, and minidumps.
    /// </summary>
    public static string ApplicationName { get; set; } = "Els_kom";

    /// <summary>
    /// Gets the path to the Application Settings file.
    ///
    /// Creates the folder if needed.
    /// </summary>
    /// <value>
    /// The path to the Application Settings file.
    ///
    /// Creates the folder if needed.
    /// </value>
    public static string SettingsPath { get; } = $"{LocalApplicationDataFolder}Settings.json";

    /// <summary>
    /// Gets the path to the Application Error Log file.
    ///
    /// Creates the Error Log file if needed.
    /// </summary>
    public static string ErrorLogPath { get; } = $"{LocalApplicationDataFolder}{ThisProcessName}-{ThisProcessId}.log";

    /// <summary>
    /// Gets the path to the Application Mini-Dump file.
    /// </summary>
    // On Non-Windows OS's all crash dumps must be named "core.{PID}"!!!
    public static string MiniDumpPath { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{LocalApplicationDataFolder}{ThisProcessName}-{ThisProcessId}.mdmp" : $"{LocalApplicationDataFolder}core.{ThisProcessId}";

    internal static int ThisProcessId
    {
        get
        {
            if (!thisProcessId.Equals(0))
            {
                return thisProcessId;
            }

            using var thisProcess = ThisProcess;
            thisProcessId = thisProcess.Id;
            return thisProcessId;
        }
    }

    private static string ThisProcessName
    {
        get
        {
            if (!string.IsNullOrEmpty(thisProcessName))
            {
                return thisProcessName;
            }

            using var thisProcess = ThisProcess;
            thisProcessName = thisProcess.ProcessName;
            return thisProcessName;
        }
    }

    private static Process ThisProcess { get; } = Process.GetCurrentProcess();

    private static string LocalApplicationDataFolder
    {
        get
        {
            if (!string.IsNullOrEmpty(localApplicationDataFolder))
            {
                return localApplicationDataFolder;
            }

            // We cannot use System.Windows.Forms.Application.LocalUserAppDataPath as it would
            // Create annoying folders, and throw annoying Exceptions making it harder to
            // debug as it spams the debugger. Also then we would not need to Replace
            // everything added to the path obtained from System.Environment.GetFolderPath.
            // Also trap devenv if it is detected as well and use the provided ApplicationName instead.
            localApplicationDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify)}{Path.DirectorySeparatorChar}{(ThisProcessName == "devenv" ? ApplicationName : ThisProcessName)}{Path.DirectorySeparatorChar}";
            if (!Directory.Exists(localApplicationDataFolder))
            {
                _ = Directory.CreateDirectory(localApplicationDataFolder);
            }

            return localApplicationDataFolder;
        }
    }
}
