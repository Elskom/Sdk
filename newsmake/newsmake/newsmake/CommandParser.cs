// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: GPL, see LICENSE for more details.

namespace Newsmake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using newsmake.Properties;

    internal class CommandParser
    {
        private readonly string[] args;

        public CommandParser(string[] args)
        {
            this.args = args;
            this.Groups = new List<Group>();
        }

        public int Length
            => this.args.Length;

        public bool HasDocs { get; set; }

        public string DocsUrl { get; set; } = string.Empty;

        private List<Group> Groups { get; set; }

        /*
        [Obsolete("Do not use this version of operator +; use the Dictionary<string, Command> one as it allows you to save code.", true)]
        public static CommandParser operator +(CommandParser parser, Group group)
        {
            // if "Global" command group does not exist.
            if (parser.GetGroupIndex("Global") == -1)
            {
                parser.AddGroup(new Group("Global"));
            }

            parser.AddGroup(group);
            return parser;
        }

        [Obsolete("Do not use this version of operator +; use the Dictionary<string, Command> one as it allows you to save code.", true)]
        public static CommandParser operator +(CommandParser parser, Group[] groups)
        {
            // if "Global" command group does not exist.
            if (parser.GetGroupIndex("Global") == -1)
            {
                parser.AddGroup(new Group("Global"));
            }

            parser.AddGroups(groups);
            return parser;
        }
        */

        /// <summary>
        /// Adds options to the parser under a group.
        /// </summary>
        public static CommandParser operator +(CommandParser parser, Dictionary<string, Option> opts)
        {
            // if "Global" command group does not exist.
            if (parser.GetGroupIndex("Global") == -1)
            {
                parser.AddGroup(new Group("Global"));
            }

            foreach (var option in opts)
            {
                if (parser.GetGroup(option.Key) == null)
                {
                    parser.AddGroup(new Group(option.Key));
                }

                var group = parser.GetGroup(option.Key);
                group += option.Value;
                parser.Replace(new Dictionary<string, Group> { { group.GroupName, group }, });
            }

            return parser;
        }

        /// <summary>
        /// Adds commands to the parser under a group.
        /// </summary>
        public static CommandParser operator +(CommandParser parser, Dictionary<string, Command> cmds)
        {
            // if "Global" command group does not exist.
            if (parser.GetGroupIndex("Global") == -1)
            {
                parser.AddGroup(new Group("Global"));
            }

            foreach (var command in cmds)
            {
                if (parser.GetGroup(command.Key) == null)
                {
                    parser.AddGroup(new Group(command.Key));
                }

                var group = parser.GetGroup(command.Key);
                group += command.Value;
                parser.Replace(new Dictionary<string, Group> { { group.GroupName, group }, });
            }

            return parser;
        }

        /// <summary>
        /// Adds commands to the parser under a group with options in the group.
        /// </summary>
        public static CommandParser operator +(CommandParser parser, Dictionary<string, Dictionary<Option, Command>> cmds)
        {
            // if "Global" command group does not exist.
            if (parser.GetGroupIndex("Global") == -1)
            {
                parser.AddGroup(new Group("Global"));
            }

            foreach (var option in cmds)
            {
                if (parser.GetGroup(option.Key) == null)
                {
                    parser.AddGroup(new Group(option.Key));
                }

                var group = parser.GetGroup(option.Key);
                foreach (var command in option.Value)
                {
                    // do not add option if option is null.
                    if (command.Key != null)
                    {
                        group += command.Key;
                    }

                    // do not add command if command is null.
                    if (command.Value != null)
                    {
                        group += command.Value;
                    }
                }

                parser.Replace(new Dictionary<string, Group> { { group.GroupName, group }, });
            }

            return parser;
        }

        /// <summary>
        /// Adds commands to the parser under a group with options in the group and in the commands.
        /// </summary>
        public static CommandParser operator +(CommandParser parser, Dictionary<string, Dictionary<Option, Dictionary<Command, Option>>> cmds)
        {
            // if "Global" command group does not exist.
            if (parser.GetGroupIndex("Global") == -1)
            {
                parser.AddGroup(new Group("Global"));
            }

            foreach (var option in cmds)
            {
                if (parser.GetGroup(option.Key) == null)
                {
                    parser.AddGroup(new Group(option.Key));
                }

                var group = parser.GetGroup(option.Key);
                foreach (var command in option.Value)
                {
                    // do not add option if option is null.
                    if (command.Key != null)
                    {
                        group += command.Key;
                    }

                    // do not add command if command is null.
                    if (command.Value != null)
                    {
                        foreach (var commandOption in command.Value)
                        {
                            group += commandOption.Key;
                            var cmd = group.FindCommand(commandOption.Key.CommandSwitch);
                            cmd += commandOption.Value;
                        }
                    }
                }

                parser.Replace(new Dictionary<string, Group> { { group.GroupName, group }, });
            }

            return parser;
        }

        public void ProcessCommands()
        {
            if (this.Length == 0)
            {
                this.ShowHelp();
            }
            else
            {
                var cmdgroup = string.Empty;
                var foundcmd = false;
                var foundgrp = false;
                var currentArg = string.Empty;
                foreach (var arg in this.args)
                {
                    currentArg = arg;
                    foreach (var group in this.Groups)
                    {
                        // if (cmd.Group == null && cmd.Group.GroupName.Equals(arg))
                        // {
                        //     foundgrp = true;
                        //     cmdgroup = cmd.Group.GroupName;
                        // }
                        /*else */
                        if (group.GroupName.Equals(currentArg, StringComparison.Ordinal))
                        {
                            foundgrp = true;
                            cmdgroup = group.GroupName;
                        }
                        else if (/*cmd.Group != null && */group.CommandEquals(arg) && (group.GroupName.Equals(cmdgroup, StringComparison.Ordinal) || group.GroupName.Equals("Global", StringComparison.Ordinal)))
                        {
                            foundcmd = true;

                            // now we got to filter the commands, stripping the ones already processed for passing to the invoked command.
                            var tmp = this.args.ToList();
                            var index = tmp.IndexOf(arg);
                            tmp.RemoveRange(0, index + 1);
                            group.FindCommand(arg).InvokeCommand(tmp.ToArray());
                            tmp.Clear();
                        }
                        else if (group.OptionEquals(arg))
                        {
                            foundcmd = true;

                            // now we got to filter the commands, stripping the ones already processed for passing to the invoked command.
                            var tmp = this.args.ToList();
                            var index = tmp.IndexOf(arg);
                            tmp.RemoveRange(0, index + 1);
                            group.FindOption(arg).InvokeOption(tmp.ToArray());
                            tmp.Clear();
                        }
                    }
                }

                if (!foundcmd && !foundgrp)
                {
                    Console.Error.WriteLine($"Error: invalid command-line option '{currentArg}'.");
                }
            }
        }

        private void ShowHelp()
        {
            // thanks dotnet-cli for the idea of the help contents.
            Console.WriteLine(Resources.CommandParser_ShowHelp_Version, Assembly.GetEntryAssembly().GetName().Version);
            Console.WriteLine(Resources.CommandParser_ShowHelp_usage, Assembly.GetEntryAssembly().GetName().Name);
            Console.WriteLine();
            Console.WriteLine(Resources.CommandParser_ShowHelp_Available_commands);
            Console.WriteLine();
            foreach (var group in this.Groups)
            {
                foreach (var cmd in group.Commands)
                {
                    Console.WriteLine(Resources.CommandParser_ShowHelp, group.GroupName.Equals("Global", StringComparison.Ordinal) ? string.Empty : group.GroupName + " ", cmd.CommandSwitch, cmd.CommandDescription);
                    Console.WriteLine();
                }
            }

            if (this.HasDocs)
            {
                Console.WriteLine(Resources.CommandParser_ShowHelp_For_more_information__visit, this.DocsUrl);
            }
        }

        private Group GetGroup(string groupName)
        {
            foreach (var group in this.Groups)
            {
                if (group.GroupName.Equals(groupName, StringComparison.Ordinal))
                {
                    return group;
                }
            }

            return null;
        }

        private int GetGroupIndex(string groupName)
        {
            foreach (var group in this.Groups)
            {
                if (group.GroupName.Equals(groupName, StringComparison.Ordinal))
                {
                    return this.Groups.IndexOf(group);
                }
            }

            return -1; // to be consistent with IndexOf()!!!
        }

        private void AddGroup(Group group)
            => this.Groups.Add(group);

        /*
        [Obsolete("Not needed anymore.", true)]
        private void AddGroups(Group[] groups)
            => this.Groups.AddRange(groups);
        */

        private void Replace(Dictionary<string, Group> groups)
        {
            foreach (var item in groups)
            {
                this.Groups[this.GetGroupIndex(item.Key)] = item.Value;
            }
        }
    }
}
