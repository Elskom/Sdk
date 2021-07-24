// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class TableTarget : Target
    {
        private readonly Expression m_table;
        private readonly Expression m_index;

        public TableTarget(Expression table, Expression index)
        {
            this.m_table = table;
            this.m_index = index;
        }

        public override bool IsFunctionName => this.m_index.IsIdentifier && this.m_table.IsDotChain;

        public override void Print(Output output)
            => new TableReference(this.m_table, this.m_index).Print(output);

        public override void PrintMethod(Output output)
        {
            this.m_table.Print(output);
            output.Print(":");
            output.Print(this.m_index.AsName());
        }
    }
}
