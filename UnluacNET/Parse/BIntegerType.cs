// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.IO;
    using IO;
    
    public class BIntegerType : BObjectType<BInteger>
    {
        public int IntSize { get; private set; }

        public override BInteger Parse(Stream stream, BHeader header)
        {
            var value = this.RawParse(stream, header);
            if (header.Debug)
            {
                Console.WriteLine("-- parsed <integer> " + value.AsInteger());
            }

            return value;
        }

        protected internal BInteger RawParse(Stream stream, BHeader header)
        {
            // HACK HACK HACK
            var bigEndian = header.BigEndian;
            BInteger value = null;
            switch (this.IntSize)
            {
            case 0:
                value = new BInteger(0);
                break;
            case 1:
                value = new BInteger(stream.ReadByte());
                break;
            case 2:
                value = new BInteger(stream.ReadInt16(bigEndian));
                break;
            case 4:
                value = new BInteger(stream.ReadInt32(bigEndian));
                break;
            case 8:
                value = new BInteger(stream.ReadInt64(bigEndian));
                break;
            default:
                throw new InvalidOperationException("Bad IntSize, cannot parse data");
            //default:
            //    {
            //        var bytes = new byte[IntSize];
            //
            //        var start = 0;
            //        var delta = 1;
            //
            //        for (int i = start; i >= 0 && i < IntSize; i += delta)
            //            bytes[i] = (byte)stream.ReadByte();
            //
            //        value = new BInteger(BitConverter.ToInt64(bytes, 0));
            //    } break;
            }

            return value;
        }

        public BIntegerType(int intSize)
            => this.IntSize = intSize;
    }
}
