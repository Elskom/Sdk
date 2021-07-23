// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class GlobalSet : Operation
    {
        private readonly string m_global;
        private readonly Expression m_value;

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
