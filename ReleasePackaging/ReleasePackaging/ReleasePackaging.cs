// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    /// <summary>
    /// Handles application's release packaging command line.
    /// </summary>
    public static class ReleasePackaging
    {
        /// <summary>
        /// Packages an application's Release build to a zip file.
        /// </summary>
        /// <param name="args">The command line arguments passed into the calling process.</param>
        public static void PackageRelease(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            string outfilename;
            if (args[1].StartsWith(".\\", StringComparison.OrdinalIgnoreCase))
            {
                // Replace spaces with periods.
                outfilename = ReplaceStr(args[1], " ", ".", StringComparison.OrdinalIgnoreCase);
                args[1] = ReplaceStr(outfilename, ".\\", $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase);
            }
            else if (args[1].StartsWith("./", StringComparison.OrdinalIgnoreCase))
            {
                // Replace spaces with periods.
                outfilename = ReplaceStr(args[1], " ", ".", StringComparison.OrdinalIgnoreCase);
                args[1] = ReplaceStr(outfilename, "./", $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Replace spaces with periods.
                outfilename = ReplaceStr(args[1], " ", ".", StringComparison.OrdinalIgnoreCase);
                args[1] = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{outfilename}";
            }

            if (args[0].Equals("-p", StringComparison.Ordinal))
            {
                Console.WriteLine($"Writing build files and debug symbol files to {outfilename}.");
                if (File.Exists(args[1]))
                {
                    File.Delete(args[1]);
                }

                using (var zipFile = ZipFile.Open(args[1], ZipArchiveMode.Update))
                {
                    var di1 = new DirectoryInfo(Directory.GetCurrentDirectory());
                    foreach (var fi1 in GetAllFilesWithExtensions(di1, new string[] { "*.exe", "*.dll", "*.xml", "*.txt", "*.pdb" }, new string[] { ".CodeAnalysisLog.xml" }))
                    {
                        _ = zipFile.CreateEntryFromFile(fi1.Name, fi1.Name);
                    }

                    foreach (var di2 in di1.GetDirectories())
                    {
                        foreach (var fi2 in GetAllFilesWithExtensions(di2, new string[] { "*.pdb", "*.dll", "*.xml", "*.txt" }, new string[] { ".CodeAnalysisLog.xml" }))
                        {
                            _ = zipFile.CreateEntryFromFile($"{di2.Name}{Path.DirectorySeparatorChar}{fi2.Name}", $"{di2.Name}{Path.DirectorySeparatorChar}{fi2.Name}");
                        }
                    }
                }
            }
        }

        private static FileInfo[] GetAllFilesWithExtensions(DirectoryInfo dinfo, string[] extensions, string[] excludeFiles)
        {
            var fileInfos = new List<FileInfo>();
            foreach (var extension in extensions)
            {
                var files = dinfo.GetFiles(extension).ToList();
                foreach (var file in files)
                {
                    // filter out excluded files.
                    var excluded = false;
                    foreach (var excludeFile in excludeFiles)
                    {
                        if (file.Name.EndsWith(excludeFile, StringComparison.OrdinalIgnoreCase))
                        {
                            excluded = true;
                        }
                    }

                    if (!excluded)
                    {
                        fileInfos.Add(file);
                    }
                }
            }

            return fileInfos.ToArray();
        }

        private static string ReplaceStr(string str1, string str2, string str3, StringComparison comp)
#if NETSTANDARD2_1 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
            => str1.Replace(str2, str3, comp);
#else
        {
            if (comp == StringComparison.OrdinalIgnoreCase)
            {
            }

            return str1.Replace(str2, str3);
        }
#endif
    }
}
