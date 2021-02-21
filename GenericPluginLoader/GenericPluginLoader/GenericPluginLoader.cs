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
    using System.IO;
    using System.IO.Compression;
#if !NETCOREAPP2_0 && !NETSTANDARD
    using System.Linq;
#endif
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

#if NET5_0_OR_GREATER
        /// <summary>
        /// Loads plugins with the specified plugin interface type.
        /// </summary>
        /// <param name="path">
        /// The path to look for plugins to load.
        /// </param>
        /// <param name="contexts">
        /// The contexts created by the loader.
        /// </param>
        /// <param name="saveToZip">
        /// Tells this function to see if the plugin was saved to a zip file and it's pdb file as well.
        /// </param>
        /// <returns>
        /// A list of plugins loaded that derive from the specified type.
        /// </returns>
        public ICollection<T> LoadPlugins(string path, out List<PluginLoadContext> contexts, bool saveToZip = false)
#else
        /// <summary>
        /// Loads plugins with the specified plugin interface type.
        /// </summary>
        /// <param name="path">
        /// The path to look for plugins to load.
        /// </param>
        /// <param name="domains">The <see cref="AppDomain"/>s created by the loader.</param>
        /// <param name="saveToZip">
        /// Tells this function to see if the plugin was saved to a zip file and it's pdb file as well.
        /// </param>
        /// <returns>
        /// A list of plugins loaded that derive from the specified type.
        /// </returns>
        public ICollection<T> LoadPlugins(string path, out List<AppDomain> domains, bool saveToZip = false)
#endif
        {
            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");
            }

            // try to load from a zip as well if plugins are installed in both places.
            var zippath = $"{path}.zip";
            ICollection<T> plugins = new List<T>();
#if NET5_0_OR_GREATER
            contexts = new List<PluginLoadContext>();
#elif NET45
            domains = new List<AppDomain>();
#endif

            // handle when path points to a zip file.
            if (Directory.Exists(path) || File.Exists(zippath))
            {
                ICollection<Assembly> assemblies = new List<Assembly>();
                if (dllFileNames != null)
                {
                    foreach (var dllFile in dllFileNames)
                    {
#if NET5_0_OR_GREATER
                        var context = new PluginLoadContext($"Plugin#{dllFileNames.ToList().IndexOf(dllFile)}", path);
#else
                        var domain = AppDomain.CreateDomain($"Plugin#{dllFileNames.ToList().IndexOf(dllFile)}");
#endif
                        try
                        {
                            var asmFile = File.ReadAllBytes(dllFile);

                            // We need to handle the case where the pdb does not exist and where the
                            // symbols might actually be embedded inside the dll instead or simply does
                            // not exist yet.
#if NET5_0_OR_GREATER
                            var pdbFile = Debugger.IsAttached && File.Exists(dllFile.Replace("dll", "pdb", StringComparison.Ordinal))
                                ? File.ReadAllBytes(
                                    dllFile.Replace("dll", "pdb", StringComparison.Ordinal)) : null;
                            using var ms1 = new MemoryStream(asmFile);
                            using var ms2 = new MemoryStream(pdbFile);
                            var assembly = Debugger.IsAttached && pdbFile != null ?
                                context.LoadFromStream(ms1, ms2) :
                                context.LoadFromStream(ms1);
                            contexts.Add(context);
#else
                            var pdbFile = Debugger.IsAttached && File.Exists(dllFile.Replace("dll", "pdb"))
                                ? File.ReadAllBytes(dllFile.Replace("dll", "pdb")) : null;
                            var assembly = Debugger.IsAttached && pdbFile != null ?
                                domain.Load(asmFile, pdbFile) :
                                domain.Load(asmFile);
                            domains.Add(domain);
#endif
                            assemblies.Add(assembly);
                        }
                        catch (BadImageFormatException)
                        {
                            // ignore the error and load the other files.
                        }
                        catch (FileLoadException)
                        {
#if NET5_0_OR_GREATER
                            var assembly = context.LoadFromAssemblyPath(dllFile);
                            contexts.Add(context);
#elif NET45
                            var assembly = domain.Load(File.ReadAllBytes(dllFile));
#else
                            var assembly = Assembly.LoadFrom(dllFile);
#endif
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
                    using var zipFile = ZipFile.OpenRead(zippath);
                    foreach (var entry in zipFile.Entries)
                    {
                        // just lookup the dlls here. The LoadFromZip method will load the pdb’s if they are deemed needed.
                        if (entry.FullName.EndsWith(".dll", StringComparison.Ordinal))
                        {
#if NET5_0_OR_GREATER
                            var context = new PluginLoadContext($"ZipPlugin#{zipFile.Entries.IndexOf(entry)}", path);
                            var assembly = ZipAssembly.LoadFromZip(zippath, entry.FullName, context);
                            contexts.Add(context);
#else
                            var domain = AppDomain.CreateDomain($"ZipPlugin#{zipFile.Entries.IndexOf(entry)}");
                            var assembly = ZipAssembly.LoadFromZip(zippath, entry.FullName, domain);
                            domains.Add(domain);
#endif
                            if (assembly != null)
                            {
                                assemblies.Add(assembly);
                            }
                        }
                    }
                }

                var pluginType = typeof(T);
                var pluginTypes = new List<Type>();
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

                ICollection<int> toRemove = new List<int>();
                foreach (var type in pluginTypes)
                {
                    try
                    {
                        var plugin = (T)Activator.CreateInstance(type);
                        plugins.Add(plugin);
                    }
                    catch (MissingMethodException)
                    {
                        toRemove.Add(pluginTypes.IndexOf(type));
                        var index = 0;
#if NET5_0_OR_GREATER
                        foreach (var context in contexts)
                        {
                            if (context.Assemblies.Contains(type.Assembly) && context.IsCollectible)
                            {
                                index = contexts.IndexOf(context);
                                context.Unload();
                            }
                        }
#else
                        foreach (var domain in domains)
                        {
                            if (domain.GetAssemblies().ToList().Contains(type.Assembly))
                            {
                                index = domains.IndexOf(domain);
                                AppDomain.Unload(domain);
                            }
                        }
#endif

                        if (index > -1)
                        {
#if NET5_0_OR_GREATER
                            contexts.RemoveAt(index);
#else
                            domains.RemoveAt(index);
#endif
                        }
                    }
                }

                foreach (var indexToRemove in toRemove)
                {
                    if (indexToRemove > -1)
                    {
                        pluginTypes.RemoveAt(indexToRemove);
                    }
                }

                toRemove.Clear();
                return plugins;
            }

            return plugins;
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Unloads all of the plugins using their contexts.
        /// </summary>
        /// <param name="contexts">The list returned from LoadPlugins to unload all plugins loaded.</param>
        public void UnloadPlugins(List<PluginLoadContext> contexts)
        {
            foreach (var context in contexts)
            {
                if (context.IsCollectible)
                {
                    context.Unload();
                }
            }
        }
#else
        /// <summary>
        /// Unloads all of the plugins using their <see cref="AppDomain"/>s.
        /// </summary>
        /// <param name="domains">The list returned from LoadPlugins to unload all plugins loaded.</param>
        public void UnloadPlugins(List<AppDomain> domains)
        {
            foreach (var domain in domains)
            {
                AppDomain.Unload(domain);
            }
        }
#endif

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
