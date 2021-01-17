// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public abstract class LNumber : LObject
    {
        public static LNumber MakeInteger(int number) => new LIntNumber(number);

        public override bool Equals(object obj)
        {
            if (obj is LNumber)
                return this.Value == ((LNumber)obj).Value;

            return false;
        }

        // TODO: problem solution for this issue (???)
        public abstract double Value { get; }
    }

    public class LFloatNumber : LNumber
    {
        public float Number { get; private set; }

        public override double Value => this.Number;

        public override bool Equals(object obj)
        {
            if (obj is LFloatNumber)
                return this.Number == ((LFloatNumber)obj).Number;

            return base.Equals(obj);
        }

        public override string ToString()
        {
            if (this.Number == (float)Math.Round(this.Number))
                return ((int)this.Number).ToString();
            else
                return this.Number.ToString();
        }

        public LFloatNumber(float number)
            => this.Number = number;
    }

    public class LDoubleNumber : LNumber
    {
        public double Number { get; private set; }

        public override double Value => this.Number;

        public override bool Equals(object obj)
        {
            if (obj is LDoubleNumber)
                return this.Number == ((LDoubleNumber)obj).Number;

            return base.Equals(obj);
        }

        public override string ToString()
        {
            if (this.Number == Math.Round(this.Number))
                return ((long)this.Number).ToString();
            else
                return this.Number.ToString();
        }

        public LDoubleNumber(double number)
            => this.Number = number;
    }

    public class LIntNumber : LNumber
    {
        public int Number { get; private set; }

        public override double Value => this.Number;

        public override bool Equals(object obj)
        {
            if (obj is LIntNumber)
                return this.Number == ((LIntNumber)obj).Number;
            
            return base.Equals(obj);
        }

        public override string ToString()
            => this.Number.ToString();

        public LIntNumber(int number)
            => this.Number = number;
    }

    public class LLongNumber : LNumber
    {
        public long Number { get; private set; }

        public override double Value => this.Number;

        public override bool Equals(object obj)
        {
            if (obj is LLongNumber)
                return this.Number == ((LLongNumber)obj).Number;

            return base.Equals(obj);
        }

        public override string ToString()
            => this.Number.ToString();

        public LLongNumber(long number)
            => this.Number = number;
    }
}
