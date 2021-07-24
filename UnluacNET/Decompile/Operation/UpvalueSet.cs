// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class UpvalueSet : Operation
    {
        private readonly UpvalueTarget m_target;
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
