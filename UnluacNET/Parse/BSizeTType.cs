// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.IO;

    public class BSizeTType : BObjectType<BSizeT>
    {
        private BIntegerType m_integerType;

        public int SizeTSize { get; private set; }

        public override BSizeT Parse(Stream stream, BHeader header)
        {
            var value = new BSizeT(this.m_integerType.RawParse(stream, header));

            if (header.Debug)
                Console.WriteLine("-- parsed <size_t> " + value.AsInteger());

            return value;
        }

        public BSizeTType(int sizeTSize)
        {
            this.SizeTSize = sizeTSize;
            this.m_integerType = new BIntegerType(sizeTSize);
        }
    }
}
