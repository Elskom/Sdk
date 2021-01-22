// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public abstract class LNumber : LObject
    {
        // TODO: problem solution for this issue (???)
        public abstract double Value { get; }

        public static LNumber MakeInteger(int number)
            => new LIntNumber(number);

        public override bool Equals(object obj)
            => obj is LNumber number && this.Value == number.Value;

        public override int GetHashCode()
            => throw new NotImplementedException();
    }
}
