namespace Elskom.Sdk.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;
using System.Runtime.InteropServices;

internal static class TaskHelpers
{
    internal static ITaskItem[]? AddKnownFrameworkReference()
    {
        var installedRuntimeVersion = GetInstalledDotNetSdkRuntimePackVersion("Elskom.Sdk.App");
        if (installedRuntimeVersion is null or "")
        {
            return null;
        }

        var knownFrameworkReference1 = new TaskItem("Elskom.Sdk.App");
        knownFrameworkReference1.SetMetadata("TargetFramework", "net6.0");
        knownFrameworkReference1.SetMetadata("RuntimeFrameworkName", "Elskom.Sdk.App");
        knownFrameworkReference1.SetMetadata("DefaultRuntimeFrameworkVersion", installedRuntimeVersion);
        knownFrameworkReference1.SetMetadata("LatestRuntimeFrameworkVersion", installedRuntimeVersion);
        knownFrameworkReference1.SetMetadata("TargetingPackName", "Elskom.Sdk.App.Ref");
        knownFrameworkReference1.SetMetadata("TargetingPackVersion", installedRuntimeVersion);
        knownFrameworkReference1.SetMetadata("RuntimePackNamePatterns", "Elskom.Sdk.App.Runtime.**RID**");
        knownFrameworkReference1.SetMetadata("RuntimePackRuntimeIdentifiers", "win-x86;win-x64;win-arm64;linux-x64;linux-arm;linux-arm64;osx-x64;osx-arm64");
        knownFrameworkReference1.SetMetadata("IsTrimmable", "true");
        var knownFrameworkReference2 = new TaskItem("Elskom.Sdk.App");
        knownFrameworkReference2.SetMetadata("TargetFramework", "net7.0");
        knownFrameworkReference2.SetMetadata("RuntimeFrameworkName", "Elskom.Sdk.App");
        knownFrameworkReference2.SetMetadata("DefaultRuntimeFrameworkVersion", installedRuntimeVersion);
        knownFrameworkReference2.SetMetadata("LatestRuntimeFrameworkVersion", installedRuntimeVersion);
        knownFrameworkReference2.SetMetadata("TargetingPackName", "Elskom.Sdk.App.Ref");
        knownFrameworkReference2.SetMetadata("TargetingPackVersion", installedRuntimeVersion);
        knownFrameworkReference2.SetMetadata("RuntimePackNamePatterns", "Elskom.Sdk.App.Runtime.**RID**");
        knownFrameworkReference2.SetMetadata("RuntimePackRuntimeIdentifiers", "win-x86;win-x64;win-arm64;linux-x64;linux-arm;linux-arm64;osx-x64;osx-arm64");
        knownFrameworkReference2.SetMetadata("IsTrimmable", "true");
        return new[] { knownFrameworkReference1, knownFrameworkReference2 };
    }

    internal static ITaskItem[] ReturnItemOrEmpty(bool add, ITaskItem[] items)
        => add switch
        {
            true => items, // new[] { item, },
            false => Array.Empty<ITaskItem>()
        };

    private static string GetInstalledDotNetSdkRuntimePackVersion(string packName)
    {
        var workloadRuntimePackFolder = GetDotNetSdkRuntimePacksFolder();
        var packPath = Path.Combine(workloadRuntimePackFolder, packName);
        if (Directory.Exists(packPath))
        {
            var di = new DirectoryInfo(packPath);
            return di.EnumerateDirectories("*", SearchOption.TopDirectoryOnly).Max(
                item => ConvertVersionToNuGetVersion(item.Name))?.ToFullString() ?? string.Empty;
        }

        return string.Empty;
    }

    private static NuGetVersion ConvertVersionToNuGetVersion(string version)
    {
        _ = NuGetVersion.TryParse(version, out var version2);
        return version2;
    }

    private static string GetDotNetSdkRuntimePacksFolder()
    {
        var sdkLocation = GetDotNetSdkLocation();
        return Path.Combine(sdkLocation, "shared");
    }

    private static string GetDotNetSdkLocation()
    {
        var knownDotNetLocations = (
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux),
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
        {
            (true, false, false) => new[]
            {
                // On 64-bit systems this will ensure that it will
                // check the 64 bit program files folder first.
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "dotnet",
                    "dotnet.exe"),
                // Only include x86 program files directory once when the system is 32-bit.
                RuntimeInformation.OSArchitecture is Architecture.X64 or Architecture.Arm64
                    ? Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                        "dotnet",
                        "dotnet.exe")
                    : string.Empty,
            },
            (false, false, true) => new[]
            {
                "/usr/local/share/dotnet/dotnet",
            },
            (false, true, false) => new[]
            {
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "share",
                    "dotnet",
                    "dotnet"),
            },
            _ => Array.Empty<string>(),
        };
        var sdkRoot = string.Empty;
        var envSdkRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
        if (envSdkRoot is not null && Directory.Exists(envSdkRoot))
        {
            sdkRoot = envSdkRoot;
        }

        if (string.IsNullOrEmpty(sdkRoot) || !Directory.Exists(sdkRoot))
        {
            sdkRoot = knownDotNetLocations
                // On 64-bit Windows, we need the 64-bit sdk location (if installed) instead.
                .Select(GetSdkFullPath)
                .First(s => !string.IsNullOrEmpty(s));
        }

        return sdkRoot;
    }

    private static string GetSdkFullPath(string loc)
    {
        var sdkRoot = string.Empty;
        if (File.Exists(loc))
        {
            var dotnet = new FileInfo(loc);
            var sdkDir = dotnet.Directory;
            if (sdkDir is not null)
            {
                sdkRoot = sdkDir.FullName;
            }
        }

        return sdkRoot;
    }
}
