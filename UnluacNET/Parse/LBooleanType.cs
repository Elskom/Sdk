// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

using System;
using System.Diagnostics;
using System.IO;

public class LBooleanType : BObjectType<LBoolean>
{
    public override LBoolean Parse(Stream stream, BHeader header)
    {
        var value = stream.ReadByte();
        if ((value & 0xFFFFFFFE) is not 0)
        {
            throw new InvalidOperationException();
        }

        var boolean = value is 0 ? LBoolean.LFALSE : LBoolean.LTRUE;
        if (header.Debug)
        {
            Debug.WriteLine($"-- parsed <boolean> {boolean}");
        }

        return boolean;
    }
}
