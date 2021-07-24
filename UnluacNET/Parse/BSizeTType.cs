// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics;
    using System.IO;

    public class BSizeTType : BObjectType<BSizeT>
    {
        private readonly BIntegerType integerType;

        public BSizeTType(int sizeTSize)
        {
            this.SizeTSize = sizeTSize;
            this.integerType = new(sizeTSize);
        }

        public int SizeTSize { get; }

        public override BSizeT Parse(Stream stream, BHeader header)
        {
            BSizeT value = new(this.integerType.RawParse(stream, header));
            if (header.Debug)
            {
                Debug.WriteLine($"-- parsed <size_t> {value.AsInteger()}");
            }

            return value;
        }
    }
}
