// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
#if !NETFRAMEWORK
    using System;
#endif
    using System.Collections.Generic;

    public class Return : Statement
    {
        private readonly Expression[] values;

        public Return()
#if NETFRAMEWORK
            => this.values = new Expression[0];
#else
            => this.values = Array.Empty<Expression>();
#endif

        public Return(Expression value)
            => this.values = new Expression[1]
            {
                value,
            };

        public Return(Expression[] values)
            => this.values = values;

        public override void Print(Output output)
        {
            output.Print("do ");
            this.PrintTail(output);
            output.Print(" end");
        }

        public override void PrintTail(Output output)
        {
            output.Print("return");
            if (this.values.Length > 0)
            {
                output.Print(" ");
                var rtns = new List<Expression>(this.values.Length);
                foreach (var value in this.values)
                {
                    rtns.Add(value);
                }

                Expression.PrintSequence(output, rtns, false, true);
            }
        }
    }
}
