// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class Declaration
    {
        public string Name { get; private set; }
        public int Begin { get; private set; }
        public int End { get; private set; }
        public int Register { get; set; }

        //Whether this is an invisible for-loop book-keeping variable.
        internal bool ForLoop { get; set; }

        //Whether this is an explicit for-loop declared variable.
        internal bool ForLoopExplicit { get; set; }

        public Declaration(LLocal local)
        {
            this.Name = local.ToString();
            this.Begin = local.Start;
            this.End = local.End;
        }

        public Declaration(string name, int begin, int end)
        {
            this.Name = name;
            this.Begin = begin;
            this.End = end;
        }
    }
}
