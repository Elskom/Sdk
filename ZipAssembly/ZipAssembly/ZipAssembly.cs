// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
#if NET5_0_OR_GREATER
    using System.Runtime.Loader;
#endif
    using System.Runtime.Serialization;
    using Elskom.Generic.Libs.Properties;

    /// <summary>
    /// Load assemblies from a zip file.
    /// </summary>
    [Serializable]
    public sealed class ZipAssembly : Assembly
    {
        // always set to Zip file full path + \\ + file path in zip.
        private string locationValue;

        private ZipAssembly(SerializationInfo serializationInfo, StreamingContext streamingContext)
            => throw new NotImplementedException();

        // hopefully this has the path to the assembly on System.Reflection.Assembly.Location output with the value from this override.

        /// <summary>
        /// Gets the location of the assembly in the zip file.
        /// </summary>
        public override string Location => this.locationValue;

#if NET5_0_OR_GREATER
        /// <summary>
        /// Loads the assembly with it’s debugging symbols
        /// from the specified zip file.
        /// </summary>
        /// <param name="zipFileName">The zip file for which to look for the assembly in.</param>
        /// <param name="assemblyName">The assembly file name to load.</param>
        /// <param name="context">The context to load the assemblies into.</param>
        /// <returns>A new <see cref="ZipAssembly"/> that represents the loaded assembly.</returns>
        /// <exception cref="ArgumentException">
        /// When <paramref name="zipFileName"/> is null, Empty, or does not exist.
        /// Or <paramref name="assemblyName"/> is null, Empty or does not end with the '.dll' extension.
        /// </exception>
        /// <exception cref="ZipAssemblyLoadException">
        /// When the assembly name specified was not found in the input zip file.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception not documented here indirectly thrown by this
        /// If any other exceptions other than the ones above is thrown from a call to this, it exposes a bug.
        /// </exception>
        public static ZipAssembly LoadFromZip(string zipFileName, string assemblyName, AssemblyLoadContext context)
#else
        /// <summary>
        /// Loads the assembly with it’s debugging symbols
        /// from the specified zip file.
        /// </summary>
        /// <param name="zipFileName">The zip file for which to look for the assembly in.</param>
        /// <param name="assemblyName">The assembly file name to load.</param>
        /// <param name="domain">The <see cref="AppDomain"/> to load the assemblies into.</param>
        /// <returns>A new <see cref="ZipAssembly"/> that represents the loaded assembly.</returns>
        /// <exception cref="ArgumentException">
        /// When <paramref name="zipFileName"/> is null, Empty, or does not exist.
        /// Or <paramref name="assemblyName"/> is null, Empty or does not end with the '.dll' extension.
        /// </exception>
        /// <exception cref="ZipAssemblyLoadException">
        /// When the assembly name specified was not found in the input zip file.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception not documented here indirectly thrown by this
        /// If any other exceptions other than the ones above is thrown from a call to this, it exposes a bug.
        /// </exception>
        public static ZipAssembly LoadFromZip(string zipFileName, string assemblyName, AppDomain domain)
#endif
        {
            if (string.IsNullOrWhiteSpace(zipFileName))
            {
                throw new ArgumentException(string.Format(Resources.ZipAssembly_not_allowed_to_be_empty!, nameof(zipFileName)), nameof(zipFileName));
            }

            if (!File.Exists(zipFileName))
            {
                throw new ArgumentException(string.Format(Resources.ZipAssembly_does_not_exist!, nameof(zipFileName)), nameof(zipFileName));
            }

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentException(string.Format(Resources.ZipAssembly_not_allowed_to_be_empty!, nameof(assemblyName)), nameof(assemblyName));
            }

            if (!assemblyName.EndsWith(".dll", StringComparison.Ordinal))
            {
                // setting pdbFileName fails or makes unpredicted/unwanted things if this is not checked
                throw new ArgumentException(string.Format(Resources.ZipAssembly_must_end_with_dll!, nameof(assemblyName)), nameof(assemblyName));
            }

            // check if the assembly is in the zip file.
            // If it is, get it’s bytes then load it.
            // If not throw an exception. Also throw
            // an exception if the pdb file is requested but not found.
            bool found;
            string zipAssemblyName;
            var pdbAssemblyName = string.Empty;
            byte[] asmbytes;
            byte[] pdbbytes = null;
            using (var zipFile = ZipFile.OpenRead(zipFileName))
            {
                GetBytesFromZipFile(assemblyName, zipFile, out asmbytes, out found, out zipAssemblyName);
                if (Debugger.IsAttached)
                {
                    var pdbFileName = assemblyName.Replace("dll", "pdb");
                    GetBytesFromZipFile(pdbFileName, zipFile, out pdbbytes, out _, out pdbAssemblyName);
                }
            }

            if (!found)
            {
                throw new ZipAssemblyLoadException(Resources.ZipAssembly_Assembly_specified_not_found);
            }

            // always load pdb when debugging (automatically loaded when embedded however).
            // PDB should be automatically downloaded to zip file always
            // and really *should* always be present unless embedded inside of the dll file.
            try
            {
#if NET5_0_OR_GREATER
                using MemoryStream ms1 = new(asmbytes);
                MemoryStream ms2 = pdbbytes is not null ? new(pdbbytes) : null;
                var zipassembly = Debugger.IsAttached && pdbbytes is not null ?
                    (ZipAssembly)context.LoadFromStream(ms1, ms2) :
                    (ZipAssembly)context.LoadFromStream(ms1);
                ms2?.Dispose();
#else
                var zipassembly = (ZipAssembly)(Assembly)domain.Load(asmbytes, Debugger.IsAttached ? pdbbytes : null);
#endif
                zipassembly.locationValue = $"{zipFileName}{Path.DirectorySeparatorChar}{zipAssemblyName}";
                return zipassembly;
            }
            catch (BadImageFormatException)
            {
                // ignore the error and load the other files.
                return null;
            }
            catch (FileLoadException)
            {
                var tmpDir = Path.GetTempPath();
                using (var dllfs = File.Create($"{tmpDir}{zipAssemblyName}"))
                {
                    dllfs.Write(asmbytes, 0, asmbytes.Length);
                }

                if (Debugger.IsAttached && pdbbytes is not null)
                {
                    using var pdbfs = File.Create($"{tmpDir}{pdbAssemblyName}");
                    pdbfs.Write(pdbbytes, 0, pdbbytes.Length);
                }

#if NET5_0_OR_GREATER
                var zipassembly = (ZipAssembly)context.LoadFromAssemblyPath($"{tmpDir}{zipAssemblyName}");
#else
                var zipassembly = (ZipAssembly)(Assembly)domain.Load(File.ReadAllBytes($"{tmpDir}{zipAssemblyName}"));
#endif
                zipassembly.locationValue = $"{tmpDir}{zipAssemblyName}";
                return zipassembly;
            }
        }

        private static void GetBytesFromZipFile(string entryName, ZipArchive zipFile, out byte[] bytes, out bool found, out string assemblyName)
        {
            var assemblyEntry = zipFile.Entries.FirstOrDefault(e => e.FullName.Equals(entryName, StringComparison.OrdinalIgnoreCase));
            assemblyName = string.Empty;
            found = false;
            bytes = null;
            if (assemblyEntry is not null)
            {
                assemblyName = assemblyEntry.FullName;
                found = true;
                using var strm = assemblyEntry.Open();
                using MemoryStream ms = new();
                strm.CopyTo(ms);
                bytes = ms.ToArray();
            }
        }
    }
}
