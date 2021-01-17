// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;

    public class RepeatBlock : Block
    {
        private readonly Branch m_branch;
        private readonly Registers m_r;
        private readonly List<Statement> m_statements;

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
            Statement.PrintSequence(output, this.m_statements);
            output.DecreaseIndent();
            output.Print("until ");
            this.m_branch.AsExpression(this.m_r).Print(output);
        }

        public RepeatBlock(LFunction function, Branch branch, Registers r)
            : base(function, branch.End, branch.Begin)
        {
            this.m_branch = branch;
            this.m_r = r;
            this.m_statements = new List<Statement>(branch.Begin - branch.End + 1);
        }
    }
}
