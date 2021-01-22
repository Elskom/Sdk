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
    public class LConstantType : BObjectType<LObject>
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1311:Static readonly fields should begin with upper-case letter", Justification = "Don't care for now.")]
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

            switch (type)
            {
                case 0:
                    return LNil.NIL;
                case 1:
                    return header.Bool.Parse(stream, header);
                case 3:
                    return header.Number.Parse(stream, header);
                case 4:
                    return header.String.Parse(stream, header);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
