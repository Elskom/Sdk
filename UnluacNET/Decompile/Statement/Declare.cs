// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Collections.Generic;

    public class Declare : Statement
    {
        private readonly List<Declaration> m_decls;

        public Declare(List<Declaration> decls)
            => this.m_decls = decls;

        public override void Print(Output output)
        {
            output.Print("local ");
            output.Print(this.m_decls[0].Name);
            for (var i = 1; i < this.m_decls.Count; i++)
            {
                output.Print(", ");
                output.Print(this.m_decls[i].Name);
            }
        }
    }
}
