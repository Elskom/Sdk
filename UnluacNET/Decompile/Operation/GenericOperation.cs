// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class GenericOperation : Operation
{
    private readonly Statement m_statement;

    public GenericOperation(int line, Statement statement)
        : base(line)
        => this.m_statement = statement;

    public override Statement Process(Registers r, Block block)
        => this.m_statement;
}
