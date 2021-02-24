// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class LLocal : BObject
    {
        public LLocal(LString name, BInteger start, BInteger end)
        {
            this.Name = name;
            this.Start = start.AsInteger();
            this.End = end.AsInteger();
        }

        public LString Name { get; private set; }

        public int Start { get; private set; }

        public int End { get; private set; }

        /* Used by the decompiler for annotation. */
        internal bool ForLoop { get; set; }

        public override string ToString()
            => this.Name.DeRef();
    }
}
