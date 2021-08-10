// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class Break : Block
{
    public Break(LFunction function, int line, int target)
        : base(function, line, line)
        => this.Target = target;

    public int Target { get; private set; }

    public override bool Breakable => false;

    public override bool IsContainer => false;

    // Actually, it is unprotected, but not really a block
    public override bool IsUnprotected => false;

    public override void AddStatement(Statement statement)
        => throw new InvalidOperationException();

    public override int GetLoopback()
        => throw new InvalidOperationException();

    public override void Print(Output output)
        => output.Print("do return end");

    public override void PrintTail(Output output)
        => output.Print("break");
}
