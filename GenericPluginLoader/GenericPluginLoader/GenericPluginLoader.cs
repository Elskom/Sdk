// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

/*
 Credits to this file goes to the C# plugin
 loader & interface examples.
*/

namespace Elskom.Generic.Libs;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
#if !NET5_0_OR_GREATER
using System.Reflection;
#endif

/// <summary>
/// A generic loader for plugins.
/// </summary>
/// <typeparam name="T">The type to look for when loading plugins.</typeparam>
public class GenericPluginLoader<T>
{
    /// <summary>
    /// Triggers when the Plugin Loader has a message to send to the application.
    /// </summary>
    public static event MessageEventHandler? PluginLoaderMessage;

#if NET5_0_OR_GREATER
    /// <summary>
    /// Gets the list of <see cref="PluginLoadContext"/>s loaded by this instance.
    /// </summary>
    public List<PluginLoadContext> Contexts { get; } = new();
#else
    /// <summary>
    /// Gets the list of <see cref="AppDomain"/>s loaded by this instance.
    /// </summary>
    public List<AppDomain> Domains { get; } = new();
#endif

    /// <summary>
    /// Loads plugins with the specified plugin interface type.
    /// </summary>
    /// <param name="path">
    /// The path to look for plugins to load.
    /// </param>
    /// <returns>
    /// A list of plugins loaded that derive from the specified type.
    /// </returns>
    public ICollection<T> LoadPlugins(string path)
        => this.LoadPlugins(path, false);

    /// <summary>
    /// Loads plugins with the specified plugin interface type.
    /// </summary>
    /// <param name="path">
    /// The path to look for plugins to load.
    /// </param>
    /// <param name="saveToZip">
    /// Tells this function to see if the plugin was saved to a zip file and it's pdb file as well.
    /// </param>
    /// <returns>
    /// A list of plugins loaded that derive from the specified type.
    /// </returns>
    public ICollection<T> LoadPlugins(string path, bool saveToZip)
    {
        List<string>? dllFileNames = null;
        if (Directory.Exists(path))
        {
            dllFileNames = Directory.EnumerateFiles(path, "*.dll").ToList();
        }

        // try to load from a zip as well if plugins are installed in both places.
        var zippath = $"{path}.zip";
        List<T> plugins = new();

        // handle when path points to a zip file.
        if (Directory.Exists(path) || File.Exists(zippath))
        {
            if (dllFileNames is not null)
            {
                foreach (var dllFile in dllFileNames)
                {
#if NET5_0_OR_GREATER
                    PluginLoadContext context = new($"Plugin#{dllFileNames.IndexOf(dllFile)}", path);
                    var instances = context.CreateInstancesFromInterface<T>(
                        dllFile,
                        dllFile.Replace(".dll", ".pdb", StringComparison.InvariantCulture));
                    context.UnloadIfNoInstances(instances);
#else
                    var domain = AppDomain.CreateDomain($"Plugin#{dllFileNames.IndexOf(dllFile)}");
                    domain.AssemblyResolve += this.Domain_AssemblyResolve;
                    var instances = domain.CreateInstancesFromInterface<T>(dllFile, dllFile.Replace(".dll", ".pdb"));
                    domain.UnloadIfNoInstances(instances);
#endif
                    if (instances.Any())
                    {
#if NET5_0_OR_GREATER
                        this.Contexts.Add(context);
#else
                        this.Domains.Add(domain);
#endif
                    }
                }
            }

            if (saveToZip && File.Exists(zippath))
            {
                Dictionary<string, int> filesInZip = new();
                using (var zipFile = ZipFile.OpenRead(zippath))
                {
                    foreach (var entry in zipFile.Entries)
                    {
                        filesInZip.Add(entry.FullName, zipFile.Entries.IndexOf(entry));
                    }
                }

                foreach (var entry in filesInZip.Keys)
                {
                    // just lookup the dlls here. The LoadFromZip method will load the pdb’s if they are deemed needed.
                    if (entry.EndsWith(".dll", StringComparison.Ordinal))
                    {
#if NET5_0_OR_GREATER
                        PluginLoadContext context = new($"ZipPlugin#{filesInZip[entry]}", path);
                        var instances = RuntimeExtensions.CreateInstancesFromInterface<T>(ZipAssembly.LoadFromZip(zippath, entry, context));
                        context.UnloadIfNoInstances(instances);
#else
                        var domain = AppDomain.CreateDomain($"ZipPlugin#{filesInZip[entry]}");
                        var instances = RuntimeExtensions.CreateInstancesFromInterface<T>(ZipAssembly.LoadFromZip(zippath, entry, domain));
                        domain.UnloadIfNoInstances(instances);
#endif
                        if (instances.Any())
                        {
#if NET5_0_OR_GREATER
                            this.Contexts.Add(context);
#else
                            this.Domains.Add(domain);
#endif
                        }
                    }
                }
            }
        }

        return plugins;
    }

    /// <summary>
    /// Unloads all of the loaded plugins.
    /// </summary>
    public void UnloadPlugins()
    {
#if NET5_0_OR_GREATER
        foreach (var context in this.Contexts)
        {
            if (context.IsCollectible)
            {
                context.Unload();
            }
        }

        this.Contexts.Clear();
#else
        foreach (var domain in this.Domains)
        {
            AppDomain.Unload(domain);
        }

        this.Domains.Clear();
#endif
    }

    internal static void InvokeLoaderMessage(MessageEventArgs args)
        => PluginLoaderMessage?.Invoke(null, args);

#if !NET5_0_OR_GREATER
    private Assembly? Domain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        var isLoadedToDefaultDomain = new Func<string, bool>(name =>
        {
            var found = false;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.Equals(name, StringComparison.Ordinal))
                {
                    found = true;
                }
            }

            return AppDomain.CurrentDomain.IsDefaultAppDomain() && found;
        });
        var getFromDefaultDomain = new Func<string, Assembly?>(name =>
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.Equals(name, StringComparison.Ordinal))
                {
                    return assembly;
                }
            }

            return null;
        });
        var getDomainFromAssembly = new Func<Assembly, AppDomain>(assembly =>
        {
            if (assembly is not null)
            {
                foreach (var appDomain in this.Domains)
                {
                    if (appDomain.GetAssemblies().ToList().Contains(assembly))
                    {
                        return appDomain;
                    }
                }
            }

            return AppDomain.CurrentDomain;
        });
        if (isLoadedToDefaultDomain(args.Name))
        {
            return getFromDefaultDomain(args.Name);
        }

        AssemblyName assemblyName = new(args.Name);
        var domain = getDomainFromAssembly(args.RequestingAssembly);
        var loaddir = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}{assemblyName.Name}.dll";
        return !File.Exists(loaddir) switch
        {
            true => null,
            false => domain.Load(AssemblyName.GetAssemblyName(loaddir)),
        };
    }
#endif
}
