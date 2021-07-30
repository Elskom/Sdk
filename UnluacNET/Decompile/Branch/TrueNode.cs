// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class TrueNode : Branch
{
    public TrueNode(int register, bool inverted, int line, int begin, int end)
        : base(line, begin, end)
    {
        this.Register = register;
        this.Inverted = inverted;
        this.SetTarget = register;
    }

    public int Register { get; }

    public bool Inverted { get; }

    public override Expression AsExpression(Registers registers)
        => new ConstantExpression(new(this.Inverted ? LBoolean.LTRUE : LBoolean.LFALSE), -1);

    public override int GetRegister()
        => this.Register;

    public override Branch Invert()
        => new TrueNode(this.Register, !this.Inverted, this.Line, this.End, this.Begin);

    public override void UseExpression(Expression expression)
    {
        // Do nothing
    }

    public override string ToString()
        => string.Format("TrueNode[register={0};inverted={1};line={2};begin={3};end={4}]", this.Register, this.Inverted, this.Line, this.Begin, this.End);
}
