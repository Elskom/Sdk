// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

using System;

public class AssignNode : Branch
{
    private Expression m_expression;

    public AssignNode(int line, int begin, int end)
        : base(line, begin, end)
    {
    }

    public override Expression AsExpression(Registers registers)
        => this.m_expression;

    public override int GetRegister()
        => throw new InvalidOperationException();

    public override Branch Invert()
        => throw new InvalidOperationException();

    public override void UseExpression(Expression expression)
        => this.m_expression = expression;
}
