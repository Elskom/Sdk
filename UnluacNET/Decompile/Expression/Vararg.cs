// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class Vararg : Expression
    {
        private readonly int m_length;
        private readonly bool m_multiple;

        public override int ConstantIndex => -1;

        public override bool IsMultiple => this.m_multiple;

        public override void Print(Output output)
            //output.Print("...");
            => output.Print(this.m_multiple ? "..." : "(...)");

        public override void PrintMultiple(Output output)
            => output.Print(this.m_multiple ? "..." : "(...)");

        public Vararg(int length, bool multiple)
            : base(PRECEDENCE_ATOMIC)
        {
            this.m_length = length;
            this.m_multiple = multiple;
        }
    }
}
