// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A class that handles the settings for any application.
    /// </summary>
    public static class SettingsFile
    {
        static SettingsFile()
        {
            using var thisProcess = Process.GetCurrentProcess();
            ThisProcessName = thisProcess.ProcessName;
            ThisProcessId = thisProcess.Id;
            SettingsJson = JsonSettings.OpenFile();
            ApplicationName = "Els_kom";

            // We cannot use System.Windows.Forms.Application.LocalUserAppDataPath as it would
            // Create annoying folders, and throw annoying Exceptions making it harder to
            // debug as it spams the debugger. Also then we would not need to Replace
            // everything added to the path obtained from System.Environment.GetFolderPath.
            // Also trap devenv if it is detected as well and use the provided ApplicationName instead.
            var localApplicationDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify)}{Path.DirectorySeparatorChar}{(ThisProcessName == "devenv" ? ApplicationName : ThisProcessName)}{Path.DirectorySeparatorChar}";
            if (!Directory.Exists(localApplicationDataFolder))
            {
                _ = Directory.CreateDirectory(localApplicationDataFolder);
            }

            SettingsPath = $"{localApplicationDataFolder}Settings.json";
            ErrorLogPath = $"{localApplicationDataFolder}{ThisProcessName}-{ThisProcessId}.log";

            // On Non-Windows OS's all crash dumps must be named "core.{PID}"!!!
            MiniDumpPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{localApplicationDataFolder}{ThisProcessName}-{ThisProcessId}.mdmp" : $"{localApplicationDataFolder}core.{ThisProcessId}";
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
        public static JsonSettings SettingsJson { get; set; }

        /// <summary>
        /// Gets or sets the Application's name to use for Resolving the paths for the
        /// Settings file, error logs, and minidumps.
        /// </summary>
        public static string ApplicationName { get; set; }

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
        public static string SettingsPath { get; }

        /// <summary>
        /// Gets the path to the Application Error Log file.
        ///
        /// Creates the Error Log file if needed.
        /// </summary>
        public static string ErrorLogPath { get; }

        /// <summary>
        /// Gets the path to the Application Mini-Dump file.
        /// </summary>
        public static string MiniDumpPath { get; }

        internal static string ThisProcessName { get; }

        internal static int ThisProcessId { get; }
    }
}
