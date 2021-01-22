// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class WhileBlock : Block
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Branch m_branch;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly int m_loopback;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Registers m_registers;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly List<Statement> m_statements;

        public WhileBlock(LFunction function, Branch branch, int loopback, Registers registers)
            : base(function, branch.Begin, branch.End)
        {
            this.m_branch = branch;
            this.m_loopback = loopback;
            this.m_registers = registers;
            this.m_statements = new List<Statement>(branch.End - branch.Begin + 1);
        }

        public override int ScopeEnd => this.End - 2;

        public override bool Breakable => true;

        public override bool IsContainer => true;

        public override bool IsUnprotected => true;

        public override void AddStatement(Statement statement)
            => this.m_statements.Add(statement);

        public override int GetLoopback()
            => this.m_loopback;

        public override void Print(Output output)
        {
            output.Print("while ");
            this.m_branch.AsExpression(this.m_registers).Print(output);
            output.Print(" do");
            output.PrintLine();
            output.IncreaseIndent();
            PrintSequence(output, this.m_statements);
            output.DecreaseIndent();
            output.Print("end");
        }
    }
}
