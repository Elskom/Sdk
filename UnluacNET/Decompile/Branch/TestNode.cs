// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class TestNode : Branch
    {
        public TestNode(int test, bool inverted, int line, int begin, int end)
            : base(line, begin, end)
        {
            this.Test = test;
            this.Inverted = inverted;
            this.IsTest = true;
        }

        public int Test { get; }

        public bool Inverted { get; }

        public override Expression AsExpression(Registers registers)
            => this.Inverted ? new NotBranch(this.Invert()).AsExpression(registers) : registers.GetExpression(this.Test, this.Line);

        public override int GetRegister()
            => this.Test;

        public override Branch Invert()
            => new TestNode(this.Test, !this.Inverted, this.Line, this.End, this.Begin);

        public override void UseExpression(Expression expression)
        {
            // Do nothing
        }

        public override string ToString()
            => string.Format("TestNode[test={0};inverted={1};line={2};begin={3};end={4}]", this.Test, this.Inverted, this.Line, this.Begin, this.End);
    }
}
