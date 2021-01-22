// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class LString : LObject
    {
        public LString(BSizeT size, string value)
        {
            this.Size = size;
            this.Value = (value.Length == 0) ? string.Empty : value.Substring(0, value.Length - 1);
        }

        public BSizeT Size { get; private set; }

        public string Value { get; private set; }

        public override string DeRef()
            => this.Value;

        public override bool Equals(object obj)
            => obj is LString lstring && this.Value == lstring.Value;

        public override int GetHashCode()
            => throw new NotImplementedException();

        public override string ToString()
            => string.Format("\"{0}\"", this.Value);
    }
}
