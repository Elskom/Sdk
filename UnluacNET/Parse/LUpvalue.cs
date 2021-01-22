// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class LUpvalue : BObject
    {
        public int Index { get; set; }

        public bool InStack { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is LUpvalue upVal)
            {
                if (!(this.InStack == upVal.InStack && this.Index == upVal.Index))
                {
                    return false;
                }
                else if (this.Name == upVal.Name)
                {
                    return true;
                }

                return this.Name != null && this.Name == upVal.Name;
            }

            return false;
        }

        public override int GetHashCode()
            => throw new NotImplementedException();
    }
}
