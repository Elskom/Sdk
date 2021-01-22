// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class BinaryExpression : Expression
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly string m_op;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression m_left;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression m_right;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly int m_associativity;

        public BinaryExpression(string op, Expression left, Expression right, int precedence, int associativity)
            : base(precedence)
        {
            this.m_op = op;
            this.m_left = left;
            this.m_right = right;
            this.m_associativity = associativity;
        }

        public override int ConstantIndex
            => Math.Max(this.m_left.ConstantIndex, this.m_right.ConstantIndex);

        public override bool BeginsWithParen
            => this.LeftGroup || this.m_left.BeginsWithParen;

        protected bool LeftGroup
            => (this.Precedence > this.m_left.Precedence) ||
            (this.Precedence == this.m_left.Precedence && this.m_associativity == ASSOCIATIVITY_RIGHT);

        protected bool RightGroup
            => (this.Precedence > this.m_right.Precedence) ||
            (this.Precedence == this.m_right.Precedence && this.m_associativity == ASSOCIATIVITY_LEFT);

        public override void Print(Output output)
        {
            var leftGroup = this.LeftGroup;
            var rightGroup = this.RightGroup;
            if (leftGroup)
            {
                output.Print("(");
            }

            this.m_left.Print(output);
            if (leftGroup)
            {
                output.Print(")");
            }

            output.Print(" ");
            output.Print(this.m_op);
            output.Print(" ");
            if (rightGroup)
            {
                output.Print("(");
            }

            this.m_right.Print(output);
            if (rightGroup)
            {
                output.Print(")");
            }
        }
    }
}
