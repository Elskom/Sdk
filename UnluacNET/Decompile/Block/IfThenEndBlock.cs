// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

using System;
using System.Collections.Generic;

public class IfThenEndBlock : Block
{
    private readonly Branch m_branch;
    private readonly Stack<Branch> m_stack;
    private readonly Registers m_r;
    private readonly List<Statement> m_statements;

    public IfThenEndBlock(LFunction function, Branch branch, Registers r)
        : this(function, branch, null, r)
    {
    }

    public IfThenEndBlock(LFunction function, Branch branch, Stack<Branch> stack, Registers r)
        : base(function, branch.Begin == branch.End ? branch.Begin - 1 : branch.Begin, branch.Begin == branch.End ? branch.Begin - 1 : branch.End)
    {
        this.m_branch = branch;
        this.m_stack = stack;
        this.m_r = r;
        this.m_statements = new(branch.End - branch.Begin + 1);
    }

    public override bool Breakable => false;

    public override bool IsContainer => true;

    public override bool IsUnprotected => false;

    public override void AddStatement(Statement statement)
        => this.m_statements.Add(statement);

    public override int GetLoopback()
        => throw new InvalidOperationException();

    public override void Print(Output output)
    {
        output.Print("if ");
        this.m_branch.AsExpression(this.m_r).Print(output);
        output.Print(" then");
        output.PrintLine();
        output.IncreaseIndent();
        PrintSequence(output, this.m_statements);
        output.DecreaseIndent();
        output.Print("end");
    }

    public override Operation Process(Decompiler d)
    {
        if (this.m_statements.Count == 1)
        {
            var statement = this.m_statements[0];
            if (statement is Assignment)
            {
                var assignment = statement as Assignment;
                if (assignment.GetArity() == 1 && this.m_branch is TestNode)
                {
                    var node = this.m_branch as TestNode;
                    var decl = this.m_r.GetDeclaration(node.Test, node.Line);
                    if (assignment.GetFirstTarget().IsDeclaration(decl))
                    {
                        LocalVariable left = new(decl);
                        var right = assignment.GetFirstValue();
                        var expr = node.Inverted
                            ? Expression.MakeOR(left, right)
                            : Expression.MakeAND(left, right);
                        return new LambdaOperation(this.End - 1, (_, _) =>
                        {
                            return new Assignment(assignment.GetFirstTarget(), expr);
                        });
                    }
                }
            }
        }
        else if (this.m_statements.Count == 0 && this.m_stack != null)
        {
            var test = this.m_branch.GetRegister();
            if (test < 0)
            {
                for (var reg = 0; reg < this.m_r.NumRegisters; reg++)
                {
                    if (this.m_r.GetUpdated(reg, this.m_branch.End - 1) >= this.m_branch.Begin)
                    {
                        if (test >= 0)
                        {
                            test = -1;
                            break;
                        }

                        test = reg;
                    }
                }
            }

            if (test >= 0 && this.m_r.GetUpdated(test, this.m_branch.End - 1) >= this.m_branch.Begin)
            {
                var right = this.m_r.GetValue(test, this.m_branch.End);
                var setb = d.PopSetCondition(this.m_stack, this.m_stack.Peek().End, test);
                setb.UseExpression(right);
                var testReg = test;
                return new LambdaOperation(this.End - 1, (r, _) =>
                {
                    r.SetValue(testReg, this.m_branch.End - 1, setb.AsExpression(r));
                    return null;
                });
            }
        }

        return base.Process(d);
    }
}
