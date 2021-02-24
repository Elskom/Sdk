// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Collections.Generic;
    using System.Linq;

    public class BList<T> : BObject
        where T : BObject
    {
        private readonly List<T> m_values;

        public BList(BInteger length, List<T> values)
        {
            this.Length = length;
            this.m_values = values;
        }

        public BInteger Length { get; private set; }

        public T this[int index] => this.m_values[index];

        public T[] AsArray()
            => this.m_values.AsParallel().ToArray();
    }
}
