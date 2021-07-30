// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs;

/// <summary>
/// Class that allows executing Elsword directly or it's launcher.
/// </summary>
public static class ExecutionManager
{
    /// <summary>
    /// Gets a value indicating whether the launcher to Elsword is running.
    /// </summary>
    /// <returns>A value indicating if Elsword is running.</returns>
    public static bool RunningElsword { get; private set; }

    /// <summary>
    /// Gets a value indicating whether Elsword is running Directly.
    /// </summary>
    /// <returns>A value indicating if Elsword is running directly.</returns>
    public static bool RunningElswordDirectly { get; private set; }

    private static string ElsDir { get; set; }

    /// <summary>
    /// Gets if Els_kom.exe is already Running. If So, Helps with Closing any new Instances.
    /// </summary>
    /// <returns>Boolean.</returns>
    public static bool IsElsKomRunning()
    {
        var els_komexe = Process.GetProcessesByName("Els_kom");
        return els_komexe.Length > 1;
    }

    /// <summary>
    /// Runs Elsword Directly.
    /// This is an blocking call that has to run in an separate thread from Els_kom's main thread.
    /// NEVER UNDER ANY CIRCUMSTANCES RUN THIS IN THE MAIN THREAD, YOU WILL DEADLOCK ELS_KOM!!!.
    /// </summary>
    public static void RunElswordDirectly()
    {
        if (File.Exists(SettingsFile.SettingsPath))
        {
            SettingsFile.SettingsJson = SettingsFile.SettingsJson.ReopenFile();
            ElsDir = SettingsFile.SettingsJson.ElsDir;
            if (ElsDir.Length > 0)
            {
                if (File.Exists($"{ElsDir}\\data\\x2.exe"))
                {
                    RunningElswordDirectly = true;
                    using (Process proc = new())
                    {
                        _ = proc.Shell(
                            $"{ElsDir}\\data\\x2.exe",
                            "pxk19slammsu286nfha02kpqnf729ck",
                            false,
                            false,
                            false,
                            false,
                            ProcessWindowStyle.Normal,
                            $"{ElsDir}\\data\\",
                            true);
                    }

                    RunningElswordDirectly = false;
                }
                else
                {
                    MessageEventArgs args = new(
                        string.Format(
                            Resources.ExecutionManager_Cannot_Find_x2_exe,
                            ElsDir,
                            Path.DirectorySeparatorChar),
                        Resources.Error,
                        ErrorLevel.Error);
                    KOMManager.InvokeMessageEvent(args);
                    ProcessExtensions.Executing = false;
                }
            }
            else
            {
                MessageEventArgs args = new(
                    string.Format(
                        Resources.ExecutionManager_ElsDir_Not_Set,
                        "Test your mods"),
                    Resources.Error,
                    ErrorLevel.Error);
                KOMManager.InvokeMessageEvent(args);
                ProcessExtensions.Executing = false;
            }
        }
        else
        {
            MessageEventArgs args = new(
                string.Format(
                    Resources.ExecutionManager_ElsDir_Not_Set,
                    "Test your mods"),
                Resources.Error,
                ErrorLevel.Error);
            KOMManager.InvokeMessageEvent(args);
            ProcessExtensions.Executing = false;
        }
    }

    /// <summary>
    /// Runs Elsword Launcher.
    /// This is an blocking call that has to run in an separate thread from Els_kom's main thread.
    /// NEVER UNDER ANY CIRCUMSTANCES RUN THIS IN THE MAIN THREAD, YOU WILL DEADLOCK ELS_KOM!!!.
    /// </summary>
    public static void RunElswordLauncher()
    {
        // for the sake of sanity and the need to disable the pack, unpack, and test mods
        // buttons in UI while updating game.
        if (File.Exists(SettingsFile.SettingsPath))
        {
            SettingsFile.SettingsJson = SettingsFile.SettingsJson.ReopenFile();
            ElsDir = SettingsFile.SettingsJson.ElsDir;
            if (ElsDir.Length > 0)
            {
                if (File.Exists($"{ElsDir}\\voidels.exe"))
                {
                    RunningElsword = true;
                    using (Process proc = new())
                    {
                        _ = proc.Shell(
                            $"{ElsDir}\\voidels.exe",
                            string.Empty,
                            false,
                            false,
                            false,
                            false,
                            ProcessWindowStyle.Normal,
                            ElsDir,
                            true);
                    }

                    RunningElsword = false;
                }
                else
                {
                    if (File.Exists($"{ElsDir}\\elsword.exe"))
                    {
                        RunningElsword = true;
                        using (Process proc = new())
                        {
                            _ = proc.Shell(
                                $"{ElsDir}\\elsword.exe",
                                string.Empty,
                                false,
                                false,
                                false,
                                false,
                                ProcessWindowStyle.Normal,
                                ElsDir,
                                true);
                        }

                        RunningElsword = false;
                    }
                    else
                    {
                        MessageEventArgs args = new(
                            string.Format(
                                Resources.ExecutionManager_Cannot_Find_elsword_exe,
                                ElsDir,
                                Path.DirectorySeparatorChar),
                            Resources.Error,
                            ErrorLevel.Error);
                        KOMManager.InvokeMessageEvent(args);
                        ProcessExtensions.Executing = false;
                    }
                }
            }
            else
            {
                MessageEventArgs args = new(
                    string.Format(
                        Resources.ExecutionManager_ElsDir_Not_Set,
                        "update Elsword"),
                    Resources.Error,
                    ErrorLevel.Error);
                KOMManager.InvokeMessageEvent(args);
                ProcessExtensions.Executing = false;
            }
        }
        else
        {
            MessageEventArgs args = new(
                string.Format(
                    Resources.ExecutionManager_ElsDir_Not_Set,
                    "update Elsword"),
                Resources.Error,
                ErrorLevel.Error);
            KOMManager.InvokeMessageEvent(args);
            ProcessExtensions.Executing = false;
        }
    }
}
