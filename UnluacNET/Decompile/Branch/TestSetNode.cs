// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class TestSetNode : TestNode
    {
        public override Expression AsExpression(Registers registers)
            => registers.GetExpression(this.Test, this.Line);

        public override int GetRegister()
            => this.SetTarget;

        public override Branch Invert()
            => new TestSetNode(this.SetTarget, this.Test, !this.Inverted, this.Line, this.End, this.Begin);

        public override string ToString()
            => string.Format("TestSetNode[target={0};test={1};inverted={2};line={3};begin={4};end={5}]",
                this.Test, this.Inverted, this.Line, this.Begin, this.End);

        public TestSetNode(int target, int test, bool inverted, int line, int begin, int end)
            : base(test, inverted, line, begin, end)
            => SetTarget = target;
    }
}
