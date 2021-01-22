// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class LambdaOperation : Operation
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Func<Registers, Block, Statement> m_func;

        public LambdaOperation(int line, Func<Registers, Block, Statement> func)
            : base(line)
            => this.m_func = func;

        public override Statement Process(Registers r, Block block)
            => this.m_func.Invoke(r, block);
    }
}
