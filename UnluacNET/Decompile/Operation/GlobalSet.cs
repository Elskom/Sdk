// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class GlobalSet : Operation
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private string m_global;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private Expression m_value;

        public GlobalSet(int line, string global, Expression value)
            : base(line)
        {
            this.m_global = global;
            this.m_value = value;
        }

        public override Statement Process(Registers r, Block block)
            => new Assignment(new GlobalTarget(this.m_global), this.m_value);
    }
}
