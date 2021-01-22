// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class TableTarget : Target
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression m_table;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
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
