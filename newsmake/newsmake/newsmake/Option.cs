// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: GPL, see LICENSE for more details.

namespace Newsmake
{
    using System;
    using newsmake.Properties;

    internal class Option
    {
        private readonly Func<string[], object> optionCode;

        internal Option(string optionSwitch, string optionDescr, Func<string[], object> optionCode)
        {
            this.OptionSwitch = optionSwitch;
            this.OptionDescription = optionDescr;
            this.optionCode = optionCode;
        }

        internal static Option NullOption { get; } = new Option(string.Empty, string.Empty, null);

        internal static Option NullOption2 { get; } = new Option(string.Empty, string.Empty, null);

        internal Group Group { get; set; }

        internal string OptionDescription { get; private set; }

        internal string OptionSwitch { get; private set; }

        internal object Result { get; private set; }

        internal bool Equals(string value) => !string.IsNullOrEmpty(this.OptionSwitch) && this.OptionSwitch.Equals(value, StringComparison.Ordinal);

        internal bool Equals(Option value)
        {
            if (string.IsNullOrEmpty(this.OptionSwitch) && string.IsNullOrEmpty(this.OptionDescription) && this.optionCode == null)
            {
                if (this.Equals(value.OptionSwitch) && this.OptionDescription.Equals(value.OptionDescription, StringComparison.Ordinal) && this.optionCode == value.optionCode)
                {
                    return true;
                }
            }

            return false;
        }

        internal void InvokeOption(string[] commands)
        {
            // make sure this does not throw for null options.
            if (!this.Equals(NullOption) && !this.Equals(NullOption2))
            {
                if (this.optionCode != null)
                {
                    this.Result = this.optionCode.Invoke(commands);
                }
                else
                {
                    throw new InvalidOperationException(Resources.InvokeOption_Calling_InvokeCommand_on_a_object_with_no_code);
                }
            }
        }
    }
}
