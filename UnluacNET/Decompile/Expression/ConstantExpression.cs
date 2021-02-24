// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class ConstantExpression : Expression
    {
        private readonly Constant m_constant;
        private readonly int m_index;

        public ConstantExpression(Constant constant, int index)
            : base(PRECEDENCE_ATOMIC)
        {
            this.m_constant = constant;
            this.m_index = index;
        }

        public override int ConstantIndex => this.m_index;

        public override bool IsConstant => true;

        public override bool IsBoolean => this.m_constant.IsBoolean;

        public override bool IsBrief => !this.m_constant.IsString || this.m_constant.AsName().Length <= 10;

        public override bool IsIdentifier => this.m_constant.IsIdentifier;

        public override bool IsInteger => this.m_constant.IsInteger;

        public override bool IsString => this.m_constant.IsString;

        public override bool IsNil => this.m_constant.IsNil;

        public override int AsInteger()
            => this.m_constant.AsInteger();

        public override string AsName()
            => this.m_constant.AsName();

        public override void Print(Output output)
            => this.m_constant.Print(output);
    }
}
