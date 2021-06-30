// Copyright (c) 2018-2021, Els_kom org.
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
    using Elskom.Generic.Libs.Properties;

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
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            string outfilename;
            if (args[1].StartsWith(".\\", StringComparison.OrdinalIgnoreCase))
            {
                // Replace spaces with periods.
                outfilename = args[1].ReplaceStr(" ", ".", StringComparison.OrdinalIgnoreCase);
                args[1] = outfilename.ReplaceStr(".\\", $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase);
            }
            else if (args[1].StartsWith("./", StringComparison.OrdinalIgnoreCase))
            {
                // Replace spaces with periods.
                outfilename = args[1].ReplaceStr(" ", ".", StringComparison.OrdinalIgnoreCase);
                args[1] = outfilename.ReplaceStr("./", $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Replace spaces with periods.
                outfilename = args[1].ReplaceStr(" ", ".", StringComparison.OrdinalIgnoreCase);
                args[1] = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{outfilename.ReplaceStr(".", string.Empty, StringComparison.OrdinalIgnoreCase)}";
            }

            if (args[0].Equals("-p", StringComparison.Ordinal))
            {
                Console.WriteLine(Resources.ReleasePackaging_Write_output, outfilename);
                if (File.Exists(args[1]))
                {
                    File.Delete(args[1]);
                }

                using var zipFile = ZipFile.Open(args[1], ZipArchiveMode.Update);
                DirectoryInfo di1 = new(Directory.GetCurrentDirectory());
                var extensions = new[] { "*.exe", "*.dll", "*.xml", "*.txt", "*.pdb" };
                Span<char> excludeFile = stackalloc char[]
                {
                    '.', 'C', 'o', 'd', 'e', 'A', 'n', 'a', 'l', 'y', 's', 'i', 's',
                    'L', 'o', 'g', '.', 'x', 'm', 'l',
                };
                foreach (var fi1 in GetAllFilesWithExtensions(di1, extensions, excludeFile))
                {
                    _ = zipFile.CreateEntryFromFile(fi1.Name, fi1.Name);
                }

                foreach (var di2 in di1.GetDirectories())
                {
                    foreach (var fi2 in GetAllFilesWithExtensions(di2, extensions.AsSpan().Slice(1).ToArray(), excludeFile))
                    {
                        _ = zipFile.CreateEntryFromFile($"{di2.Name}{Path.DirectorySeparatorChar}{fi2.Name}", $"{di2.Name}{Path.DirectorySeparatorChar}{fi2.Name}");
                    }
                }
            }
        }

        private static FileInfo[] GetAllFilesWithExtensions(DirectoryInfo dinfo, string[] extensions, ReadOnlySpan<char> excludeFile)
        {
            List<FileInfo> fileInfos = new();
            foreach (var extension in extensions)
            {
                var files = dinfo.GetFiles(extension).ToList();
                foreach (var file in files)
                {
                    // filter out excluded files.
                    var excluded = file.Name.EndsWith(excludeFile.ToString(), StringComparison.OrdinalIgnoreCase);
                    if (!excluded)
                    {
                        fileInfos.Add(file);
                    }
                }
            }

            return fileInfos.ToArray();
        }
    }
}
