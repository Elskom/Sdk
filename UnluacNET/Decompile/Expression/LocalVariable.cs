// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class LocalVariable : Expression
{
    public LocalVariable(Declaration decl)
        : base(PRECEDENCE_ATOMIC)
        => this.Declaration = decl;

    public Declaration Declaration { get; private set; }

    public override int ConstantIndex => -1;

    public override bool IsBrief => true;

    public override bool IsDotChain => true;

    public override void Print(Output output)
        => output.Print(this.Declaration.Name);
}
