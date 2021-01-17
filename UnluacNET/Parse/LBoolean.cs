// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class LBoolean : LObject
    {
        public static readonly LBoolean LTRUE = new LBoolean(true);
        public static readonly LBoolean LFALSE = new LBoolean(false);

        private readonly bool m_value;

        public override bool Equals(object obj)
            => (this == obj);

        public override string ToString()
            => this.m_value.ToString();

        private LBoolean(bool value)
            => this.m_value = value;
    }
}
