// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class CallOperation : Operation
    {
        private readonly FunctionCall m_call;

        public CallOperation(int line, FunctionCall call)
            : base(line)
            => this.m_call = call;

        public override Statement Process(Registers r, Block block)
            => new FunctionCallStatement(this.m_call);
    }
}
