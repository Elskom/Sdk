// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class ReturnOperation : Operation
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private Expression[] m_values;

        public ReturnOperation(int line, Expression value)
            : base(line)
            => this.m_values = new Expression[1]
            {
                value,
            };

        public ReturnOperation(int line, Expression[] values)
            : base(line)
            => this.m_values = values;

        public override Statement Process(Registers r, Block block)
            => new Return(this.m_values);
    }
}
