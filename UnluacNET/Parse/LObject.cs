// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    [SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "Stupid Analyzer bug.")]
    public abstract class LObject : BObject, IEqualityComparer<LObject>
    {
        public abstract new bool Equals(object obj);

        public virtual string DeRef()
            => throw new NotImplementedException();

        public bool Equals(LObject x, LObject y)
            => x.Equals(y);

        public int GetHashCode(LObject obj)
            => throw new NotImplementedException();
    }
}
