// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.IO;
    using System.Text;
    using IO;

    public class LStringType : BObjectType<LString>
    {
        public override LString Parse(Stream stream, BHeader header)
        {
            var sizeT = header.SizeT.Parse(stream, header);
            var sb = new StringBuilder();
            sizeT.Iterate(() =>
            {
                sb.Append(stream.ReadChar());
            });
            var str = sb.ToString();
            if (header.Debug)
                Console.WriteLine("-- parsed <string> \"" + str + "\"");

            return new LString(sizeT, str);
        }
    }
}
