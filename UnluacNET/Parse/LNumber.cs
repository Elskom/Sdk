// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public abstract class LNumber : LObject
    {
        // TODO: problem solution for this issue (???)
        public abstract double Value { get; }

        public static LNumber MakeInteger(int number)
            => new LIntNumber(number);

        public override bool Equals(object obj)
            => obj is LNumber number && this.Value.Equals(number.Value);

        public override int GetHashCode()
            => throw new NotImplementedException();
    }
}
