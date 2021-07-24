// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class LConstantType : BObjectType<LObject>
    {
        public override LObject Parse(Stream stream, BHeader header)
        {
            var type = stream.ReadByte();
            if (header.Debug && type < 5)
            {
                var cType = type switch
                {
                    0 => "<nil>",
                    1 => "<boolean>",
                    2 => null, // no type?
                    3 => "<number>",
                    4 => "<string>",
                    _ => throw new InvalidOperationException(),
                };
                Debug.WriteLine($"-- parsing <constant>, type is {(type is not 2 ? cType : $"illegal {type}")}");
            }

            return type switch
            {
                0 => LNil.NIL,
                1 => header.Bool.Parse(stream, header),
                3 => header.Number.Parse(stream, header),
                4 => header.String.Parse(stream, header),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
