// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class NotBranch : Branch
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Branch m_branch;

        public NotBranch(Branch branch)
            : base(branch.Line, branch.Begin, branch.End)
            => this.m_branch = branch;

        public override Expression AsExpression(Registers registers)
            => Expression.MakeNOT(this.m_branch.AsExpression(registers));

        public override int GetRegister()
            => this.m_branch.GetRegister();

        public override Branch Invert()
            => this.m_branch;

        public override void UseExpression(Expression expression)
        {
            // Do nothing
        }
    }
}
