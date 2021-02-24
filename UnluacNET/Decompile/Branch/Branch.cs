// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public abstract class Branch
    {
        protected Branch(int line, int begin, int end)
        {
            this.Line = line;
            this.Begin = begin;
            this.End = end;
        }

        public int Line { get; private set; }

        public int Begin { get; set; }

        public int End { get; set; } // Might be modified to undo redirect

        public bool IsSet { get; set; }

        public bool IsCompareSet { get; set; }

        public bool IsTest { get; set; }

        public int SetTarget { get; set; } = -1;

        public abstract Branch Invert();

        public abstract int GetRegister();

        public abstract Expression AsExpression(Registers registers);

        public abstract void UseExpression(Expression expression);
    }
}
