// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class LString : LObject
    {
        public BSizeT Size { get; private set; }
        public string Value { get; private set; }

        public override string DeRef()
            => this.Value;

        public override bool Equals(object obj)
        {
            if (obj is LString)
                return this.Value == ((LString)obj).Value;

            return false;
        }

        public override string ToString()
            => string.Format("\"{0}\"", this.Value);

        public LString(BSizeT size, string value)
        {
            this.Size = size;
            this.Value = (value.Length == 0) ? string.Empty : value.Substring(0, value.Length - 1);
        }
    }
}
