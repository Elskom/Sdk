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
    public class ForBlock : Block
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly int m_register;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Registers m_r;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly List<Statement> m_statements;

        public ForBlock(LFunction function, int begin, int end, int register, Registers r)
            : base(function, begin, end)
        {
            this.m_register = register;
            this.m_r = r;
            this.m_statements = new List<Statement>(end - begin + 1);
        }

        public override bool Breakable => true;

        public override bool IsContainer => true;

        public override bool IsUnprotected => false;

        public override int ScopeEnd => this.End - 2;

        public override void AddStatement(Statement statement)
            => this.m_statements.Add(statement);

        public override int GetLoopback()
            => throw new InvalidOperationException();

        public override void Print(Output output)
        {
            output.Print("for ");
            this.m_r.GetTarget(this.m_register + 3, this.Begin - 1).Print(output);
            output.Print(" = ");
            this.m_r.GetValue(this.m_register, this.Begin - 1).Print(output);
            output.Print(", ");
            this.m_r.GetValue(this.m_register + 1, this.Begin - 1).Print(output);
            var step = this.m_r.GetValue(this.m_register + 2, this.Begin - 1);
            if (!step.IsInteger || step.AsInteger() != 1)
            {
                output.Print(", ");
                step.Print(output);
            }

            output.Print(" do");
            output.PrintLine();
            output.IncreaseIndent();
            PrintSequence(output, this.m_statements);
            output.DecreaseIndent();
            output.Print("end");
        }
    }
}
