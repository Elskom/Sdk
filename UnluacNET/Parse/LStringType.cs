// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class LStringType : BObjectType<LString>
{
    public override LString Parse(Stream stream, BHeader header)
    {
        var sizeT = header.SizeT.Parse(stream, header);
        StringBuilder sb = new();
        sizeT.Iterate(() =>
        {
            sb.Append(stream.ReadChar());
        });
        var str = sb.ToString();
        if (header.Debug)
        {
            Debug.WriteLine($"-- parsed <string> \"{str}\"");
        }

        return new(sizeT, str);
    }
}
