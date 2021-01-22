// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class Vararg : Expression
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly int m_length;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly bool m_multiple;

        public Vararg(int length, bool multiple)
            : base(PRECEDENCE_ATOMIC)
        {
            this.m_length = length;
            this.m_multiple = multiple;
        }

        public override int ConstantIndex => -1;

        public override bool IsMultiple => this.m_multiple;

        public override void Print(Output output)
            => output.Print(this.m_multiple ? "..." : "(...)");

        public override void PrintMultiple(Output output)
            => output.Print(this.m_multiple ? "..." : "(...)");
    }
}
