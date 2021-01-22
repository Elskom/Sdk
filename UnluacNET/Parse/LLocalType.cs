// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class LLocalType : BObjectType<LLocal>
    {
        public override LLocal Parse(Stream stream, BHeader header)
        {
            var name = header.String.Parse(stream, header);
            var start = header.Integer.Parse(stream, header);
            var end = header.Integer.Parse(stream, header);
            if (header.Debug)
            {
                Console.WriteLine("-- parsing local, name: {0} from {1} to {2}", name, start.AsInteger(), end.AsInteger());
            }

            return new LLocal(name, start, end);
        }
    }
}
