// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;

    public class OuterBlock : Block
    {
        private readonly List<Statement> m_statements;

        public OuterBlock(LFunction function, int length)
            : base(function, 0, length + 1)
            => this.m_statements = new List<Statement>(length);

        public override bool Breakable => false;

        public override bool IsContainer => true;

        public override bool IsUnprotected => false;

        public override int ScopeEnd => this.End - 1 + this.Function.Header.Version.OuterBlockScopeAdjustment;

        public override void AddStatement(Statement statement)
            => this.m_statements.Add(statement);

        public override int GetLoopback()
            => throw new InvalidOperationException();

        public override void Print(Output output)
        {
            /* extra return statement */
            var last = this.m_statements.Count - 1;
            if (last < 0 || !(this.m_statements[last] is Return))
            {
                throw new InvalidOperationException(this.m_statements[last].ToString());
            }

            // this doesn't seem like appropriate behavior???
            this.m_statements.RemoveAt(last);
            PrintSequence(output, this.m_statements);
        }
    }
}
