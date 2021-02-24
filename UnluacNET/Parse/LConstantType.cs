// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.IO;

    public class LConstantType : BObjectType<LObject>
    {
        private static readonly string[] m_constantTypes =
        {
            "<nil>",
            "<boolean>",
            null, // no type?
            "<number>",
            "<string>",
        };

        public override LObject Parse(Stream stream, BHeader header)
        {
            var type = stream.ReadByte();
            if (header.Debug && type < m_constantTypes.Length)
            {
                var cType = m_constantTypes[type];
                Console.WriteLine("-- parsing <constant>, type is {0}", type != 2 ? cType : "illegal " + type);
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
