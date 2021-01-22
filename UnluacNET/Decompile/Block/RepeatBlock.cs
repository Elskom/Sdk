// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class RepeatBlock : Block
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Branch m_branch;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Registers m_r;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly List<Statement> m_statements;

        public RepeatBlock(LFunction function, Branch branch, Registers r)
            : base(function, branch.End, branch.Begin)
        {
            this.m_branch = branch;
            this.m_r = r;
            this.m_statements = new List<Statement>(branch.Begin - branch.End + 1);
        }

        public override bool Breakable => true;

        public override bool IsContainer => true;

        public override bool IsUnprotected => false;

        public override void AddStatement(Statement statement)
            => this.m_statements.Add(statement);

        public override int GetLoopback()
            => throw new InvalidOperationException();

        public override void Print(Output output)
        {
            output.Print("repeat");
            output.PrintLine();
            output.IncreaseIndent();
            PrintSequence(output, this.m_statements);
            output.DecreaseIndent();
            output.Print("until ");
            this.m_branch.AsExpression(this.m_r).Print(output);
        }
    }
}
