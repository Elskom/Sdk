// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public abstract class Operation
    {
        protected Operation(int line)
            => this.Line = line;

        public int Line { get; private set; }

        public abstract Statement Process(Registers r, Block block);
    }
}
