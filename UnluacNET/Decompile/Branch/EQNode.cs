// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class EQNode : Branch
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly int m_left;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly int m_right;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly bool m_invert;

        public EQNode(int left, int right, bool invert, int line, int begin, int end)
            : base(line, begin, end)
        {
            this.m_left = left;
            this.m_right = right;
            this.m_invert = invert;
        }

        public override Branch Invert()
            => new EQNode(this.m_left, this.m_right, !this.m_invert, this.Line, this.End, this.Begin);

        public override int GetRegister()
            => -1;

        [SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "Don't care for now.")]
        public override Expression AsExpression(Registers registers)
        {
            var transpose = false;
            var op = this.m_invert ? "~=" : "==";
            return new BinaryExpression(
                op,
                registers.GetKExpression(!transpose ? this.m_left : this.m_right, this.Line),
                registers.GetKExpression(!transpose ? this.m_right : this.m_left, this.Line),
                Expression.PRECEDENCE_COMPARE,
                Expression.ASSOCIATIVITY_LEFT);
        }

        public override void UseExpression(Expression expression)
        {
            /* Do nothing */
        }
    }
}
