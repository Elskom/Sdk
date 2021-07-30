// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class CompareBlock : Block
{
    public CompareBlock(LFunction function, int begin, int end, int target, Branch branch)
        : base(function, begin, end)
    {
        this.Target = target;
        this.Branch = branch;
    }

    public int Target { get; }

    public Branch Branch { get; }

    public override bool Breakable => false;

    public override bool IsContainer => false;

    public override bool IsUnprotected => false;

    public override void AddStatement(Statement statement)
    {
        // Do nothing
    }

    public override int GetLoopback()
        => throw new InvalidOperationException();

    public override void Print(Output output)
        => output.Print("-- unhandled compare assign");

    public override Operation Process(Decompiler d)
        => new LambdaOperation(this.End - 1, (r, block) =>
        {
            return new RegisterSet(this.End - 1, this.Target, this.Branch.AsExpression(r)).Process(r, block);
        });
}
