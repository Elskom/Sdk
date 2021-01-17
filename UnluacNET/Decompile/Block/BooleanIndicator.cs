// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public class BooleanIndicator : Block
    {
        public override void AddStatement(Statement statement)
        {
            // Do nothing
            return;
        }

        public override bool Breakable => false;

        public override bool IsContainer => false;

        public override bool IsUnprotected => false;

        public override int GetLoopback()
            => throw new InvalidOperationException();

        public override void Print(Output output)
            => output.Print("-- unhandled boolean indicator");

        public BooleanIndicator(LFunction function, int line)
            : base(function, line, line)
        {
        }
    }
}
