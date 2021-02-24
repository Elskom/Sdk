// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public class LFloatNumber : LNumber
    {
        public LFloatNumber(float number)
            => this.Number = number;

        public float Number { get; private set; }

        public override double Value => this.Number;

        public override bool Equals(object obj)
            => obj is LFloatNumber number ? this.Number == number.Number : base.Equals(obj);

        public override int GetHashCode()
            => throw new NotImplementedException();

        public override string ToString()
            => this.Number == (float)Math.Round(this.Number) ? ((int)this.Number).ToString() : this.Number.ToString();
    }
}
