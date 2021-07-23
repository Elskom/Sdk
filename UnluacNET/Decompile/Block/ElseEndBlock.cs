// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;

    public class ElseEndBlock : Block
    {
        private readonly List<Statement> m_statements;

        public ElseEndBlock(LFunction function, int begin, int end)
            : base(function, begin, end)
            => this.m_statements = new(end - begin + 1);

        public IfThenElseBlock Partner { get; set; }

        public override bool Breakable => false;

        public override bool IsContainer => true;

        public override bool IsUnprotected => false;

        public override void AddStatement(Statement statement)
            => this.m_statements.Add(statement);

        public override int CompareTo(Block other)
            => other == this.Partner ? 1 : base.CompareTo(other);

        public override int GetLoopback()
            => throw new InvalidOperationException();

        public override void Print(Output output)
        {
            output.Print("else");
            if (this.m_statements.Count == 1 && this.m_statements[0] is IfThenEndBlock)
            {
                this.m_statements[0].Print(output);
            }
            else if (this.m_statements.Count == 2 && this.m_statements[0] is IfThenElseBlock && this.m_statements[1] is ElseEndBlock)
            {
                this.m_statements[0].Print(output);
                this.m_statements[1].Print(output);
            }
            else
            {
                output.PrintLine();
                output.IncreaseIndent();
                PrintSequence(output, this.m_statements);
                output.DecreaseIndent();
                output.Print("end");
            }
        }
    }
}
