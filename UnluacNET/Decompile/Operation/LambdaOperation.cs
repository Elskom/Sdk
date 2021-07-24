// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public class LambdaOperation : Operation
    {
        private readonly Func<Registers, Block, Statement> m_func;

        public LambdaOperation(int line, Func<Registers, Block, Statement> func)
            : base(line)
            => this.m_func = func;

        public override Statement Process(Registers r, Block block)
            => this.m_func.Invoke(r, block);
    }
}
