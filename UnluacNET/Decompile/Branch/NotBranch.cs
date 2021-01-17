// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class NotBranch : Branch
    {
        private readonly Branch m_branch;

        public override Expression AsExpression(Registers registers)
            => Expression.MakeNOT(this.m_branch.AsExpression(registers));

        public override int GetRegister()
            => this.m_branch.GetRegister();

        public override Branch Invert()
            => this.m_branch;

        public override void UseExpression(Expression expression)
        {
            // Do nothing
            return;
        }

        public NotBranch(Branch branch)
            : base(branch.Line, branch.Begin, branch.End)
            => this.m_branch = branch;
    }
}
