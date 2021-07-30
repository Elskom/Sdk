// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class LFunctionType52 : LFunctionType
{
    protected override void ParseDebug(Stream stream, BHeader header, LFunctionParseState s)
    {
        s.Name = header.String.Parse(stream, header);
        base.ParseDebug(stream, header, s);
    }

    protected override void ParseMain(Stream stream, BHeader header, LFunctionParseState s)
    {
        s.LineBegin = header.Integer.Parse(stream, header).AsInteger();
        s.LineEnd = header.Integer.Parse(stream, header).AsInteger();
        s.LenParameter = stream.ReadByte();
        s.VarArg = stream.ReadByte();
        s.MaximumStackSize = stream.ReadByte();
        this.ParseCode(stream, header, s);
        this.ParseConstants(stream, header, s);
        this.ParseUpvalues(stream, header, s);
        this.ParseDebug(stream, header, s);
    }

    protected override void ParseUpvalues(Stream stream, BHeader header, LFunctionParseState s)
    {
        var upvalues = header.UpValue.ParseList(stream, header);
        s.LenUpvalues = upvalues.Length.AsInteger();
        s.Upvalues = upvalues.AsArray();
    }
}
