// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

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
        return r.IsAssignable(this.Register, this.Line) ? new Assignment(r.GetTarget(this.Register, this.Line), this.Value) : null;
    }
}
