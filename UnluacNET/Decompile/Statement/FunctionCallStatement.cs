// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class FunctionCallStatement : Statement
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly FunctionCall m_call;

        public FunctionCallStatement(FunctionCall call)
            => this.m_call = call;

        public override bool BeginsWithParen => this.m_call.BeginsWithParen;

        public override void Print(Output output)
            => this.m_call.Print(output);
    }
}
