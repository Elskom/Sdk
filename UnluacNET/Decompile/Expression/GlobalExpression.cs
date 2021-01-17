// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class GlobalExpression : Expression
    {
        private readonly string m_name;
        private readonly int m_index;

        public override int ConstantIndex => this.m_index;

        public override bool IsBrief => true;

        public override bool IsDotChain => true;

        public override void Print(Output output)
            => output.Print(this.m_name);

        public GlobalExpression(string name, int index)
            : base(PRECEDENCE_ATOMIC)
        {
            this.m_name = name;
            this.m_index = index;
        }
    }
}
