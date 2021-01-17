// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    
    public abstract class Operation
    {
        public int Line { get; private set; }

        public abstract Statement Process(Registers r, Block block);

        public Operation(int line)
            => this.Line = line;
    }

    public class GenericOperation : Operation
    {
        private readonly Statement m_statement;

        public override Statement Process(Registers r, Block block)
            => this.m_statement;

        public GenericOperation(int line, Statement statement)
            : base(line)
            => this.m_statement = statement;
    }

    public class LambdaOperation : Operation
    {
        private readonly Func<Registers, Block, Statement> m_func;

        public override Statement Process(Registers r, Block block)
            => this.m_func.Invoke(r, block);

        public LambdaOperation(int line, Func<Registers, Block, Statement> func)
            : base(line)
            => this.m_func = func;
    }
}
