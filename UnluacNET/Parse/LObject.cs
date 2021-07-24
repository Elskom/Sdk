// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;

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
