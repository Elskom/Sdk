// Copyright (c) 2018-2021, AraHaan.
// https://github.com/AraHaan/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace XmlAbstraction
{
    using System.Collections.Generic;

    internal class XmlElementData
    {
        internal string Name { get; set; } = string.Empty;

        internal List<XmlElementData> Subelements { get; set; }

        internal List<XmlAttributeData> Attributes { get; set; } = new List<XmlAttributeData>();

        internal string Value { get; set; } = string.Empty;
    }
}
