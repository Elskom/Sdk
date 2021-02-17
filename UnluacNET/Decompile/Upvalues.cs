// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class Upvalues
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly LUpvalue[] m_upvalues;

        public Upvalues(LUpvalue[] upvalues)
            => this.m_upvalues = upvalues;

        public string GetName(int idx)
            => idx < this.m_upvalues.Length && this.m_upvalues[idx].Name != null
            ? this.m_upvalues[idx].Name
            : string.Format("_UPVALUE{0}_", idx);

        public UpvalueExpression GetExpression(int index)
            => new(this.GetName(index));
    }
}
