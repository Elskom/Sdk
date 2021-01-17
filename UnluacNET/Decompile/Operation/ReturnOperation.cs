// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class ReturnOperation : Operation
    {
        private Expression[] m_values;

        public override Statement Process(Registers r, Block block)
            => new Return(this.m_values);

        public ReturnOperation(int line, Expression value)
            : base(line)
            => this.m_values = new Expression[1]
            {
                value
            };

        public ReturnOperation(int line, Expression[] values)
            : base(line)
            => this.m_values = values;
    }
}
