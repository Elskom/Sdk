// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class BInteger : BObject
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly long MAX_INT = int.MaxValue;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly long MIN_INT = int.MinValue;

        // TODO: Why not just use a 'long' to hold both sizes? Doesn't make much of a difference IMO
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly long m_big;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
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

        [SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "🖕")]
        public int AsInteger()
            => this.m_big == 0
            ? this.m_number
            : this.m_big.CompareTo(MAX_INT) > 0 || this.m_big.CompareTo(MIN_INT) < 0
            ? throw new InvalidOperationException("The size of an integer is outside the range that unluac can handle.")
            : (int)this.m_big;

        public void Iterate(Action thunk)
        {
            // so what even is the difference between these two? they look exactly the same..
            if (this.m_big == 0)
            {
                var i = this.m_number;
                while (i-- != 0)
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
