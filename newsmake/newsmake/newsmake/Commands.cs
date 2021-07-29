// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Newsmake
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Help;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Elskom.Generic.Libs;
    using newsmakeResources = Elskom.Generic.Libs.Properties;

    internal class Commands
    {
        protected Commands()
        {
        }

        public static int GlobalCommandHandler(bool help, bool version, InvocationContext ctx, IConsole console)
        {
            var inst = GitInformation.GetAssemblyInstance(typeof(Program));
            if (!inst.IsMain || inst.IsDirty)
            {
                console.Out.WriteLine(newsmakeResources.Resources.Commands_Potentially_Unstable_Build);
            }

            if (version)
            {
                console.Out.WriteLine(string.Format(CultureInfo.InvariantCulture, newsmakeResources.Resources.CommandParser_ShowHelp_Version, Assembly.GetEntryAssembly()?.GetName().Version));
            }

            if (help)
            {
                var helpBuilder = new HelpBuilder(console);
                helpBuilder.Write(ctx.ParseResult.CommandResult.Command);
            }

            return 0;
        }

        public static int BuildCommandHandler(IConsole console)
        {
            var inst = GitInformation.GetAssemblyInstance(typeof(Program));
            if (!inst.IsMain || inst.IsDirty)
            {
                console.Out.WriteLine(newsmakeResources.Resources.Commands_Potentially_Unstable_Build);
            }

            try
            {
                var project_name = string.Empty;
                var outputfile_name = string.Empty;
                var tabs = true;
                var devmode = false;
                var delete_files = true;
                var first_import = true;
                var output_format_md = false;
                var section_data = new List<string>();
                var p = Directory.EnumerateFiles(
                    Directory.GetCurrentDirectory(),
                    "*.master",
                    SearchOption.AllDirectories).First();
                if (string.IsNullOrEmpty(p))
                {
                    console.Error.WriteLine(newsmakeResources.Resources.Commands_BuildCommand_Fatal__no___master_file_found);
                    return 1;
                }

                console.Out.WriteLine(string.Format(CultureInfo.InvariantCulture, newsmakeResources.Resources.Commands_BuildCommand_Processing, p));

                // set the current directory to p.
                Directory.SetCurrentDirectory(new FileInfo(p).Directory?.FullName!);
                var master_file = File.ReadAllLines(p);
                for (var i = 0; i < master_file.Length; i++)
                {
                    // a hack to make this all work as intended.
                    var line = master_file[i];
                    if (!line.Contains("# ", StringComparison.Ordinal))
                    {
                        do
                        {
                            // get environment variable name.
                            // find the "$(" and make a substring then find the first ")"
                            var env_open = line.IndexOf("$(", StringComparison.Ordinal);
                            var env_close = line.IndexOf(")", StringComparison.Ordinal);
                            if (env_open != env_close)
                            {
                                var envvar = line[(env_open + 2)..env_close];

                                // a hack to resolve the current working directory manually...
                                var envvalue = envvar.Equals("CD", StringComparison.OrdinalIgnoreCase)
                                    ? $"{Directory.GetCurrentDirectory().Replace("\\", "/", StringComparison.Ordinal)}/"
                                    : Environment.GetEnvironmentVariable(envvar);
                                if (envvalue != null)
                                {
                                    line = line.Replace(line[env_open..(env_close + 1)], envvalue, StringComparison.Ordinal);
                                }
                                else
                                {
                                    console.Error.WriteLine(newsmakeResources.Resources.Commands_BuildCommand_Fatal__Environment_variable_does_not_exist);
                                    return 1;
                                }
                            }
                        }
                        while (line.Contains("$(", StringComparison.Ordinal));

                        if (line.Contains("projname = \"", StringComparison.Ordinal))
                        {
                            project_name = line;
                            project_name = project_name.Replace(project_name.Substring(0, 12), string.Empty, StringComparison.Ordinal);
                            project_name = project_name.Replace(project_name.Substring(project_name.Length - 1, 1), string.Empty, StringComparison.Ordinal);
                        }
                        else if (!line.Contains("projname = \"", StringComparison.Ordinal) && string.IsNullOrEmpty(project_name))
                        {
                            console.Error.WriteLine(newsmakeResources.Resources.Commands_BuildCommand_Fatal__Project_name_not_set);
                            return 1;
                        }
                        else if (line.Contains("devmode = ", StringComparison.Ordinal))
                        {
                            devmode = line.Equals("devmode = true", StringComparison.Ordinal);
                        }
                        else if (line.Contains("genfilename = \"", StringComparison.Ordinal))
                        {
                            outputfile_name = line;
                            outputfile_name = outputfile_name.Replace(outputfile_name.Substring(0, 15), string.Empty, StringComparison.Ordinal);
                            outputfile_name = outputfile_name.Replace(outputfile_name.Substring(outputfile_name.Length - 1, 1), string.Empty, StringComparison.Ordinal);
                        }
                        else if (!line.Contains("genfilename = \"", StringComparison.Ordinal) && string.IsNullOrEmpty(outputfile_name))
                        {
                            console.Error.WriteLine(newsmakeResources.Resources.Commands_BuildCommand_Fatal__generated_output_file_name_not_set);
                            return 1;
                        }
                        else if (line.Contains("tabs = ", StringComparison.Ordinal))
                        {
                            tabs = line.Equals("tabs = true", StringComparison.Ordinal);
                        }
                        else if (line.Contains("deletechunkentryfiles = ", StringComparison.Ordinal))
                        {
                            delete_files = !devmode && line.Equals("deletechunkentryfiles = true", StringComparison.Ordinal);
                        }
                        else if (line.Contains("outputasmd = ", StringComparison.Ordinal))
                        {
                            output_format_md = line.Equals("outputasmd = true", StringComparison.Ordinal);
                        }
                        else if (line.Contains("import \"", StringComparison.Ordinal))
                        {
                            var imported_folder = line;
                            imported_folder = imported_folder.Replace(imported_folder.Substring(0, 8), string.Empty, StringComparison.Ordinal);
                            imported_folder = imported_folder.Replace(imported_folder.Substring(imported_folder.Length - 1, 1), string.Empty, StringComparison.Ordinal);
                            var section_string = new StringBuilder();
                            if (first_import)
                            {
                                _ = section_string.Append(output_format_md ? "Whats new in v" : "                          Whats new in v");
                                _ = section_string.Append(imported_folder);
                                _ = section_string.Append("\n==============================================================================\n");
                                first_import = false;
                            }
                            else
                            {
                                if (!output_format_md)
                                {
                                    // if the section divider is not here the resulting markdown
                                    // would look like trash.
                                    _ = section_string.Append(
                                        "\n==============================================================================\n");
                                    _ = section_string.Append("                             ");
                                }

                                _ = section_string.AppendFormat("{0} v{1}", project_name, imported_folder);
                                _ = section_string.Append("\n==============================================================================\n");
                            }

                            var section_text = new StringBuilder();
                            if (Directory.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{imported_folder}"))
                            {
                                foreach (var imported_path in
                                    Directory.EnumerateFiles($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{imported_folder}", "*", SearchOption.AllDirectories))
                                {
                                    var temp = new StringBuilder();
                                    _ = temp.Append((!output_format_md, tabs) switch
                                    {
                                        (true, true) => "\t+ ",
                                        (true, false) => "    + ",
                                        _ => "+ ",
                                    });
                                    var entry_lines = File.ReadAllLines(imported_path);
                                    foreach (var entry_line in entry_lines)
                                    {
                                        temp.Append(entry_line);
                                    }

                                    var temp2 = temp.ToString();
                                    Formatline(ref temp2, tabs, output_format_md);
                                    _ = temp.Clear();
                                    _ = temp.Append(temp2);
                                    _ = temp.Append('\n');
                                    section_text.Append(temp);
                                }
                            }

                            if (delete_files && !string.IsNullOrEmpty(section_text.ToString()))
                            {
                                // save section text and then delete the folder.
                                File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{imported_folder}.section", section_text.ToString());
                                foreach (var imported_path in Directory.EnumerateFiles($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{imported_folder}", "*", SearchOption.AllDirectories))
                                {
                                    File.Delete(imported_path);
                                }

                                Directory.Delete($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{imported_folder}");
                            }

                            if (section_text.Length == 0 && File.Exists($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{imported_folder}.section"))
                            {
                                // load saved *.section file.
                                section_text.Append(File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{imported_folder}.section"));
                            }

                            section_string.Append(section_text);
                            section_data.Add(section_string.ToString());
                        }
                        else if (string.IsNullOrEmpty(line))
                        {
                            // do nothing.
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(outputfile_name))
                {
                    var finfo = new FileInfo(outputfile_name);
                    if (!finfo.Directory.Exists)
                    {
                        finfo.Directory.Create();
                    }

                    var output = new StringBuilder();
                    section_data.ForEach(x => _ = output.Append(x));
                    File.WriteAllText(outputfile_name, output.ToString());
                    console.Out.WriteLine(string.Format(CultureInfo.InvariantCulture, newsmakeResources.Resources.Commands_BuildCommand_Successfully_Generated, outputfile_name));

                    // clear leaking any specific information to other *.master file configurations that should not be present.
                    project_name = string.Empty;
                    outputfile_name = string.Empty;
                    tabs = true;
                    devmode = false;
                    delete_files = true;
                    first_import = true;
                    output_format_md = false;
                    section_data.Clear();
                }
            }
            catch (Exception ex)
            {
                return MiniDumpAttribute.DumpException(ex, false);
            }

            return 0;
        }

        public static int NewReleaseCommandHandler(string release, IConsole console)
        {
            var inst = GitInformation.GetAssemblyInstance(typeof(Program));
            if (!inst.IsMain || inst.IsDirty)
            {
                console.Out.WriteLine(newsmakeResources.Resources.Commands_Potentially_Unstable_Build);
            }

            if (!string.IsNullOrEmpty(release))
            {
                var p = Directory.EnumerateFiles(
                    Directory.GetCurrentDirectory(),
                    "*.master",
                    SearchOption.AllDirectories).First();
                var path = new FileInfo(p).Directory?.FullName;
                var lines = File.ReadAllLines(p);
                var masterfile = p;
                if (string.IsNullOrEmpty(path))
                {
                    console.Error.WriteLine(newsmakeResources.Resources.Commands_BuildCommand_Fatal__no___master_file_found);
                    return 1;
                }

                if (Directory.Exists($"{path}{Path.DirectorySeparatorChar}{release}"))
                {
                    console.Error.WriteLine($"Fatal: The *.master file already has a pending release.");
                    return 1;
                }

                if (!Directory.Exists($"{path}{Path.DirectorySeparatorChar}{release}"))
                {
                    _ = Directory.CreateDirectory($"{path}{Path.DirectorySeparatorChar}{release}");
                    var linecnt = 0;
                    foreach (var line in lines!)
                    {
                        linecnt++;
                        if (line.Contains("import \"", StringComparison.Ordinal) && !line.Contains($"import \"{release}\"", StringComparison.Ordinal))
                        {
                            break;
                        }

                        if (line.Contains($"import \"{release}\"", StringComparison.Ordinal))
                        {
                            console.Error.WriteLine($"Fatal: The import of the release '{release}' already exists in the *.master file.");
                            return 1;
                        }
                    }

                    var linelst = lines.ToList();
                    linelst.Insert(linecnt - 1, $"import \"{release}\"");
                    File.WriteAllLines(masterfile, linelst.ToArray());
                }
                else
                {
                    console.Error.WriteLine($"Fatal: The release '{release}' already exists.");
                    return 1;
                }
            }
            else
            {
                console.Error.WriteLine("Fatal: no release name specified.");
                return 1;
            }

            return 0;
        }

        public static int NewEntryCommandHandler(string content, IConsole console)
        {
            var inst = GitInformation.GetAssemblyInstance(typeof(Program));
            if (!inst.IsMain || inst.IsDirty)
            {
                console.Out.WriteLine(newsmakeResources.Resources.Commands_Potentially_Unstable_Build);
            }

            try
            {
                var path = new FileInfo(
                    Directory.EnumerateFiles(
                        Directory.GetCurrentDirectory(),
                        "*.master",
                        SearchOption.AllDirectories).First()).Directory?.FullName;
                if (string.IsNullOrEmpty(path))
                {
                    console.Error.WriteLine(newsmakeResources.Resources.Commands_BuildCommand_Fatal__no___master_file_found);
                    return 1;
                }

                foreach (var p in Directory.EnumerateDirectories(path))
                {
                    var currentFile = "item000";
                    var files = Directory.EnumerateFiles(p).ToArray();
                    foreach (var file in files)
                    {
                        currentFile = file;
                    }

                    var fileCountStr = files.Length switch
                    {
                        < 10 and > -1 => $"00{files.Length}",
                        > 99 and < 1000 => $"{files.Length}",
                        > 9 => $"0{files.Length}",
                        _ => throw new InvalidOperationException(),
                    };
                    var newfileCount = files.Length + 1;
                    var newfileCountStr = newfileCount switch
                    {
                        < 10 and > -1 => $"00{newfileCount}",
                        > 99 and < 1000 => $"{newfileCount}",
                        > 9 => $"0{newfileCount}",
                        _ => throw new InvalidOperationException(),
                    };
                    var newFile = $"{currentFile.Replace($"item{fileCountStr}", $"item{newfileCountStr}", StringComparison.Ordinal)}";
                    File.WriteAllText(newFile, content);
                }
            }
            catch (Exception ex)
            {
                return MiniDumpAttribute.DumpException(ex, false);
            }

            return 0;
        }

        public static int FinalizeReleaseCommandHandler(IConsole console)
        {
            var inst = GitInformation.GetAssemblyInstance(typeof(Program));
            if (!inst.IsMain || inst.IsDirty)
            {
                console.Out.WriteLine(newsmakeResources.Resources.Commands_Potentially_Unstable_Build);
            }

            try
            {
                var tabs = true;
                var output_format_md = false;
                var p = Directory.EnumerateFiles(
                    Directory.GetCurrentDirectory(),
                    "*.master",
                    SearchOption.AllDirectories).First();
                if (string.IsNullOrEmpty(p))
                {
                    console.Error.WriteLine(newsmakeResources.Resources.Commands_BuildCommand_Fatal__no___master_file_found);
                    return 1;
                }

                var fi = new FileInfo(p);
                var master_file = File.ReadAllLines(p);
                for (var i = 0; i < master_file.Length; i++)
                {
                    // a hack to make this all work as intended.
                    var line = master_file[i];
                    if (!line.Contains("# ", StringComparison.Ordinal))
                    {
                        if (line.Contains("tabs = ", StringComparison.Ordinal))
                        {
                            tabs = line.Equals("tabs = true", StringComparison.Ordinal);
                        }
                        else if (line.Contains("outputasmd = ", StringComparison.Ordinal))
                        {
                            output_format_md = line.Equals("outputasmd = true", StringComparison.Ordinal);
                        }
                        else if (line.Contains("import \"", StringComparison.Ordinal))
                        {
                            var imported_folder = line;
                            imported_folder = imported_folder.Replace(imported_folder.Substring(0, 8), string.Empty, StringComparison.Ordinal);
                            imported_folder = imported_folder.Replace(imported_folder.Substring(imported_folder.Length - 1, 1), string.Empty, StringComparison.Ordinal);
                            var section_text = new StringBuilder();
                            if (Directory.Exists($"{fi.Directory?.FullName}{Path.DirectorySeparatorChar}{imported_folder}"))
                            {
                                foreach (var imported_path in
                                    Directory.EnumerateFiles($"{fi.Directory?.FullName}{Path.DirectorySeparatorChar}{imported_folder}", "*", SearchOption.AllDirectories))
                                {
                                    var temp = new StringBuilder();
                                    _ = temp.Append((!output_format_md, tabs) switch
                                    {
                                        (true, true) => "\t+ ",
                                        (true, false) => "    + ",
                                        _ => "+ ",
                                    });
                                    var entry_lines = File.ReadAllLines(imported_path);
                                    foreach (var entry_line in entry_lines)
                                    {
                                        _ = temp.Append(entry_line);
                                    }

                                    var temp2 = temp.ToString();
                                    Formatline(ref temp2, tabs, output_format_md);
                                    _ = temp.Clear();
                                    _ = temp.Append(temp2);
                                    _ = temp.Append('\n');
                                    section_text.Append(temp);
                                }
                            }

                            if (!string.IsNullOrEmpty(section_text.ToString()))
                            {
                                // save section text and then delete the folder.
                                File.WriteAllText($"{fi.Directory?.FullName}{Path.DirectorySeparatorChar}{imported_folder}.section", section_text.ToString());
                                foreach (var imported_path in Directory.EnumerateFiles($"{fi.Directory?.FullName}{Path.DirectorySeparatorChar}{imported_folder}", "*", SearchOption.AllDirectories))
                                {
                                    File.Delete(imported_path);
                                }

                                Directory.Delete($"{fi.Directory?.FullName}{Path.DirectorySeparatorChar}{imported_folder}");
                            }
                        }
                        else if (string.IsNullOrEmpty(line))
                        {
                            // do nothing.
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return MiniDumpAttribute.DumpException(ex, false);
            }

            return 0;
        }

        private static void Formatline(ref string input, bool tabs, bool output_format_md, int line_length = 80)
        {
            if (input.Length < line_length)
            {
                return;
            }

            var indent = (!output_format_md, tabs) switch
            {
                (true, true) => "\t\t",
                (true, false) => "        ",
                (false, true) => "\t",
                (false, false) => "    ",
            };
            var tab_length = 8;
            var indent_line_length = line_length;
            var pos = 0;
            var last_pos = 0;
            var output = new StringBuilder();
            while (true)
            {
                if (pos > 0 && last_pos == pos)
                {
                    throw new IOException(newsmakeResources.Resources.Commands_Formatline_bug);
                }

                last_pos = pos;
                if (input[pos] == ' ' && pos > 0)
                {
                    ++pos;
                }

                if (pos >= input.Length)
                {
                    break;
                }

                var sub_s = input.Substring(pos, input.Length - pos > indent_line_length ? indent_line_length : input.Length - pos);
                var last_space = sub_s.LastIndexOf(' ');
                if (last_space is 0 || last_space is int.MaxValue || pos + indent_line_length >= input.Length)
                {
                    last_space = sub_s.Length;
                }

                if (last_space != sub_s.Length)
                {
                    sub_s = sub_s.Substring(0, last_space);
                }

                _ = output.Append(sub_s);
                pos += last_space;
                if (pos >= input.Length)
                {
                    break;
                }

                _ = output.Append($"\n{indent}");
                indent_line_length = line_length - tab_length;
            }

            input = output.ToString();
        }
    }
}
