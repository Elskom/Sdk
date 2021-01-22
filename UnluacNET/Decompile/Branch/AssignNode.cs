// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class AssignNode : Branch
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private Expression m_expression;

        public AssignNode(int line, int begin, int end)
            : base(line, begin, end)
        {
        }

        public override Expression AsExpression(Registers registers)
            => this.m_expression;

        public override int GetRegister()
            => throw new InvalidOperationException();

        public override Branch Invert()
            => throw new InvalidOperationException();

        public override void UseExpression(Expression expression)
            => this.m_expression = expression;
    }
}
