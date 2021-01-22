// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public abstract class Version
    {
        public static readonly Version LUA51 = new Version51();
        public static readonly Version LUA52 = new Version52();
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Part of API.")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Part of API.")]
        protected int m_versionNumber;

        protected Version(int versionNumber)
            => this.m_versionNumber = versionNumber;

        public abstract bool HasHeaderTail { get; }

        public abstract int OuterBlockScopeAdjustment { get; }

        public abstract Op TForTarget { get; }

        public abstract bool UsesInlineUpvalueDeclaritions { get; }

        public abstract bool UsesOldLoadNilEncoding { get; }

        public abstract LFunctionType GetLFunctionType();

        public abstract bool IsBreakableLoopEnd(Op op);

        public OpcodeMap GetOpcodeMap()
            => new OpcodeMap(this.m_versionNumber);
    }
}
