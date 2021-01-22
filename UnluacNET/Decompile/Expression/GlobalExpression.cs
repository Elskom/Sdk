// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class GlobalExpression : Expression
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly string m_name;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly int m_index;

        public GlobalExpression(string name, int index)
            : base(PRECEDENCE_ATOMIC)
        {
            this.m_name = name;
            this.m_index = index;
        }

        public override int ConstantIndex => this.m_index;

        public override bool IsBrief => true;

        public override bool IsDotChain => true;

        public override void Print(Output output)
            => output.Print(this.m_name);
    }
}
