// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class LNil : LObject
    {
        public static readonly LNil NIL = new LNil();

        private LNil()
        {
        }

        public override bool Equals(object obj)
            => this == obj;

        public override int GetHashCode()
            => throw new NotImplementedException();
    }
}
