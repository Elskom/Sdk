// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class LBoolean : LObject
    {
        public static readonly LBoolean LTRUE = new LBoolean(true);
        public static readonly LBoolean LFALSE = new LBoolean(false);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly bool m_value;

        private LBoolean(bool value)
            => this.m_value = value;

        public override bool Equals(object obj)
            => this == obj;

        public override int GetHashCode()
            => throw new NotImplementedException();

        public override string ToString()
            => this.m_value.ToString();
    }
}
