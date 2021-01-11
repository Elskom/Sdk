// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Seems like the analyzer has a bug.", Scope = "member", Target = "~M:Elskom.Generic.Libs.PluginUpdateCheck.Dispose(System.Boolean)")]
[assembly: SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected.", Justification = "Needed to clean up WebClient.", Scope = "member", Target = "~M:Elskom.Generic.Libs.PluginUpdateCheck.Dispose(System.Boolean)")]
