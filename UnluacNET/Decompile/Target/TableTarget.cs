// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class TableTarget : Target
    {
        private readonly Expression m_table;
        private readonly Expression m_index;

        public override bool IsFunctionName
        {
            get
            {
                if (!this.m_index.IsIdentifier)
                    return false;
                if (!this.m_table.IsDotChain)
                    return false;

                return true;
            }
        }

        public override void Print(Output output)
            => new TableReference(this.m_table, this.m_index).Print(output);

        public override void PrintMethod(Output output)
        {
            this.m_table.Print(output);
            output.Print(":");
            output.Print(this.m_index.AsName());
        }

        public TableTarget(Expression table, Expression index)
        {
            this.m_table = table;
            this.m_index = index;
        }
    }
}
