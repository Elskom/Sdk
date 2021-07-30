// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

using System;
using System.Collections.Generic;

public class TForBlock : Block
{
    private readonly int m_register;
    private readonly int m_length;
    private readonly Registers m_r;
    private readonly List<Statement> m_statements;

    public TForBlock(LFunction function, int begin, int end, int register, int length, Registers r)
        : base(function, begin, end)
    {
        this.m_register = register;
        this.m_length = length;
        this.m_r = r;
        this.m_statements = new(end - begin + 1);
    }

    public override bool Breakable => true;

    public override bool IsContainer => true;

    public override bool IsUnprotected => false;

    public override int ScopeEnd => this.End - 3;

    public override void AddStatement(Statement statement)
        => this.m_statements.Add(statement);

    public override int GetLoopback()
        => throw new InvalidOperationException();

    public override void Print(Output output)
    {
        output.Print("for ");
        this.m_r.GetTarget(this.m_register + 3, this.Begin - 1).Print(output);
        var n = this.m_register + 2 + this.m_length;
        for (var r1 = this.m_register + 4; r1 <= n; r1++)
        {
            output.Print(", ");
            this.m_r.GetTarget(r1, this.Begin - 1).Print(output);
        }

        output.Print(" in ");

        // TODO: Optimize code
        this.PrintRecurse(this.m_r.GetValue(this.m_register, this.Begin - 1), output, 1);
        output.Print(" do");
        output.PrintLine();
        output.IncreaseIndent();
        PrintSequence(output, this.m_statements);
        output.DecreaseIndent();
        output.Print("end");
    }

    private void PrintRecurse(Expression expression, Output output, int regincrem)
    {
        expression.Print(output);
        if (!expression.IsMultiple)
        {
            output.Print(", ");
            this.PrintRecurse(
                this.m_r.GetValue(this.m_register + regincrem, this.Begin - 1),
                output,
                regincrem + 1);
        }
    }
}
