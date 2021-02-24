// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public sealed class Version51 : Version
    {
        public Version51()
            : base(0x51)
        {
        }

        public override bool HasHeaderTail => false;

        public override int OuterBlockScopeAdjustment => -1;

        public override Op TForTarget => Op.TFORLOOP;

        public override bool UsesInlineUpvalueDeclaritions => true;

        public override bool UsesOldLoadNilEncoding => true;

        public override LFunctionType GetLFunctionType()
            => LFunctionType.TYPE51;

        public override bool IsBreakableLoopEnd(Op op)
            => op == Op.JMP || op == Op.FORLOOP;
    }
}
