// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public abstract class Version
    {
        public static readonly Version LUA51 = new Version51();
        public static readonly Version LUA52 = new Version52();
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
            => new(this.m_versionNumber);
    }
}
