// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class SetBlock : Block
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly bool m_empty;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Registers m_r;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private Assignment m_assign;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private bool m_finalize;

        public SetBlock(LFunction function, Branch branch, int target, int begin, int end, bool empty, Registers r)
            : base(function, begin, end)
        {
            this.m_empty = empty;
            if (begin == end)
            {
                this.Begin -= 1;
            }

            this.Target = target;
            this.Branch = branch;
            this.m_r = r;
        }

        public int Target { get; private set; }

        public Branch Branch { get; private set; }

        public override bool Breakable => false;

        public override bool IsContainer => false;

        public override bool IsUnprotected => false;

        public override void AddStatement(Statement statement)
        {
            if (!this.m_finalize && statement is Assignment)
            {
                this.m_assign = statement as Assignment;
            }
            else if (statement is BooleanIndicator)
            {
                this.m_finalize = true;
            }
        }

        public override int GetLoopback()
            => throw new InvalidOperationException();

        public Expression GetValue()
            => this.Branch.AsExpression(this.m_r);

        public override void Print(Output output)
        {
            if (this.m_assign != null)
            {
                var target = this.m_assign.GetFirstTarget();
                if (target != null)
                {
                    new Assignment(target, this.GetValue()).Print(output);
                }
                else
                {
                    output.Print("-- unhandled set block");
                    output.PrintLine();
                }
            }
        }

        public override Operation Process(Decompiler d)
        {
            if (this.m_empty)
            {
                var expr = this.m_r.GetExpression(this.Branch.SetTarget, this.End);
                this.Branch.UseExpression(expr);
                return new RegisterSet(this.End - 1, this.Branch.SetTarget, this.Branch.AsExpression(this.m_r));
            }
            else if (this.m_assign != null)
            {
                this.Branch.UseExpression(this.m_assign.GetFirstValue());
                var target = this.m_assign.GetFirstTarget();
                var value = this.GetValue();
                return new LambdaOperation(this.End - 1, (r, block) =>
                {
                    return new Assignment(target, value);
                });
            }
            else
            {
                return new LambdaOperation(this.End - 1, (r, block) =>
                {
                    Expression expr = null;
                    var register = 0;
                    for (; register < r.NumRegisters; register++)
                    {
                        if (r.GetUpdated(register, this.Branch.End - 1) == this.Branch.End - 1)
                        {
                            expr = r.GetValue(register, this.Branch.End);
                            break;
                        }
                    }

                    if (d.Code.Op(this.Branch.End - 2) == Op.LOADBOOL &&
                        d.Code.C(this.Branch.End - 2) != 0)
                    {
                        var target = d.Code.A(this.Branch.End - 2);
                        expr = d.Code.Op(this.Branch.End - 3) == Op.JMP &&
                            d.Code.SBx(this.Branch.End - 3) == 2
                            ? r.GetValue(target, this.Branch.End - 2)
                            : r.GetValue(target, this.Branch.Begin);
                        this.Branch.UseExpression(expr);
                        if (r.IsLocal(target, this.Branch.End - 1))
                        {
                            return new Assignment(r.GetTarget(target, this.Branch.End - 1), this.Branch.AsExpression(r));
                        }

                        r.SetValue(target, this.Branch.End - 1, this.Branch.AsExpression(r));
                    }
                    else if (expr != null && this.Target >= 0)
                    {
                        this.Branch.UseExpression(expr);
                        if (r.IsLocal(this.Target, this.Branch.End - 1))
                        {
                            return new Assignment(r.GetTarget(this.Target, this.Branch.End - 1), this.Branch.AsExpression(r));
                        }

                        r.SetValue(this.Target, this.Branch.End - 1, this.Branch.AsExpression(r));
                    }
                    else
                    {
                        Console.WriteLine("-- fail " + (this.Branch.End - 1));
                        Console.WriteLine(expr);
                        Console.WriteLine(this.Target);
                    }

                    return null;
                });
            }
        }

        public void UseAssignment(Assignment assignment)
        {
            this.m_assign = assignment;
            this.Branch.UseExpression(assignment.GetFirstValue());
        }
    }
}
