// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

#if WITH_CUSTOM_ENVIRONMENT
namespace Elskom.Generic.Libs
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;

    internal static class Environment
    {
        internal enum SpecialFolder
        {
            ApplicationData = SpecialFolderValues.CSIDLAPPDATA,
            CommonApplicationData = SpecialFolderValues.CSIDLCOMMONAPPDATA,
            LocalApplicationData = SpecialFolderValues.CSIDLLOCALAPPDATA,
            Cookies = SpecialFolderValues.CSIDLCOOKIES,
            Desktop = SpecialFolderValues.CSIDLDESKTOP,
            Favorites = SpecialFolderValues.CSIDLFAVORITES,
            History = SpecialFolderValues.CSIDLHISTORY,
            InternetCache = SpecialFolderValues.CSIDLINTERNETCACHE,
            Programs = SpecialFolderValues.CSIDLPROGRAMS,
            MyComputer = SpecialFolderValues.CSIDLDRIVES,
            MyMusic = SpecialFolderValues.CSIDLMYMUSIC,
            MyPictures = SpecialFolderValues.CSIDLMYPICTURES,
            MyVideos = SpecialFolderValues.CSIDLMYVIDEO,
            Recent = SpecialFolderValues.CSIDLRECENT,
            SendTo = SpecialFolderValues.CSIDLSENDTO,
            StartMenu = SpecialFolderValues.CSIDLSTARTMENU,
            Startup = SpecialFolderValues.CSIDLSTARTUP,
            System = SpecialFolderValues.CSIDLSYSTEM,
            Templates = SpecialFolderValues.CSIDLTEMPLATES,
            DesktopDirectory = SpecialFolderValues.CSIDLDESKTOPDIRECTORY,
            Personal = SpecialFolderValues.CSIDLPERSONAL,
            MyDocuments = SpecialFolderValues.CSIDLPERSONAL,
            ProgramFiles = SpecialFolderValues.CSIDLPROGRAMFILES,
            CommonProgramFiles = SpecialFolderValues.CSIDLPROGRAMFILESCOMMON,
            AdminTools = SpecialFolderValues.CSIDLADMINTOOLS,
            CDBurning = SpecialFolderValues.CSIDLCDBURNAREA,
            CommonAdminTools = SpecialFolderValues.CSIDLCOMMONADMINTOOLS,
            CommonDocuments = SpecialFolderValues.CSIDLCOMMONDOCUMENTS,
            CommonMusic = SpecialFolderValues.CSIDLCOMMONMUSIC,
            CommonOemLinks = SpecialFolderValues.CSIDLCOMMONOEMLINKS,
            CommonPictures = SpecialFolderValues.CSIDLCOMMONPICTURES,
            CommonStartMenu = SpecialFolderValues.CSIDLCOMMONSTARTMENU,
            CommonPrograms = SpecialFolderValues.CSIDLCOMMONPROGRAMS,
            CommonStartup = SpecialFolderValues.CSIDLCOMMONSTARTUP,
            CommonDesktopDirectory = SpecialFolderValues.CSIDLCOMMONDESKTOPDIRECTORY,
            CommonTemplates = SpecialFolderValues.CSIDLCOMMONTEMPLATES,
            CommonVideos = SpecialFolderValues.CSIDLCOMMONVIDEO,
            Fonts = SpecialFolderValues.CSIDLFONTS,
            NetworkShortcuts = SpecialFolderValues.CSIDLNETHOOD,
            PrinterShortcuts = SpecialFolderValues.CSIDLPRINTHOOD,
            UserProfile = SpecialFolderValues.CSIDLPROFILE,
            CommonProgramFilesX86 = SpecialFolderValues.CSIDLPROGRAMFILESCOMMONX86,
            ProgramFilesX86 = SpecialFolderValues.CSIDLPROGRAMFILESX86,
            Resources = SpecialFolderValues.CSIDLRESOURCES,
            LocalizedResources = SpecialFolderValues.CSIDLRESOURCESLOCALIZED,
            SystemX86 = SpecialFolderValues.CSIDLSYSTEMX86,
            Windows = SpecialFolderValues.CSIDLWINDOWS,
        }

        internal enum SpecialFolderOption
        {
            None = 0,
            Create = SpecialFolderOptionValues.CSIDLFLAGCREATE,
            DoNotVerify = SpecialFolderOptionValues.CSIDLFLAGDONTVERIFY,
        }

        internal enum Error
        {
            // These values were defined in src/Native/System.Native/fxerrno.h
            //
            // They compare against values obtained via Interop.Sys.GetLastError() not Marshal.GetLastWin32Error()
            // which obtains the raw errno that varies between unixes. The strong typing as an enum is meant to
            // prevent confusing the two. Casting to or from int is suspect. Use GetLastErrorInfo() if you need to
            // correlate these to the underlying platform values or obtain the corresponding error message.
            SUCCESS = 0,

            E2BIG = 0x10001,           // Argument list too long.
            EACCES = 0x10002,           // Permission denied.
            EADDRINUSE = 0x10003,           // Address in use.
            EADDRNOTAVAIL = 0x10004,           // Address not available.
            EAFNOSUPPORT = 0x10005,           // Address family not supported.
            EAGAIN = 0x10006,           // Resource unavailable, try again (same value as EWOULDBLOCK),
            EALREADY = 0x10007,           // Connection already in progress.
            EBADF = 0x10008,           // Bad file descriptor.
            EBADMSG = 0x10009,           // Bad message.
            EBUSY = 0x1000A,           // Device or resource busy.
            ECANCELED = 0x1000B,           // Operation canceled.
            ECHILD = 0x1000C,           // No child processes.
            ECONNABORTED = 0x1000D,           // Connection aborted.
            ECONNREFUSED = 0x1000E,           // Connection refused.
            ECONNRESET = 0x1000F,           // Connection reset.
            EDEADLK = 0x10010,           // Resource deadlock would occur.
            EDESTADDRREQ = 0x10011,           // Destination address required.
            EDOM = 0x10012,           // Mathematics argument out of domain of function.
            EDQUOT = 0x10013,           // Reserved.
            EEXIST = 0x10014,           // File exists.
            EFAULT = 0x10015,           // Bad address.
            EFBIG = 0x10016,           // File too large.
            EHOSTUNREACH = 0x10017,           // Host is unreachable.
            EIDRM = 0x10018,           // Identifier removed.
            EILSEQ = 0x10019,           // Illegal byte sequence.
            EINPROGRESS = 0x1001A,           // Operation in progress.
            EINTR = 0x1001B,           // Interrupted function.
            EINVAL = 0x1001C,           // Invalid argument.
            EIO = 0x1001D,           // I/O error.
            EISCONN = 0x1001E,           // Socket is connected.
            EISDIR = 0x1001F,           // Is a directory.
            ELOOP = 0x10020,           // Too many levels of symbolic links.
            EMFILE = 0x10021,           // File descriptor value too large.
            EMLINK = 0x10022,           // Too many links.
            EMSGSIZE = 0x10023,           // Message too large.
            EMULTIHOP = 0x10024,           // Reserved.
            ENAMETOOLONG = 0x10025,           // Filename too long.
            ENETDOWN = 0x10026,           // Network is down.
            ENETRESET = 0x10027,           // Connection aborted by network.
            ENETUNREACH = 0x10028,           // Network unreachable.
            ENFILE = 0x10029,           // Too many files open in system.
            ENOBUFS = 0x1002A,           // No buffer space available.
            ENODEV = 0x1002C,           // No such device.
            ENOENT = 0x1002D,           // No such file or directory.
            ENOEXEC = 0x1002E,           // Executable file format error.
            ENOLCK = 0x1002F,           // No locks available.
            ENOLINK = 0x10030,           // Reserved.
            ENOMEM = 0x10031,           // Not enough space.
            ENOMSG = 0x10032,           // No message of the desired type.
            ENOPROTOOPT = 0x10033,           // Protocol not available.
            ENOSPC = 0x10034,           // No space left on device.
            ENOSYS = 0x10037,           // Function not supported.
            ENOTCONN = 0x10038,           // The socket is not connected.
            ENOTDIR = 0x10039,           // Not a directory or a symbolic link to a directory.
            ENOTEMPTY = 0x1003A,           // Directory not empty.
            ENOTRECOVERABLE = 0x1003B,           // State not recoverable.
            ENOTSOCK = 0x1003C,           // Not a socket.
            ENOTSUP = 0x1003D,           // Not supported (same value as EOPNOTSUP).
            ENOTTY = 0x1003E,           // Inappropriate I/O control operation.
            ENXIO = 0x1003F,           // No such device or address.
            EOVERFLOW = 0x10040,           // Value too large to be stored in data type.
            EOWNERDEAD = 0x10041,           // Previous owner died.
            EPERM = 0x10042,           // Operation not permitted.
            EPIPE = 0x10043,           // Broken pipe.
            EPROTO = 0x10044,           // Protocol error.
            EPROTONOSUPPORT = 0x10045,           // Protocol not supported.
            EPROTOTYPE = 0x10046,           // Protocol wrong type for socket.
            ERANGE = 0x10047,           // Result too large.
            EROFS = 0x10048,           // Read-only file system.
            ESPIPE = 0x10049,           // Invalid seek.
            ESRCH = 0x1004A,           // No such process.
            ESTALE = 0x1004B,           // Reserved.
            ETIMEDOUT = 0x1004D,           // Connection timed out.
            ETXTBSY = 0x1004E,           // Text file busy.
            EXDEV = 0x1004F,           // Cross-device link.
            ESOCKTNOSUPPORT = 0x1005E,           // Socket type not supported.
            EPFNOSUPPORT = 0x10060,           // Protocol family not supported.
            ESHUTDOWN = 0x1006C,           // Socket shutdown.
            EHOSTDOWN = 0x10070,           // Host is down.
            ENODATA = 0x10071,           // No data available.

            // POSIX permits these to have the same value and we make them always equal so
            // that CoreFX cannot introduce a dependency on distinguishing between them that
            // would not work on all platforms.
            EOPNOTSUPP = ENOTSUP,            // Operation not supported on socket.
            EWOULDBLOCK = EAGAIN,             // Operation would block.
        }

        internal static string GetFolderPath(SpecialFolder folder) => GetFolderPath(folder, SpecialFolderOption.None);

        internal static string GetFolderPath(SpecialFolder folder, SpecialFolderOption option)
        {
            if (!Enum.IsDefined(typeof(SpecialFolder), folder))
            {
                throw new ArgumentOutOfRangeException(nameof(folder), folder, string.Format("Illegal enum value : {0}.", folder));
            }

            if (option != SpecialFolderOption.None && !Enum.IsDefined(typeof(SpecialFolderOption), option))
            {
                throw new ArgumentOutOfRangeException(nameof(option), option, string.Format("Illegal enum value : {0}.", option));
            }

            return GetFolderPathCore(folder, option);
        }

        private static string GetFolderPathCore(SpecialFolder folder, SpecialFolderOption option)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // We're using SHGetKnownFolderPath instead of SHGetFolderPath as SHGetFolderPath is
                // capped at MAX_PATH.
                //
                // Because we validate both of the input enums we shouldn't have to care about CSIDL and flag
                // definitions we haven't mapped. If we remove or loosen the checks we'd have to account
                // for mapping here (this includes tweaking as SHGetFolderPath would do).
                //
                // The only SpecialFolderOption defines we have are equivalent to KnownFolderFlags.
                string folderGuid;

                switch (folder)
                {
                    case SpecialFolder.ApplicationData:
                        folderGuid = KnownFolders.RoamingAppData;
                        break;
                    case SpecialFolder.CommonApplicationData:
                        folderGuid = KnownFolders.ProgramData;
                        break;
                    case SpecialFolder.LocalApplicationData:
                        folderGuid = KnownFolders.LocalAppData;
                        break;
                    case SpecialFolder.Cookies:
                        folderGuid = KnownFolders.Cookies;
                        break;
                    case SpecialFolder.Desktop:
                        folderGuid = KnownFolders.Desktop;
                        break;
                    case SpecialFolder.Favorites:
                        folderGuid = KnownFolders.Favorites;
                        break;
                    case SpecialFolder.History:
                        folderGuid = KnownFolders.History;
                        break;
                    case SpecialFolder.InternetCache:
                        folderGuid = KnownFolders.InternetCache;
                        break;
                    case SpecialFolder.Programs:
                        folderGuid = KnownFolders.Programs;
                        break;
                    case SpecialFolder.MyComputer:
                        folderGuid = KnownFolders.ComputerFolder;
                        break;
                    case SpecialFolder.MyMusic:
                        folderGuid = KnownFolders.Music;
                        break;
                    case SpecialFolder.MyPictures:
                        folderGuid = KnownFolders.Pictures;
                        break;
                    case SpecialFolder.MyVideos:
                        folderGuid = KnownFolders.Videos;
                        break;
                    case SpecialFolder.Recent:
                        folderGuid = KnownFolders.Recent;
                        break;
                    case SpecialFolder.SendTo:
                        folderGuid = KnownFolders.SendTo;
                        break;
                    case SpecialFolder.StartMenu:
                        folderGuid = KnownFolders.StartMenu;
                        break;
                    case SpecialFolder.Startup:
                        folderGuid = KnownFolders.Startup;
                        break;
                    case SpecialFolder.System:
                        folderGuid = KnownFolders.System;
                        break;
                    case SpecialFolder.Templates:
                        folderGuid = KnownFolders.Templates;
                        break;
                    case SpecialFolder.DesktopDirectory:
                        folderGuid = KnownFolders.Desktop;
                        break;
                    case SpecialFolder.Personal:
                        // Same as Personal
                        // case SpecialFolder.MyDocuments:
                        folderGuid = KnownFolders.Documents;
                        break;
                    case SpecialFolder.ProgramFiles:
                        folderGuid = KnownFolders.ProgramFiles;
                        break;
                    case SpecialFolder.CommonProgramFiles:
                        folderGuid = KnownFolders.ProgramFilesCommon;
                        break;
                    case SpecialFolder.AdminTools:
                        folderGuid = KnownFolders.AdminTools;
                        break;
                    case SpecialFolder.CDBurning:
                        folderGuid = KnownFolders.CDBurning;
                        break;
                    case SpecialFolder.CommonAdminTools:
                        folderGuid = KnownFolders.CommonAdminTools;
                        break;
                    case SpecialFolder.CommonDocuments:
                        folderGuid = KnownFolders.PublicDocuments;
                        break;
                    case SpecialFolder.CommonMusic:
                        folderGuid = KnownFolders.PublicMusic;
                        break;
                    case SpecialFolder.CommonOemLinks:
                        folderGuid = KnownFolders.CommonOEMLinks;
                        break;
                    case SpecialFolder.CommonPictures:
                        folderGuid = KnownFolders.PublicPictures;
                        break;
                    case SpecialFolder.CommonStartMenu:
                        folderGuid = KnownFolders.CommonStartMenu;
                        break;
                    case SpecialFolder.CommonPrograms:
                        folderGuid = KnownFolders.CommonPrograms;
                        break;
                    case SpecialFolder.CommonStartup:
                        folderGuid = KnownFolders.CommonStartup;
                        break;
                    case SpecialFolder.CommonDesktopDirectory:
                        folderGuid = KnownFolders.PublicDesktop;
                        break;
                    case SpecialFolder.CommonTemplates:
                        folderGuid = KnownFolders.CommonTemplates;
                        break;
                    case SpecialFolder.CommonVideos:
                        folderGuid = KnownFolders.PublicVideos;
                        break;
                    case SpecialFolder.Fonts:
                        folderGuid = KnownFolders.Fonts;
                        break;
                    case SpecialFolder.NetworkShortcuts:
                        folderGuid = KnownFolders.NetHood;
                        break;
                    case SpecialFolder.PrinterShortcuts:
                        folderGuid = KnownFolders.PrintersFolder;
                        break;
                    case SpecialFolder.UserProfile:
                        folderGuid = KnownFolders.Profile;
                        break;
                    case SpecialFolder.CommonProgramFilesX86:
                        folderGuid = KnownFolders.ProgramFilesCommonX86;
                        break;
                    case SpecialFolder.ProgramFilesX86:
                        folderGuid = KnownFolders.ProgramFilesX86;
                        break;
                    case SpecialFolder.Resources:
                        folderGuid = KnownFolders.ResourceDir;
                        break;
                    case SpecialFolder.LocalizedResources:
                        folderGuid = KnownFolders.LocalizedResourcesDir;
                        break;
                    case SpecialFolder.SystemX86:
                        folderGuid = KnownFolders.SystemX86;
                        break;
                    case SpecialFolder.Windows:
                        folderGuid = KnownFolders.Windows;
                        break;
                    default:
                        return string.Empty;
                }

                return GetKnownFolderPath(folderGuid, option);
            }
            else
            {
                // Get the path for the SpecialFolder
                var path = GetFolderPathCoreWithoutValidation(folder);
                Debug.Assert(path != null, "path == null");

                // If we didn't get one, or if we got one but we're not supposed to verify it,
                // or if we're supposed to verify it and it passes verification, return the path.
                if (path.Length == 0 ||
                    option == SpecialFolderOption.DoNotVerify ||
                    SafeNativeMethods.Access(path, SafeNativeMethods.AccessMode.R_OK) == 0)
                {
                    return path;
                }

                // Failed verification.  If None, then we're supposed to return an empty string.
                // If Create, we're supposed to create it and then return the path.
                if (option == SpecialFolderOption.None)
                {
                    return string.Empty;
                }
                else
                {
                    Debug.Assert(option == SpecialFolderOption.Create, "option != SpecialFolderOption.Create");
                    _ = Directory.CreateDirectory(path);
                    return path;
                }
            }
        }

        private static string GetFolderPathCoreWithoutValidation(SpecialFolder folder)
        {
            // First handle any paths that involve only static paths, avoiding the overheads of getting user-local paths.
            // https://www.freedesktop.org/software/systemd/man/file-hierarchy.html
            switch (folder)
            {
                case SpecialFolder.CommonApplicationData: return "/usr/share";
                case SpecialFolder.CommonTemplates: return "/usr/share/templates";
            }

            if (SafeNativeMethods.GetUnixName() == "OSX")
            {
                switch (folder)
                {
                    case SpecialFolder.ProgramFiles: return "/Applications";
                    case SpecialFolder.System: return "/System";
                }
            }

            // All other paths are based on the XDG Base Directory Specification:
            // https://specifications.freedesktop.org/basedir-spec/latest/
            string home;
            try
            {
                home = PersistedFiles.GetHomeDirectory();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exc)
            {
                Debug.Fail($"Unable to get home directory: {exc}");
                home = Path.GetTempPath();
            }
#pragma warning restore CA1031 // Do not catch general exception types

            Debug.Assert(!string.IsNullOrEmpty(home), "Expected non-null or empty HOME");

            // TODO: Consider caching (or precomputing and caching) all subsequent results.
            // This would significantly improve performance for repeated access, at the expense
            // of not being responsive to changes in the underlying environment variables,
            // configuration files, etc.
            switch (folder)
            {
                case SpecialFolder.UserProfile:
                case SpecialFolder.MyDocuments: // same value as Personal
                    return home;
                case SpecialFolder.ApplicationData:
                    return GetXdgConfig(home);
                case SpecialFolder.LocalApplicationData:
                    // "$XDG_DATA_HOME defines the base directory relative to which user specific data files should be stored."
                    // "If $XDG_DATA_HOME is either not set or empty, a default equal to $HOME/.local/share should be used."
                    var data = System.Environment.GetEnvironmentVariable("XDG_DATA_HOME");
                    if (string.IsNullOrEmpty(data) || data[0] != '/')
                    {
                        data = Path.Combine(home, ".local", "share");
                    }

                    return data;

                case SpecialFolder.Desktop:
                case SpecialFolder.DesktopDirectory:
                    return ReadXdgDirectory(home, "XDG_DESKTOP_DIR", "Desktop");
                case SpecialFolder.Templates:
                    return ReadXdgDirectory(home, "XDG_TEMPLATES_DIR", "Templates");
                case SpecialFolder.MyVideos:
                    return ReadXdgDirectory(home, "XDG_VIDEOS_DIR", "Videos");

                case SpecialFolder.MyMusic:
                    return SafeNativeMethods.GetUnixName() == "OSX" ? Path.Combine(home, "Music") : ReadXdgDirectory(home, "XDG_MUSIC_DIR", "Music");
                case SpecialFolder.MyPictures:
                    return SafeNativeMethods.GetUnixName() == "OSX" ? Path.Combine(home, "Pictures") : ReadXdgDirectory(home, "XDG_PICTURES_DIR", "Pictures");
                case SpecialFolder.Fonts:
                    return SafeNativeMethods.GetUnixName() == "OSX" ? Path.Combine(home, "Library", "Fonts") : Path.Combine(home, ".fonts");

                case SpecialFolder.Favorites:
                    if (SafeNativeMethods.GetUnixName() == "OSX")
                    {
                        return Path.Combine(home, "Library", "Favorites");
                    }

                    break;
                case SpecialFolder.InternetCache:
                    if (SafeNativeMethods.GetUnixName() == "OSX")
                    {
                        return Path.Combine(home, "Library", "Caches");
                    }

                    break;
            }

            // No known path for the SpecialFolder
            return string.Empty;
        }

        private static string GetXdgConfig(string home)
        {
            // "$XDG_CONFIG_HOME defines the base directory relative to which user specific configuration files should be stored."
            // "If $XDG_CONFIG_HOME is either not set or empty, a default equal to $HOME/.config should be used."
            var config = System.Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
            if (string.IsNullOrEmpty(config) || config[0] != '/')
            {
                config = Path.Combine(home, ".config");
            }

            return config;
        }

        private static string ReadXdgDirectory(string homeDir, string key, string fallback)
        {
            Debug.Assert(!string.IsNullOrEmpty(homeDir), $"Expected non-empty homeDir");
            Debug.Assert(!string.IsNullOrEmpty(key), $"Expected non-empty key");
            Debug.Assert(!string.IsNullOrEmpty(fallback), $"Expected non-empty fallback");

            var envPath = System.Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(envPath) && envPath[0] == '/')
            {
                return envPath;
            }

            // Use the user-dirs.dirs file to look up the right config.
            // Note that the docs also highlight a list of directories in which to look for this file:
            // "$XDG_CONFIG_DIRS defines the preference-ordered set of base directories to search for configuration files in addition
            //  to the $XDG_CONFIG_HOME base directory. The directories in $XDG_CONFIG_DIRS should be separated with a colon ':'. If
            //  $XDG_CONFIG_DIRS is either not set or empty, a value equal to / etc / xdg should be used."
            // For simplicity, we don't currently do that.  We can add it if/when necessary.
            var userDirsPath = Path.Combine(GetXdgConfig(homeDir), "user-dirs.dirs");
            if (SafeNativeMethods.Access(userDirsPath, SafeNativeMethods.AccessMode.R_OK) == 0)
            {
                try
                {
                    var lines = File.ReadLines(userDirsPath);
                    if (lines != null)
                    {
                        foreach (var line in lines)
                        {
                            // Example lines:
                            // XDG_DESKTOP_DIR="$HOME/Desktop"
                            // XDG_PICTURES_DIR = "/absolute/path"

                            // Skip past whitespace at beginning of line
                            var pos = 0;
                            SkipWhitespace(line, ref pos);
                            if (pos >= line.Length)
                            {
                                continue;
                            }

                            // Skip past requested key name
                            if (string.CompareOrdinal(line, pos, key, 0, key.Length) != 0)
                            {
                                continue;
                            }

                            pos += key.Length;

                            // Skip past whitespace and past '='
                            SkipWhitespace(line, ref pos);
                            if (pos >= line.Length - 4 || line[pos] != '=')
                            {
                                continue; // 4 for ="" and at least one char between quotes
                            }

                            pos++; // skip past '='

                            // Skip past whitespace and past first quote
                            SkipWhitespace(line, ref pos);
                            if (pos >= line.Length - 3 || line[pos] != '"')
                            {
                                continue; // 3 for "" and at least one char between quotes
                            }

                            pos++; // skip past opening '"'

                            // Skip past relative prefix if one exists
                            var relativeToHome = false;
                            const string RelativeToHomePrefix = "$HOME/";
                            if (string.CompareOrdinal(line, pos, RelativeToHomePrefix, 0, RelativeToHomePrefix.Length) == 0)
                            {
                                relativeToHome = true;
                                pos += RelativeToHomePrefix.Length;
                            }

                            // if not relative to home, must be absolute path
                            else if (line[pos] != '/')
                            {
                                continue;
                            }

                            // Find end of path
                            var endPos = line.IndexOf('"', pos);
                            if (endPos <= pos)
                            {
                                continue;
                            }

                            // Got we need.  Now extract it.
                            var path = line.Substring(pos, endPos - pos);
                            return relativeToHome ?
                                Path.Combine(homeDir, path) :
                                path;
                        }
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception exc)
                {
                    // assembly not found, file not found, errors reading file, etc. Just eat everything.
                    Debug.Fail($"Failed reading {userDirsPath}: {exc}");
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }

            return Path.Combine(homeDir, fallback);
        }

        private static void SkipWhitespace(string line, ref int pos)
        {
            while (pos < line.Length && char.IsWhiteSpace(line[pos]))
            {
                pos++;
            }
        }

        private static string GetKnownFolderPath(string folderGuid, SpecialFolderOption option)
        {
            var folderId = new Guid(folderGuid);

            var hr = SafeNativeMethods.SHGetKnownFolderPath(folderId, (uint)option, IntPtr.Zero, out var path);
            return hr != 0 ? string.Empty : path;
        }

        // Represents a platform-agnostic Error and underlying platform-specific errno
        internal struct ErrorInfo
        {
            private int rawErrno;

            internal ErrorInfo(int errno)
            {
                this.Error = SafeNativeMethods.ConvertErrorPlatformToPal(errno);
                this.rawErrno = errno;
            }

            internal ErrorInfo(Error error)
            {
                this.Error = error;
                this.rawErrno = -1;
            }

            internal Error Error { get; private set; }

            internal int RawErrno
                => this.rawErrno == -1 ? (this.rawErrno = SafeNativeMethods.ConvertErrorPalToPlatform(this.Error)) : this.rawErrno;

            public override string ToString()
                => string.Format(
                    "RawErrno: {0} Error: {1} GetErrorMessage: {2}", // No localization required; text is member names used for debugging purposes
                    this.RawErrno,
                    this.Error,
                    this.GetErrorMessage());

            internal string GetErrorMessage()
                => SafeNativeMethods.StrError(this.RawErrno);
        }

        // https://msdn.microsoft.com/en-us/library/windows/desktop/dd378457.aspx
        internal static class KnownFolders
        {
            // (CSIDL_ADMINTOOLS) Per user Administrative Tools
            // "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Administrative Tools"
            internal const string AdminTools = "{724EF170-A42D-4FEF-9F26-B60E846FBA4F}";

            // (CSIDL_CDBURN_AREA) Temporary Burn folder
            // "%LOCALAPPDATA%\Microsoft\Windows\Burn\Burn"
            internal const string CDBurning = "{9E52AB10-F80D-49DF-ACB8-4330F5687855}";

            // (CSIDL_COMMON_ADMINTOOLS) Common Administrative Tools
            // "%ALLUSERSPROFILE%\Microsoft\Windows\Start Menu\Programs\Administrative Tools"
            internal const string CommonAdminTools = "{D0384E7D-BAC3-4797-8F14-CBA229B392B5}";

            // (CSIDL_COMMON_OEM_LINKS) OEM Links folder
            // "%ALLUSERSPROFILE%\OEM Links"
            internal const string CommonOEMLinks = "{C1BAE2D0-10DF-4334-BEDD-7AA20B227A9D}";

            // (CSIDL_COMMON_PROGRAMS) Common Programs folder
            // "%ALLUSERSPROFILE%\Microsoft\Windows\Start Menu\Programs"
            internal const string CommonPrograms = "{0139D44E-6AFE-49F2-8690-3DAFCAE6FFB8}";

            // (CSIDL_COMMON_STARTMENU) Common Start Menu folder
            // "%ALLUSERSPROFILE%\Microsoft\Windows\Start Menu"
            internal const string CommonStartMenu = "{A4115719-D62E-491D-AA7C-E74B8BE3B067}";

            // (CSIDL_COMMON_STARTUP, CSIDL_COMMON_ALTSTARTUP) Common Startup folder
            // "%ALLUSERSPROFILE%\Microsoft\Windows\Start Menu\Programs\StartUp"
            internal const string CommonStartup = "{82A5EA35-D9CD-47C5-9629-E15D2F714E6E}";

            // (CSIDL_COMMON_TEMPLATES) Common Templates folder
            // "%ALLUSERSPROFILE%\Microsoft\Windows\Templates"
            internal const string CommonTemplates = "{B94237E7-57AC-4347-9151-B08C6C32D1F7}";

            // (CSIDL_DRIVES) Computer virtual folder
            internal const string ComputerFolder = "{0AC0837C-BBF8-452A-850D-79D08E667CA7}";

            // (CSIDL_CONNECTIONS) Network Connections virtual folder
            internal const string ConnectionsFolder = "{6F0CD92B-2E97-45D1-88FF-B0D186B8DEDD}";

            // (CSIDL_CONTROLS) Control Panel virtual folder
            internal const string ControlPanelFolder = "{82A74AEB-AEB4-465C-A014-D097EE346D63}";

            // (CSIDL_COOKIES) Cookies folder
            // "%APPDATA%\Microsoft\Windows\Cookies"
            internal const string Cookies = "{2B0F765D-C0E9-4171-908E-08A611B84FF6}";

            // (CSIDL_DESKTOP, CSIDL_DESKTOPDIRECTORY) Desktop folder
            // "%USERPROFILE%\Desktop"
            internal const string Desktop = "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}";

            // (CSIDL_MYDOCUMENTS, CSIDL_PERSONAL) Documents (My Documents) folder
            // "%USERPROFILE%\Documents"
            internal const string Documents = "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}";

            // (CSIDL_FAVORITES, CSIDL_COMMON_FAVORITES) Favorites folder
            // "%USERPROFILE%\Favorites"
            internal const string Favorites = "{1777F761-68AD-4D8A-87BD-30B759FA33DD}";

            // (CSIDL_FONTS) Fonts folder
            // "%windir%\Fonts"
            internal const string Fonts = "{FD228CB7-AE11-4AE3-864C-16F3910AB8FE}";

            // (CSIDL_HISTORY) History folder
            // "%LOCALAPPDATA%\Microsoft\Windows\History"
            internal const string History = "{D9DC8A3B-B784-432E-A781-5A1130A75963}";

            // (CSIDL_INTERNET_CACHE) Temporary Internet Files folder
            // "%LOCALAPPDATA%\Microsoft\Windows\Temporary Internet Files"
            internal const string InternetCache = "{352481E8-33BE-4251-BA85-6007CAEDCF9D}";

            // (CSIDL_INTERNET) The Internet virtual folder
            internal const string InternetFolder = "{4D9F7874-4E0C-4904-967B-40B0D20C3E4B}";

            // (CSIDL_LOCAL_APPDATA) Local folder
            // "%LOCALAPPDATA%" ("%USERPROFILE%\AppData\Local")
            internal const string LocalAppData = "{F1B32785-6FBA-4FCF-9D55-7B8E7F157091}";

            // (CSIDL_RESOURCES_LOCALIZED) Fixed localized resources folder
            // "%windir%\resources\0409" (per active codepage)
            internal const string LocalizedResourcesDir = "{2A00375E-224C-49DE-B8D1-440DF7EF3DDC}";

            // (CSIDL_MYMUSIC) Music folder
            // "%USERPROFILE%\Music"
            internal const string Music = "{4BD8D571-6D19-48D3-BE97-422220080E43}";

            // (CSIDL_NETHOOD) Network shortcuts folder "%APPDATA%\Microsoft\Windows\Network Shortcuts"
            internal const string NetHood = "{C5ABBF53-E17F-4121-8900-86626FC2C973}";

            // (CSIDL_NETWORK, CSIDL_COMPUTERSNEARME) Network virtual folder
            internal const string NetworkFolder = "{D20BEEC4-5CA8-4905-AE3B-BF251EA09B53}";

            // (CSIDL_MYPICTURES) Pictures folder "%USERPROFILE%\Pictures"
            internal const string Pictures = "{33E28130-4E1E-4676-835A-98395C3BC3BB}";

            // (CSIDL_PRINTERS) Printers virtual folder
            internal const string PrintersFolder = "{76FC4E2D-D6AD-4519-A663-37BD56068185}";

            // (CSIDL_PRINTHOOD) Printer Shortcuts folder
            // "%APPDATA%\Microsoft\Windows\Printer Shortcuts"
            internal const string PrintHood = "{9274BD8D-CFD1-41C3-B35E-B13F55A758F4}";

            // (CSIDL_PROFILE) The root users profile folder "%USERPROFILE%"
            // ("%SystemDrive%\Users\%USERNAME%")
            internal const string Profile = "{5E6C858F-0E22-4760-9AFE-EA3317B67173}";

            // (CSIDL_COMMON_APPDATA) ProgramData folder
            // "%ALLUSERSPROFILE%" ("%ProgramData%", "%SystemDrive%\ProgramData")
            internal const string ProgramData = "{62AB5D82-FDC1-4DC3-A9DD-070D1D495D97}";

            // (CSIDL_PROGRAM_FILES) Program Files folder for the current process architecture
            // "%ProgramFiles%" ("%SystemDrive%\Program Files")
            internal const string ProgramFiles = "{905e63b6-c1bf-494e-b29c-65b732d3d21a}";

            // (CSIDL_PROGRAM_FILESX86) 32 bit Program Files folder (available to both 32/64 bit processes)
            internal const string ProgramFilesX86 = "{7C5A40EF-A0FB-4BFC-874A-C0F2E0B9FA8E}";

            // (CSIDL_PROGRAM_FILES_COMMON) Common Program Files folder for the current process architecture
            // "%ProgramFiles%\Common Files"
            internal const string ProgramFilesCommon = "{F7F1ED05-9F6D-47A2-AAAE-29D317C6F066}";

            // (CSIDL_PROGRAM_FILES_COMMONX86) Common 32 bit Program Files folder (available to both 32/64 bit processes)
            internal const string ProgramFilesCommonX86 = "{DE974D24-D9C6-4D3E-BF91-F4455120B917}";

            // (CSIDL_PROGRAMS) Start menu Programs folder
            // "%APPDATA%\Microsoft\Windows\Start Menu\Programs"
            internal const string Programs = "{A77F5D77-2E2B-44C3-A6A2-ABA601054A51}";

            // (CSIDL_COMMON_DESKTOPDIRECTORY) Public Desktop folder
            // "%PUBLIC%\Desktop"
            internal const string PublicDesktop = "{C4AA340D-F20F-4863-AFEF-F87EF2E6BA25}";

            // (CSIDL_COMMON_DOCUMENTS) Public Documents folder
            // "%PUBLIC%\Documents"
            internal const string PublicDocuments = "{ED4824AF-DCE4-45A8-81E2-FC7965083634}";

            // (CSIDL_COMMON_MUSIC) Public Music folder
            // "%PUBLIC%\Music"
            internal const string PublicMusic = "{3214FAB5-9757-4298-BB61-92A9DEAA44FF}";

            // (CSIDL_COMMON_PICTURES) Public Pictures folder
            // "%PUBLIC%\Pictures"
            internal const string PublicPictures = "{B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5}";

            // (CSIDL_COMMON_VIDEO) Public Videos folder
            // "%PUBLIC%\Videos"
            internal const string PublicVideos = "{2400183A-6185-49FB-A2D8-4A392A602BA3}";

            // (CSIDL_RECENT) Recent Items folder
            // "%APPDATA%\Microsoft\Windows\Recent"
            internal const string Recent = "{AE50C081-EBD2-438A-8655-8A092E34987A}";

            // (CSIDL_BITBUCKET) Recycle Bin virtual folder
            internal const string RecycleBinFolder = "{B7534046-3ECB-4C18-BE4E-64CD4CB7D6AC}";

            // (CSIDL_RESOURCES) Resources fixed folder
            // "%windir%\Resources"
            internal const string ResourceDir = "{8AD10C31-2ADB-4296-A8F7-E4701232C972}";

            // (CSIDL_APPDATA) Roaming user application data folder
            // "%APPDATA%" ("%USERPROFILE%\AppData\Roaming")
            internal const string RoamingAppData = "{3EB685DB-65F9-4CF6-A03A-E3EF65729F3D}";

            // (CSIDL_SENDTO) SendTo folder
            // "%APPDATA%\Microsoft\Windows\SendTo"
            internal const string SendTo = "{8983036C-27C0-404B-8F08-102D10DCFD74}";

            // (CSIDL_STARTMENU) Start Menu folder
            // "%APPDATA%\Microsoft\Windows\Start Menu"
            internal const string StartMenu = "{625B53C3-AB48-4EC1-BA1F-A1EF4146FC19}";

            // (CSIDL_STARTUP, CSIDL_ALTSTARTUP) Startup folder
            // "%APPDATA%\Microsoft\Windows\Start Menu\Programs\StartUp"
            internal const string Startup = "{B97D20BB-F46A-4C97-BA10-5E3608430854}";

            // (CSIDL_SYSTEM) System32 folder
            // "%windir%\system32"
            internal const string System = "{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}";

            // (CSIDL_SYSTEMX86) X86 System32 folder
            // "%windir%\system32" or "%windir%\syswow64"
            internal const string SystemX86 = "{D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27}";

            // (CSIDL_TEMPLATES) Templates folder
            // "%APPDATA%\Microsoft\Windows\Templates"
            internal const string Templates = "{A63293E8-664E-48DB-A079-DF759E0509F7}";

            // (CSIDL_MYVIDEO) Videos folder
            // "%USERPROFILE%\Videos"
            internal const string Videos = "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}";

            // (CSIDL_WINDOWS) Windows folder "%windir%"
            internal const string Windows = "{F38BF404-1D43-42F2-9305-67DE0B28FC23}";
        }

        private static class SafeNativeMethods
        {
            internal const int COR_E_PLATFORMNOTSUPPORTED = unchecked((int)0x80131539);

            internal enum AccessMode : int
            {
                F_OK = 0,   /* Check for existence */
                X_OK = 1,   /* Check for execute */
                W_OK = 2,   /* Check for write */
                R_OK = 4,   /* Check for read */
            }

            internal static Error GetLastError()
                => ConvertErrorPlatformToPal(Marshal.GetLastWin32Error());

            internal static ErrorInfo GetLastErrorInfo()
                => new ErrorInfo(Marshal.GetLastWin32Error());

            internal static unsafe string StrError(int platformErrno)
            {
                var maxBufferLength = 1024; // should be long enough for most any UNIX error
                var buffer = stackalloc byte[maxBufferLength];
                var message = StrErrorR(platformErrno, buffer, maxBufferLength);

                if (message == null)
                {
                    // This means the buffer was not large enough, but still contains
                    // as much of the error message as possible and is guaranteed to
                    // be null-terminated. We're not currently resizing/retrying because
                    // maxBufferLength is large enough in practice, but we could do
                    // so here in the future if necessary.
                    message = buffer;
                }

                return Marshal.PtrToStringAnsi((IntPtr)message);
            }

            [DllImport("System.Native", EntryPoint = "SystemNative_GetPwUidR", SetLastError = false)]
            internal static extern unsafe int GetPwUidR(uint uid, out Passwd pwd, byte* buf, int bufLen);

            [DllImport("System.Native", EntryPoint = "SystemNative_GetPwNamR", SetLastError = false)]
            internal static extern unsafe int GetPwNamR(string name, out Passwd pwd, byte* buf, int bufLen);

            [DllImport("System.Native", EntryPoint = "SystemNative_ConvertErrorPlatformToPal")]
            internal static extern Error ConvertErrorPlatformToPal(int platformErrno);

            [DllImport("System.Native", EntryPoint = "SystemNative_ConvertErrorPalToPlatform")]
            internal static extern int ConvertErrorPalToPlatform(Error error);

            [DllImport("System.Native", EntryPoint = "SystemNative_GetEUid")]
            internal static extern uint GetEUid();

            // https://msdn.microsoft.com/en-us/library/windows/desktop/bb762188.aspx
            [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = false, BestFitMapping = false, ExactSpelling = true)]
            internal static extern int SHGetKnownFolderPath(
                [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
                uint dwFlags,
                IntPtr hToken,
                out string ppszPath);

            [DllImport("System.Native", EntryPoint = "SystemNative_Access", SetLastError = true)]
            internal static extern int Access(string path, AccessMode mode);

            internal static string GetUnixName()
            {
                var ptr = GetUnixNamePrivate();
                return Marshal.PtrToStringAnsi(ptr);
            }

            [DllImport("System.Native", EntryPoint = "SystemNative_StrErrorR")]
            private static extern unsafe byte* StrErrorR(int platformErrno, byte* buffer, int bufferSize);

            [DllImport("System.Native", EntryPoint = "SystemNative_GetUnixName")]
            private static extern IntPtr GetUnixNamePrivate();

            internal unsafe struct Passwd
            {
                internal const int InitialBufferSize = 256;

                internal byte* Name;
                internal byte* Password;
                internal uint UserId;
                internal uint GroupId;
                internal byte* UserInfo;
                internal byte* HomeDirectory;
                internal byte* Shell;
            }
        }

        private static class PersistedFiles
        {
            internal static string GetHomeDirectory()
            {
                // First try to get the user's home directory from the HOME environment variable.
                // This should work in most cases.
                var userHomeDirectory = System.Environment.GetEnvironmentVariable("HOME");
                if (!string.IsNullOrEmpty(userHomeDirectory))
                {
                    return userHomeDirectory;
                }

                // In initialization conditions, however, the "HOME" environment variable may
                // not yet be set. For such cases, consult with the password entry.
                unsafe
                {
                    // First try with a buffer that should suffice for 99% of cases.
                    // Note that, theoretically, userHomeDirectory may be null in the success case
                    // if we simply couldn't find a home directory for the current user.
                    // In that case, we pass back the null value and let the caller decide
                    // what to do.
                    const int BufLen = SafeNativeMethods.Passwd.InitialBufferSize;
                    var stackBuf = stackalloc byte[BufLen];
                    if (TryGetHomeDirectoryFromPasswd(stackBuf, BufLen, out userHomeDirectory))
                    {
                        return userHomeDirectory;
                    }

                    // Fallback to heap allocations if necessary, growing the buffer until
                    // we succeed.  TryGetHomeDirectory will throw if there's an unexpected error.
                    var lastBufLen = BufLen;
                    while (true)
                    {
                        lastBufLen *= 2;
                        var heapBuf = new byte[lastBufLen];
                        fixed (byte* buf = &heapBuf[0])
                        {
                            if (TryGetHomeDirectoryFromPasswd(buf, heapBuf.Length, out userHomeDirectory))
                            {
                                return userHomeDirectory;
                            }
                        }
                    }
                }
            }

            private static unsafe bool TryGetHomeDirectoryFromPasswd(byte* buf, int bufLen, out string path)
            {
                // Call getpwuid_r to get the passwd struct
                var error = SafeNativeMethods.GetPwUidR(SafeNativeMethods.GetEUid(), out var passwd, buf, bufLen);

                // If the call succeeds, give back the home directory path retrieved
                if (error == 0)
                {
                    Debug.Assert(passwd.HomeDirectory != null, "passwd.HomeDirectory == null");
                    path = Marshal.PtrToStringAnsi((IntPtr)passwd.HomeDirectory);
                    return true;
                }

                // If the current user's entry could not be found, give back null
                // path, but still return true as false indicates the buffer was
                // too small.
                if (error == -1)
                {
                    path = null;
                    return true;
                }

                var errorInfo = new ErrorInfo(error);

                // If the call failed because the buffer was too small, return false to
                // indicate the caller should try again with a larger buffer.
                if (errorInfo.Error == Error.ERANGE)
                {
                    path = null;
                    return false;
                }

                // Otherwise, fail.
                throw new IOException(errorInfo.GetErrorMessage(), errorInfo.RawErrno);
            }
        }

        // These values are specific to Windows and are known to SHGetFolderPath, however they are
        // also the values used in the SpecialFolderOption enum.  As such, we keep them as constants
        // with their Win32 names, but keep them here rather than in Interop.Kernel32 as they're
        // used on all platforms.
        private static class SpecialFolderOptionValues
        {
            internal const int CSIDLFLAGCREATE = 0x8000;
            internal const int CSIDLFLAGDONTVERIFY = 0x4000;
        }

        // These values are specific to Windows and are known to SHGetFolderPath, however they are
        // also the values used in the SpecialFolder enum.  As such, we keep them as constants
        // with their Win32 names, but keep them here rather than in Interop.Kernel32 as they're
        // used on all platforms.
        private static class SpecialFolderValues
        {
            internal const int CSIDLAPPDATA = 0x001a;
            internal const int CSIDLCOMMONAPPDATA = 0x0023;
            internal const int CSIDLLOCALAPPDATA = 0x001c;
            internal const int CSIDLCOOKIES = 0x0021;
            internal const int CSIDLFAVORITES = 0x0006;
            internal const int CSIDLHISTORY = 0x0022;
            internal const int CSIDLINTERNETCACHE = 0x0020;
            internal const int CSIDLPROGRAMS = 0x0002;
            internal const int CSIDLRECENT = 0x0008;
            internal const int CSIDLSENDTO = 0x0009;
            internal const int CSIDLSTARTMENU = 0x000b;
            internal const int CSIDLSTARTUP = 0x0007;
            internal const int CSIDLSYSTEM = 0x0025;
            internal const int CSIDLTEMPLATES = 0x0015;
            internal const int CSIDLDESKTOPDIRECTORY = 0x0010;
            internal const int CSIDLPERSONAL = 0x0005;
            internal const int CSIDLPROGRAMFILES = 0x0026;
            internal const int CSIDLPROGRAMFILESCOMMON = 0x002b;
            internal const int CSIDLDESKTOP = 0x0000;
            internal const int CSIDLDRIVES = 0x0011;
            internal const int CSIDLMYMUSIC = 0x000d;
            internal const int CSIDLMYPICTURES = 0x0027;

            internal const int CSIDLADMINTOOLS = 0x0030; // <user name>\Start Menu\Programs\Administrative Tools
            internal const int CSIDLCDBURNAREA = 0x003b; // USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
            internal const int CSIDLCOMMONADMINTOOLS = 0x002f; // All Users\Start Menu\Programs\Administrative Tools
            internal const int CSIDLCOMMONDOCUMENTS = 0x002e; // All Users\Documents
            internal const int CSIDLCOMMONMUSIC = 0x0035; // All Users\My Music
            internal const int CSIDLCOMMONOEMLINKS = 0x003a; // Links to All Users OEM specific apps
            internal const int CSIDLCOMMONPICTURES = 0x0036; // All Users\My Pictures
            internal const int CSIDLCOMMONSTARTMENU = 0x0016; // All Users\Start Menu
            internal const int CSIDLCOMMONPROGRAMS = 0X0017; // All Users\Start Menu\Programs
            internal const int CSIDLCOMMONSTARTUP = 0x0018; // All Users\Startup
            internal const int CSIDLCOMMONDESKTOPDIRECTORY = 0x0019; // All Users\Desktop
            internal const int CSIDLCOMMONTEMPLATES = 0x002d; // All Users\Templates
            internal const int CSIDLCOMMONVIDEO = 0x0037; // All Users\My Video
            internal const int CSIDLFONTS = 0x0014; // windows\fonts
            internal const int CSIDLMYVIDEO = 0x000e; // "My Videos" folder
            internal const int CSIDLNETHOOD = 0x0013; // %APPDATA%\Microsoft\Windows\Network Shortcuts
            internal const int CSIDLPRINTHOOD = 0x001b; // %APPDATA%\Microsoft\Windows\Printer Shortcuts
            internal const int CSIDLPROFILE = 0x0028; // %USERPROFILE% (%SystemDrive%\Users\%USERNAME%)
            internal const int CSIDLPROGRAMFILESCOMMONX86 = 0x002c; // x86 Program Files\Common on RISC
            internal const int CSIDLPROGRAMFILESX86 = 0x002a; // x86 C:\Program Files on RISC
            internal const int CSIDLRESOURCES = 0x0038; // %windir%\Resources
            internal const int CSIDLRESOURCESLOCALIZED = 0x0039; // %windir%\resources\0409 (code page)
            internal const int CSIDLSYSTEMX86 = 0x0029; // %windir%\system32
            internal const int CSIDLWINDOWS = 0x0024; // GetWindowsDirectory()
        }
    }
}
#endif
