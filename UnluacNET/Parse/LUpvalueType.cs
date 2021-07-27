// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

using System.IO;

public class LUpvalueType : BObjectType<LUpvalue>
{
    public override LUpvalue Parse(Stream stream, BHeader header)
        => new()
        {
            InStack = stream.ReadByte() != 0,
            Index = stream.ReadByte(),
        };
}
