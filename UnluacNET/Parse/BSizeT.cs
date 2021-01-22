// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class BSizeT : BInteger
    {
        public BSizeT(BInteger b)
            : base(b)
        {
        }

        public BSizeT(int n)
            : base(n)
        {
        }

        public BSizeT(long n)
            : base(n)
        {
        }
    }
}
