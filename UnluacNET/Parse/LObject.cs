// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public abstract class LObject : BObject
    {
        public abstract new bool Equals(object obj);

        public virtual string DeRef()
            => throw new NotImplementedException();
    }
}
