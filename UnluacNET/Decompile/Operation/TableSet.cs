// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class TableSet : Operation
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression m_table;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression m_index;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression m_value;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly bool m_isTable;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
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
            else
            {
                return new Assignment(new TableTarget(this.m_table, this.m_index), this.m_value);
            }
        }
    }
}
