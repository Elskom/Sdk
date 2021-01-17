// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class LocalVariable : Expression
    {
        private readonly Declaration m_decl;

        public Declaration Declaration => this.m_decl;

        public override int ConstantIndex => -1;

        public override bool IsBrief => true;

        public override bool IsDotChain => true;

        public override void Print(Output output)
            => output.Print(this.m_decl.Name);

        public LocalVariable(Declaration decl)
            : base(PRECEDENCE_ATOMIC)
            => this.m_decl = decl;
    }
}
