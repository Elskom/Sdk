// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Elskom.Generic.Libs.UnluacNET.IO;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
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
            {
                Console.WriteLine("-- parsed <string> \"" + str + "\"");
            }

            return new LString(sizeT, str);
        }
    }
}
