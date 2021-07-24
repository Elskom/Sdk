// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class Upvalues
    {
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
