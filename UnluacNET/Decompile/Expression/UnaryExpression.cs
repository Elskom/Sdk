// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class UnaryExpression : Expression
    {
        private readonly string m_op;
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
