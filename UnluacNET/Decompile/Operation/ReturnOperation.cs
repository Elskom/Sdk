// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class ReturnOperation : Operation
{
    private readonly Expression[] m_values;

    public ReturnOperation(int line, Expression value)
        : base(line)
        => this.m_values = new[]
        {
            value,
        };

    public ReturnOperation(int line, Expression[] values)
        : base(line)
        => this.m_values = values;

    public override Statement Process(Registers r, Block block)
        => new Return(this.m_values);
}
