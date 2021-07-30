// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

#if NET5_0_OR_GREATER
namespace Elskom.Generic.Libs;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

/// <inheritdoc/>
public class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver resolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginLoadContext"/> class.
    /// </summary>
    /// <param name="name">The name of the load context.</param>
    /// <param name="pluginPath">The path the the plugins.</param>
    public PluginLoadContext(string name, string pluginPath)
        : base(name, true)
        => this.resolver = new(pluginPath);

    /// <inheritdoc/>
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var isLoadedToDefaultContext = new Func<string, bool>(static name =>
        {
            return Default.Assemblies.Any(assembly => assembly.FullName is not null && assembly.FullName.Equals(name, StringComparison.Ordinal));
        });
        var getFromDefaultContext = new Func<string, Assembly?>(static name =>
        {
            return Default.Assemblies.FirstOrDefault(assembly => assembly.FullName is not null && assembly.FullName.Equals(name, StringComparison.Ordinal));
        });
        if (isLoadedToDefaultContext(assemblyName.FullName))
        {
            // return the assembly from the default context instead of reloading it (is same assembly and version).
            return getFromDefaultContext(assemblyName.FullName);
        }

        var assemblyPath = this.resolver.ResolveAssemblyToPath(assemblyName);
        return (assemblyPath is not null, !File.Exists($"{AppContext.BaseDirectory}{assemblyName.Name}.dll")) switch
        {
            (false, true) => null,
            (false, false) => this.LoadFromAssemblyPath($"{AppContext.BaseDirectory}{assemblyName.Name}.dll"),
            _ => this.LoadFromAssemblyPath(assemblyPath!),
        };
    }

    /// <inheritdoc/>
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = this.resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return (libraryPath is not null, !File.Exists($"{AppContext.BaseDirectory}{unmanagedDllName}.dll")) switch
        {
            (false, true) => IntPtr.Zero,
            (false, false) => this.LoadUnmanagedDllFromPath($"{AppContext.BaseDirectory}{unmanagedDllName}.dll"),
            _ => this.LoadUnmanagedDllFromPath(libraryPath!),
        };
    }
}
#endif
