// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public class BInteger : BObject
    {
        private static readonly long MAX_INT = int.MaxValue;
        private static readonly long MIN_INT = int.MinValue;

        // TODO: Why not just use a 'long' to hold both sizes? Doesn't make much of a difference IMO
        private readonly long m_big;
        private readonly int m_number;

        public BInteger(BInteger b)
        {
            this.m_big = b.m_big;
            this.m_number = b.m_number;
        }

        public BInteger(int number)
        {
            this.m_big = 0L;
            this.m_number = number;
        }

        public BInteger(long big)
        {
            this.m_big = big;
            this.m_number = 0;
        }

        public int AsInteger()
            => this.m_big is 0
            ? this.m_number
            : this.m_big.CompareTo(MAX_INT) > 0 || this.m_big.CompareTo(MIN_INT) < 0
            ? throw new InvalidOperationException("The size of an integer is outside the range that unluac can handle.")
            : (int)this.m_big;

        public void Iterate(Action thunk)
        {
            // so what even is the difference between these two? they look exactly the same..
            if (this.m_big is 0)
            {
                var i = this.m_number;
                while (i-- is not 0)
                {
                    thunk.Invoke();
                }
            }
            else
            {
                var i = this.m_big;
                while (i > 0)
                {
                    thunk.Invoke();
                    i -= 1L;
                }
            }
        }
    }
}
