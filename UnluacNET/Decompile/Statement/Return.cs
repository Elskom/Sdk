// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Collections.Generic;
    
    public class Return : Statement
    {
        private Expression[] values;

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
                    rtns.Add(value);

                Expression.PrintSequence(output, rtns, false, true);
            }
        }

        public Return()
            => values = new Expression[0];

        public Return(Expression value)
            => values = new Expression[1]
            {
                value
            };

        public Return(Expression[] values)
            => this.values = values;
    }
}
