// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;

    public class TForBlock : Block
    {
        private readonly int m_register;
        private readonly int m_length;
        private readonly Registers m_r;
        private readonly List<Statement> m_statements;

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
            Expression value = null;
            value = this.m_r.GetValue(this.m_register, this.Begin - 1);
            value.Print(output);

            // TODO: Optimize code
            if (!value.IsMultiple)
            {
                output.Print(", ");
                value = this.m_r.GetValue(this.m_register + 1, this.Begin - 1);
                value.Print(output);
                if (!value.IsMultiple)
                {
                    output.Print(", ");
                    value = this.m_r.GetValue(this.m_register + 2, this.Begin - 1);
                    value.Print(output);
                }
            }

            output.Print(" do");
            output.PrintLine();
            output.IncreaseIndent();
            Statement.PrintSequence(output, this.m_statements);
            output.DecreaseIndent();
            output.Print("end");
        }

        public TForBlock(LFunction function, int begin, int end, int register, int length, Registers r)
            : base(function, begin, end)
        {
            this.m_register = register;
            this.m_length = length;
            this.m_r = r;
            this.m_statements = new List<Statement>(end - begin + 1);
        }
    }
}
