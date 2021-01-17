// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;

    public class ElseEndBlock : Block
    {
        private readonly List<Statement> m_statements;

        public IfThenElseBlock Partner { get; set; }

        public override bool Breakable => false;

        public override bool IsContainer => true;

        public override bool IsUnprotected => false;

        public override void AddStatement(Statement statement)
            => this.m_statements.Add(statement);

        public override int CompareTo(Block block)
        {
            if (block == this.Partner)
                return 1;

            var result = base.CompareTo(block);
            //if (result == 0 && block is ElseEndBlock)
            //    Console.WriteLine("HEY HEY HEY");

            return result;
        }

        public override int GetLoopback()
            => throw new InvalidOperationException();

        public override void Print(Output output)
        {
            output.Print("else");
            if (this.m_statements.Count == 1 && this.m_statements[0] is IfThenEndBlock)
            {    
                this.m_statements[0].Print(output);
            }
            else if (this.m_statements.Count == 2 && this.m_statements[0] is IfThenElseBlock && this.m_statements[1] is ElseEndBlock)
            {
                this.m_statements[0].Print(output);
                this.m_statements[1].Print(output);
            }
            else
            {
                output.PrintLine();
                output.IncreaseIndent();
                Statement.PrintSequence(output, this.m_statements);
                output.DecreaseIndent();
                output.Print("end");
            }
        }

        public ElseEndBlock(LFunction function, int begin, int end)
            : base(function, begin, end)
            => this.m_statements = new List<Statement>(end - begin + 1);
    }
}
