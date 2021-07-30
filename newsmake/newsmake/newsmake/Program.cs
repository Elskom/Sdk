// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

global using newsmakeResources = Elskom.Generic.Libs.Properties;

namespace Newsmake;

internal static class Program
{
    [MiniDump(Text = "Please send a copy of {0} to https://github.com/Elskom/Sdk/issues by making an issue and attaching the log(s) and mini-dump(s).", DumpType = DumpType.Full)]
    internal static async Task<int> Main(string[] args)
    {
        MiniDumpAttribute.DumpMessage += MiniDump_DumpMessage;
        _ = Assembly.GetEntryAssembly()?.EntryPoint!.GetCustomAttributes<MiniDumpAttribute>(false);
        GitInformation.ApplyAssemblyAttributes(typeof(Program).Assembly);
        var cmd = new RootCommand
        {
            new Option(new[] { "--help", "help" }, "Shows the help message for the program."),
            new Option("--version", "Shows the version of this command-line program."),
            new Command("build", "builds a changelog or news file from any *.master file in the current or sub directory.")
            {
            }.WithHandler<Commands>(nameof(Commands.BuildCommandHandler)),
            new Command("new", string.Empty)
            {
                new Command("release", "Creates a new pending release folder and imports it in the *.master file in the current or sub directory.")
                {
                    new Argument<string>("release", "The new release version."),
                }.WithHandler<Commands>(nameof(Commands.NewReleaseCommandHandler)),
                new Command("entry", "Creates a new entry file for the current pending release.")
                {
                    new Argument<string>("content", "The content to add to the new entry."),
                }.WithHandler<Commands>(nameof(Commands.NewEntryCommandHandler)),
            },
            new Command("finalize", string.Empty)
            {
                new Command("release", "Finalizes a pending release to a section file.")
                {
                }.WithHandler<Commands>(nameof(Commands.FinalizeReleaseCommandHandler)),
            },
        }.WithHandler<Commands>(nameof(Commands.GlobalCommandHandler));
        return await cmd.InvokeAsync(args);
    }

    private static Command WithHandler<T>(this Command command, string name)
    {
        var method = typeof(T).GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        if (method is null)
        {
            return command;
        }

        command.Handler = CommandHandler.Create(method!);
        return command;
    }

    private static void MiniDump_DumpMessage(object sender, MessageEventArgs e)
    {
        Console.WriteLine($@"{e.Caption}: {e.Text}");
        e.ExitCode = 1;
    }
}
