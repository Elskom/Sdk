// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public class GlobalTarget : Target
    {
        private readonly string m_name;

        public GlobalTarget(string name)
            => this.m_name = name;

        public override void Print(Output output)
            => output.Print(this.m_name);

        public override void PrintMethod(Output output)
            => throw new InvalidOperationException();
    }
}
