// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.IO;
    using IO;
    
    public class LNumberType : BObjectType<LNumber>
    {
        public int Size { get; private set; }
        public bool Integral { get; private set; }

        public override LNumber Parse(Stream stream, BHeader header)
        {
            // HACK HACK HACK
            var bigEndian = header.BigEndian;
            LNumber value = null;
            if (this.Integral)
            {
                switch (this.Size)
                {
                case 4:
                    value = new LIntNumber(stream.ReadInt32(bigEndian));
                    break;
                case 8:
                    value = new LLongNumber(stream.ReadInt64(bigEndian));
                    break;
                }
            }
            else
            {
                switch (this.Size)
                {
                case 4:
                    value = new LFloatNumber(stream.ReadSingle(bigEndian));
                    break;
                case 8 :
                    value = new LDoubleNumber(stream.ReadDouble(bigEndian));
                    break;
                }
            }

            if (value == null)
                throw new InvalidOperationException("The input chunk has an unsupported Lua number format");

            if (header.Debug)
                Console.WriteLine("-- parsed <number> " + value);

            return value;
        }

        public LNumberType(int size, bool integral)
        {
            this.Size = size;
            this.Integral = integral;
            if (!(size == 4 || size == 8))
                throw new InvalidOperationException("The input chunk has an unsupported Lua number size: " + size);
        }
    }
}
