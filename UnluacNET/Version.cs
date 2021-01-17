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

        public abstract bool HasHeaderTail { get; }

        public abstract int OuterBlockScopeAdjustment { get; }
        
        public abstract Op TForTarget { get; }
        
        public abstract bool UsesInlineUpvalueDeclaritions { get; }
        public abstract bool UsesOldLoadNilEncoding { get; }

        public abstract LFunctionType GetLFunctionType();
        public abstract bool IsBreakableLoopEnd(Op op);

        public OpcodeMap GetOpcodeMap()
            => new OpcodeMap(this.m_versionNumber);

        protected Version(int versionNumber)
            => this.m_versionNumber = versionNumber;
    }

    public sealed class Version51 : Version
    {
        public override bool HasHeaderTail => false;

        public override int OuterBlockScopeAdjustment => -1;

        public override Op TForTarget => Op.TFORLOOP;

        public override bool UsesInlineUpvalueDeclaritions => true;

        public override bool UsesOldLoadNilEncoding => true;

        public override LFunctionType GetLFunctionType()
            => LFunctionType.TYPE51;

        public override bool IsBreakableLoopEnd(Op op)
            => (op == Op.JMP || op == Op.FORLOOP);

        public Version51()
            : base(0x51) { }
    }

    public sealed class Version52 : Version
    {
        public override bool HasHeaderTail => true;

        public override int OuterBlockScopeAdjustment => 0;

        public override Op TForTarget => Op.TFORCALL;

        public override bool UsesInlineUpvalueDeclaritions => false;

        public override bool UsesOldLoadNilEncoding => false;

        public override LFunctionType GetLFunctionType()
            => LFunctionType.TYPE52;

        public override bool IsBreakableLoopEnd(Op op)
            => (op == Op.JMP || op == Op.FORLOOP || op == Op.TFORLOOP);

        public Version52()
            : base(0x52) { }
    }
}
