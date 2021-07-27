// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

using System.Collections.Generic;

public class IfThenElseBlock : Block
{
    private readonly Branch m_branch;
    private readonly int m_loopback;
    private readonly Registers m_r;
    private readonly List<Statement> m_statements;
    private readonly bool m_emptyElse;

    public IfThenElseBlock(LFunction function, Branch branch, int loopback, bool emptyElse, Registers r)
        : base(function, branch.Begin, branch.End)
    {
        this.m_branch = branch;
        this.m_loopback = loopback;
        this.m_emptyElse = emptyElse;
        this.m_r = r;
        this.m_statements = new(branch.End - branch.Begin + 1);
    }

    public ElseEndBlock Partner { get; set; }

    public override bool Breakable => false;

    public override bool IsContainer => true;

    public override bool IsUnprotected => true;

    public override int ScopeEnd => this.End - 2;

    public override void AddStatement(Statement statement)
        => this.m_statements.Add(statement);

    public override int CompareTo(Block other)
        => other == this.Partner ? -1 : base.CompareTo(other);

    public override int GetLoopback()
        => this.m_loopback;

    public override void Print(Output output)
    {
        output.Print("if ");
        this.m_branch.AsExpression(this.m_r).Print(output);
        output.Print(" then");
        output.PrintLine();
        output.IncreaseIndent();

        // Handle the case where the "then" is empty in if-then-else.
        // The jump over the else block is falsely detected as a break.
        if (this.m_statements.Count == 1 && this.m_statements[0] is Break)
        {
            var b = this.m_statements[0] as Break;
            if (b.Target == this.m_loopback)
            {
                output.DecreaseIndent();
                return;
            }
        }

        PrintSequence(output, this.m_statements);
        output.DecreaseIndent();
        if (this.m_emptyElse)
        {
            output.PrintLine("else");
            output.PrintLine("end");
        }
    }
}
