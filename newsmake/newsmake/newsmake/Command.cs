// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: GPL, see LICENSE for more details.

namespace Newsmake
{
    using System;
    using System.Collections.Generic;
    using newsmake.Properties;

    internal class Command
    {
        private readonly Action<string[]> commandCode;

        internal Command(string commandSwitch, string commandDescr, Action<string[]> commandCode)
        {
            this.CommandSwitch = commandSwitch;
            this.CommandDescription = commandDescr;
            this.commandCode = commandCode;
        }

        internal Group Group { get; set; }

        internal List<Option> Options { get; private set; }

        internal string CommandDescription { get; private set; }

        internal string CommandSwitch { get; private set; }

        public static Command operator +(Command command, Option opt)
        {
            command.AddOption(opt);
            return command;
        }

        internal bool Equals(string value)
            => this.CommandSwitch?.Equals(value, StringComparison.Ordinal) ?? false;

        internal void InvokeCommand(string[] commands)
        {
            if (this.commandCode != null)
            {
                this.commandCode.Invoke(commands);
            }
            else
            {
                throw new InvalidOperationException(Resources.InvokeOption_Calling_InvokeCommand_on_a_object_with_no_code);
            }
        }

        private void AddOption(Option opt)
        {
            if (!opt.Equals(Option.NullOption) && !opt.Equals(Option.NullOption2))
            {
                opt.Group = this.Group;
                this.Options.Add(opt);
            }
        }
    }
}
