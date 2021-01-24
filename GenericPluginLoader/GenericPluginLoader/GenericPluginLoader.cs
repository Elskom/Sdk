// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

/*
 Credits to this file goes to the C# plugin
 loader & interface examples.
*/
namespace Elskom.Generic.Libs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// A generic loader for plugins.
    /// </summary>
    /// <typeparam name="T">The type to look for when loading plugins.</typeparam>
    public class GenericPluginLoader<T>
    {
        /// <summary>
        /// Triggers when the Plugin Loader has a message to send to the application.
        /// </summary>
        public static event EventHandler<MessageEventArgs> PluginLoaderMessage;

        /// <summary>
        /// Loads plugins with the specified plugin interface type.
        /// </summary>
        /// <param name="path">
        /// The path to look for plugins to load.
        /// </param>
        /// <param name="saveToZip">
        /// Tells this function to see if the plugin was saved to a zip file and it's pdb file as well.
        /// </param>
        /// <param name="loadPDBFile">
        /// Tells the method to load the plugins pdb file or not. Ignored and loaded anyway when a debugger is attached.
        /// </param>
        /// <returns>
        /// A list of plugins loaded that derive from the specified type.
        /// </returns>
        [SuppressMessage("Major Code Smell", "S3885:\"Assembly.Load\" should be used", Justification = "Would make the whole library fail if Load is used instead of LoadFrom on C++/clr dlls.")]
        public ICollection<T> LoadPlugins(string path, bool saveToZip = false, bool loadPDBFile = false)
        {
            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");
            }

            // try to load from a zip as well if plugins are installed in both places.
            var zippath = $"{path}.zip";
            ICollection<T> plugins = new List<T>();

            // handle when path points to a zip file.
            if (Directory.Exists(path) || File.Exists(zippath))
            {
                ICollection<Assembly> assemblies = new List<Assembly>();
                if (dllFileNames != null)
                {
                    foreach (var dllFile in dllFileNames)
                    {
                        var loadPDB = loadPDBFile ? loadPDBFile : Debugger.IsAttached;
                        try
                        {
                            var asmFile = File.ReadAllBytes(dllFile);
                            try
                            {
#if NETSTANDARD2_1 || NETCOREAPP || NET5_0
                                var pdbFile = loadPDB ? File.ReadAllBytes(
                                    dllFile.Replace("dll", "pdb", StringComparison.Ordinal)) : null;
#else
                                var pdbFile = loadPDB ? File.ReadAllBytes(
                                dllFile.Replace("dll", "pdb")) : null;
#endif
                                var assembly = loadPDB ?
                                    Assembly.Load(asmFile, pdbFile) :
                                    Assembly.Load(asmFile);
                                assemblies.Add(assembly);
                            }
                            catch (FileNotFoundException)
                            {
                                var assembly = Assembly.Load(asmFile);
                                assemblies.Add(assembly);
                            }
                        }
                        catch (BadImageFormatException)
                        {
                            // ignore the error and load the other files.
                        }
                        catch (FileLoadException)
                        {
                            var assembly = Assembly.LoadFrom(dllFile);
                            assemblies.Add(assembly);
                        }
                        catch (FileNotFoundException)
                        {
                            // ignore the error and load other files.
                        }
                    }
                }

                if (saveToZip && File.Exists(zippath))
                {
                    using (var zipFile = ZipFile.OpenRead(zippath))
                    {
                        foreach (var entry in zipFile.Entries)
                        {
                            // just lookup the dlls here. The LoadFromZip method will load the pdb’s if they are deemed needed.
                            if (entry.FullName.EndsWith(".dll", StringComparison.Ordinal))
                            {
                                var assembly = ZipAssembly.LoadFromZip(zippath, entry.FullName, loadPDBFile);
                                if (assembly != null)
                                {
                                    assemblies.Add(assembly);
                                }
                            }
                        }
                    }
                }

                var pluginType = typeof(T);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (var assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        try
                        {
                            var types = assembly.GetTypes();
                            foreach (var type in types)
                            {
                                if (!type.IsInterface && !type.IsAbstract && type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            var exMsg = new StringBuilder();
                            foreach (var exceptions in ex.LoaderExceptions)
                            {
                                exMsg.Append($"{exceptions.Message}{Environment.NewLine}{exceptions.StackTrace}{Environment.NewLine}");
                            }

                            PluginLoaderMessage?.Invoke(null, new MessageEventArgs(exMsg.ToString(), "Error!", ErrorLevel.Error));
                        }
                        catch (ArgumentNullException ex)
                        {
                            var exMsg = string.Empty;
                            LogException(ex, ref exMsg);

                            PluginLoaderMessage?.Invoke(null, new MessageEventArgs(exMsg, "Error!", ErrorLevel.Error));
                        }
                    }
                }

                foreach (var type in pluginTypes)
                {
                    var plugin = (T)Activator.CreateInstance(type);
                    plugins.Add(plugin);
                }

                return plugins;
            }

            return plugins;
        }

        private static void LogException(Exception ex, ref string exMsg)
        {
            exMsg += $"{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
            if (ex.InnerException != null)
            {
                LogException(ex.InnerException, ref exMsg);
            }
        }
    }
}
