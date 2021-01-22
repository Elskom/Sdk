// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class RegisterSet : Operation
    {
        public RegisterSet(int line, int register, Expression value)
            : base(line)
        {
            this.Register = register;
            this.Value = value;
        }

        public int Register { get; private set; }

        public Expression Value { get; private set; }

        public override Statement Process(Registers r, Block block)
        {
            r.SetValue(this.Register, this.Line, this.Value);
            if (r.IsAssignable(this.Register, this.Line))
            {
                return new Assignment(r.GetTarget(this.Register, this.Line), this.Value);
            }
            else
            {
                return null;
            }
        }
    }
}
