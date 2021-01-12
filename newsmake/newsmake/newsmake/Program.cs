// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: GPL, see LICENSE for more details.

namespace Newsmake
{
    using System;
    using System.Collections.Generic;
    using System.Messaging;
    using System.Reflection;
    using Elskom.Generic.Libs;

    internal static class Program
    {
        [MiniDump(Text = "Please send a copy of {0} to https://github.com/Elskom/newsmake/issues by making an issue and attaching the log(s) and mini-dump(s).", DumpType = MinidumpTypes.ValidTypeFlags)]
        internal static int Main(string[] args)
        {
            // MiniDump.DumpFailed += MiniDump_DumpFailed;
            MiniDump.DumpMessage += MiniDump_DumpMessage;
            _ = Assembly.GetEntryAssembly().EntryPoint.GetCustomAttributes(false);
            var commandParser = new CommandParser(args);
            Func<string[], object> func = (commands) =>
            {
                Commands.VersionCommand();
                return null;
            };
            commandParser += new Dictionary<string, Dictionary<Option, Command>>
            {
                { "Global", new Dictionary<Option, Command> { { new Option("--version", "Shows the version of this command-line program.", func), null }, { Option.NullOption, new Command("build", "builds a changelog or news file from any *.master file in the current or sub directory.", (string[] commands) => Commands.BuildCommand()) }, } },
                { "new", new Dictionary<Option, Command> { { Option.NullOption, new Command("release", "Creates a new pending release folder and imports it in the *.master file in the current or sub directory.", (string[] release) => Commands.NewReleaseCommand(release)) }, { Option.NullOption2, new Command("entry", "Creates a new entry file for the current pending release.", (string[] commands) => Commands.NewEntryCommand()) }, } },
                { "finalize", new Dictionary<Option, Command> { { Option.NullOption, new Command("release", "Finalizes a pending release to a section file.", (string[] commands) => Commands.FinalizeReleaseCommand()) }, } },
            };
            commandParser.ProcessCommands();
            return 0;
        }

        private static void MiniDump_DumpMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine($"{e.Caption}: {e.Text}");
            if (!e.Text.StartsWith("Mini-dumping failed with Code: ", StringComparison.Ordinal))
            {
                Environment.Exit(1);
            }
        }
    }
}
