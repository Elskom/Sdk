// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class LUpvalueType : BObjectType<LUpvalue>
    {
        public override LUpvalue Parse(Stream stream, BHeader header)
            => new()
            {
                InStack = stream.ReadByte() != 0,
                Index = stream.ReadByte(),
            };
    }
}
