// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public class CompareBlock : Block
    {
        public int Target { get; set; }
        public Branch Branch { get; set; }

        public override bool Breakable => false;

        public override bool IsContainer => false;

        public override bool IsUnprotected => false;

        public override void AddStatement(Statement statement)
        {
            // Do nothing
            return;
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

        public CompareBlock(LFunction function, int begin, int end, int target, Branch branch)
            : base(function, begin, end)
        {
            this.Target = target;
            this.Branch = branch;
        }
    }
}
