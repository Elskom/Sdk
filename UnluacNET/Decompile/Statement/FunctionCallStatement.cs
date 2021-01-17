// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class FunctionCallStatement : Statement
    {
        private FunctionCall m_call;

        public override bool BeginsWithParen => this.m_call.BeginsWithParen;

        public override void Print(Output output)
            => this.m_call.Print(output);

        public FunctionCallStatement(FunctionCall call)
            => this.m_call = call;
    }
}
