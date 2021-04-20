// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Elskom.Generic.Libs.UnluacNET.IO;

    public class LNumberType : BObjectType<LNumber>
    {
        public LNumberType(int size, bool integral)
        {
            this.Size = size;
            this.Integral = integral;
            if (size is not (4 or 8))
            {
                throw new InvalidOperationException($"The input chunk has an unsupported Lua number size: {size}");
            }
        }

        public int Size { get; }

        public bool Integral { get; }

        public override LNumber Parse(Stream stream, BHeader header)
        {
            // HACK HACK HACK
            var bigEndian = header.BigEndian;
            LNumber value = this.Size switch
            {
                4 => this.Integral
                    ? new LIntNumber(stream.ReadInt32(bigEndian))
                    : new LFloatNumber(stream.ReadSingle(bigEndian)),
                8 => this.Integral
                    ? new LLongNumber(stream.ReadInt64(bigEndian))
                    : new LDoubleNumber(stream.ReadDouble(bigEndian)),
                _ => null,
            };

            if (value is null)
            {
                throw new InvalidOperationException("The input chunk has an unsupported Lua number format");
            }

            if (header.Debug)
            {
                Debug.WriteLine($"-- parsed <number> {value}");
            }

            return value;
        }
    }
}
