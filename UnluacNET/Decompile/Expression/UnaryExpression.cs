// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class UnaryExpression : Expression
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly string m_op;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression m_expression;

        public UnaryExpression(string op, Expression expression, int precedence)
            : base(precedence)
        {
            this.m_op = op;
            this.m_expression = expression;
        }

        public override int ConstantIndex => this.m_expression.ConstantIndex;

        public override void Print(Output output)
        {
            var precedence = this.Precedence > this.m_expression.Precedence;
            output.Print(this.m_op);
            if (precedence)
            {
                output.Print("(");
            }

            this.m_expression.Print(output);
            if (precedence)
            {
                output.Print(")");
            }
        }
    }
}
