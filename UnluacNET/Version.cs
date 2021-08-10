// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class Version
{
    public static readonly Version LUA51 = new(0x51);
    public static readonly Version LUA52 = new(0x52);
    protected readonly int versionNumber;

    protected Version(int versionNumber)
    {
        this.versionNumber = versionNumber;
        this.HasHeaderTail = this.versionNumber is not 0x51;
        this.OuterBlockScopeAdjustment = this.versionNumber switch
        {
            0x51 => -1,
            0x52 => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(versionNumber)),
        };
        this.TForTarget = this.versionNumber switch
        {
            0x51 => Op.TFORLOOP,
            0x52 => Op.TFORCALL,
            _ => throw new ArgumentOutOfRangeException(nameof(versionNumber)),
        };
        this.UsesInlineUpvalueDeclaritions = this.versionNumber is 0x51;
        this.UsesOldLoadNilEncoding = this.versionNumber is 0x51;
    }

    public bool HasHeaderTail { get; }

    public int OuterBlockScopeAdjustment { get; }

    public Op TForTarget { get; }

    public bool UsesInlineUpvalueDeclaritions { get; }

    public bool UsesOldLoadNilEncoding { get; }

    public LFunctionType GetLFunctionType()
        => this.versionNumber is 0x51 ? LFunctionType.TYPE51 : LFunctionType.TYPE52;

    public bool IsBreakableLoopEnd(Op op)
        => this.versionNumber is 0x51
            ? op is Op.JMP or Op.FORLOOP
            : op is Op.JMP or Op.FORLOOP or Op.TFORLOOP;

    public OpcodeMap GetOpcodeMap()
        => new(this.versionNumber);
}
