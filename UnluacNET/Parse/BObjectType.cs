// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public abstract class BObjectType<T> : BObject
        where T : BObject
    {
        public abstract T Parse(Stream stream, BHeader header);

        public BList<T> ParseList(Stream stream, BHeader header)
        {
            var length = header.Integer.Parse(stream, header);
            var values = new List<T>();
            length.Iterate(() =>
            {
                values.Add(this.Parse(stream, header));
            });
            return new BList<T>(length, values);
        }
    }
}
