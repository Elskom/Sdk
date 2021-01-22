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
    public class LBooleanType : BObjectType<LBoolean>
    {
        public override LBoolean Parse(Stream stream, BHeader header)
        {
            var value = stream.ReadByte();
            if ((value & 0xFFFFFFFE) != 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                var boolean = (value == 0) ? LBoolean.LFALSE : LBoolean.LTRUE;
                if (header.Debug)
                {
                    Console.WriteLine("-- parsed <boolean> " + boolean);
                }

                return boolean;
            }
        }
    }
}
