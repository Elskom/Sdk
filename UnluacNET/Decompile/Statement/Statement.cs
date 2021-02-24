// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Collections.Generic;

    public abstract class Statement
    {
        public string Comment { get; set; }

        public virtual bool BeginsWithParen => false;

        public static void PrintSequence(Output output, List<Statement> statements)
        {
            var count = statements.Count;
            for (var i = 0; i < count; i++)
            {
                var last = i + 1 == count;
                var statement = statements[i];
                var next = last ? null : statements[i + 1];
                if (last)
                {
                    statement.PrintTail(output);
                }
                else
                {
                    statement.Print(output);
                }

                if (next != null && statement is FunctionCallStatement && next.BeginsWithParen)
                {
                    output.Print(";");
                }

                if (!(statement is IfThenElseBlock))
                {
                    output.PrintLine();
                }
            }
        }

        public abstract void Print(Output output);

        public virtual void PrintTail(Output output)
            => this.Print(output);
    }
}
