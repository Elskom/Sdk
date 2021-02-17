// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class UpvalueSet : Operation
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly UpvalueTarget m_target;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression m_value;

        public UpvalueSet(int line, string upvalue, Expression value)
            : base(line)
        {
            this.m_target = new UpvalueTarget(upvalue);
            this.m_value = value;
        }

        public override Statement Process(Registers r, Block block)
            => new Assignment(this.m_target, this.m_value);
    }
}
