// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class Upvalues
    {
        private readonly LUpvalue[] m_upvalues;

        public string GetName(int idx)
        {
            if (idx < this.m_upvalues.Length && this.m_upvalues[idx].Name != null)
                return this.m_upvalues[idx].Name;

            return string.Format("_UPVALUE{0}_", idx);
        }

        public UpvalueExpression GetExpression(int index)
            => new UpvalueExpression(this.GetName(index));

        public Upvalues(LUpvalue[] upvalues)
            => this.m_upvalues = upvalues;
    }
}
