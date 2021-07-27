// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class TableSet : Operation
{
    private readonly Expression m_table;
    private readonly Expression m_index;
    private readonly Expression m_value;
    private readonly bool m_isTable;
    private readonly int m_timestamp;

    public TableSet(int line, Expression table, Expression index, Expression value, bool isTable, int timestamp)
        : base(line)
    {
        this.m_table = table;
        this.m_index = index;
        this.m_value = value;
        this.m_isTable = isTable;
        this.m_timestamp = timestamp;
    }

    public override Statement Process(Registers r, Block block)
    {
        // .isTableLiteral() is sufficient when there is debugging info
        // TODO: Fix the commented out section screwing up tables
        if (this.m_table.IsTableLiteral /*&& (m_value.IsMultiple || m_table.IsNewEntryAllowed)*/)
        {
            this.m_table.AddEntry(new TableLiteral.Entry(this.m_index, this.m_value, !this.m_isTable, this.m_timestamp));
            return null;
        }

        return new Assignment(new TableTarget(this.m_table, this.m_index), this.m_value);
    }
}
