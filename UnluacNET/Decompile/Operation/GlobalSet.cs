// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class GlobalSet : Operation
    {
        private string m_global;
        private Expression m_value;

        public override Statement Process(Registers r, Block block)
            => new Assignment(new GlobalTarget(this.m_global), this.m_value);

        public GlobalSet(int line, string global, Expression value)
            : base(line)
        {
            this.m_global = global;
            this.m_value = value;
        }
    }
}
