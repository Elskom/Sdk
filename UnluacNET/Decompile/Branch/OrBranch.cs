// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class OrBranch : Branch
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Branch m_left;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Branch m_right;

        public OrBranch(Branch left, Branch right)
            : base(right.Line, right.Begin, right.End)
        {
            this.m_left = left;
            this.m_right = right;
        }

        public override Expression AsExpression(Registers registers)
            => Expression.MakeOR(this.m_left.AsExpression(registers), this.m_right.AsExpression(registers));

        public override int GetRegister()
        {
            var rLeft = this.m_left.GetRegister();
            var rRight = this.m_right.GetRegister();
            return (rLeft == rRight) ? rLeft : -1;
        }

        public override Branch Invert()
            => new AndBranch(this.m_left.Invert(), this.m_right.Invert());

        public override void UseExpression(Expression expression)
        {
            this.m_left.UseExpression(expression);
            this.m_right.UseExpression(expression);
        }
    }
}
