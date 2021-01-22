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
    public class BSizeTType : BObjectType<BSizeT>
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly BIntegerType m_integerType;

        public BSizeTType(int sizeTSize)
        {
            this.SizeTSize = sizeTSize;
            this.m_integerType = new BIntegerType(sizeTSize);
        }

        public int SizeTSize { get; private set; }

        public override BSizeT Parse(Stream stream, BHeader header)
        {
            var value = new BSizeT(this.m_integerType.RawParse(stream, header));
            if (header.Debug)
            {
                Console.WriteLine("-- parsed <size_t> " + value.AsInteger());
            }

            return value;
        }
    }
}
