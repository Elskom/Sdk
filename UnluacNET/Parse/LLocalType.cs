// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

using System.Diagnostics;
using System.IO;

public class LLocalType : BObjectType<LLocal>
{
    public override LLocal Parse(Stream stream, BHeader header)
    {
        var name = header.String.Parse(stream, header);
        var start = header.Integer.Parse(stream, header);
        var end = header.Integer.Parse(stream, header);
        if (header.Debug)
        {
            Debug.WriteLine($"-- parsing local, name: {name} from {start.AsInteger()} to {end.AsInteger()}");
        }

        return new(name, start, end);
    }
}
