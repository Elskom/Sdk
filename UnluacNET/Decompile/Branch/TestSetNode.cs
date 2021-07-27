// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class TestSetNode : TestNode
{
    public TestSetNode(int target, int test, bool inverted, int line, int begin, int end)
        : base(test, inverted, line, begin, end)
        => this.SetTarget = target;

    public override Expression AsExpression(Registers registers)
        => registers.GetExpression(this.Test, this.Line);

    public override int GetRegister()
        => this.SetTarget;

    public override Branch Invert()
        => new TestSetNode(this.SetTarget, this.Test, !this.Inverted, this.Line, this.End, this.Begin);

    public override string ToString()
        => $"TestSetNode[target={this.SetTarget};test={this.Test};inverted={this.Inverted};line={this.Line};begin={this.Begin};end={this.End}]";
}
