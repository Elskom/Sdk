// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Elskom.Generic.Libs.UnluacNET.IO;

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
}
