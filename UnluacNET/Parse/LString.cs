using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnluacNET
{
    public class LString : LObject
    {
        public BSizeT Size { get; private set; }
        public string Value { get; private set; }

        public override string DeRef()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is LString)
                return Value == ((LString)obj).Value;

            return false;
        }

        public override string ToString()
        {
            return string.Format("\"{0}\"", Value);
        }

        public LString(BSizeT size, string value)
        {
            Size = size;
            Value = (value.Length == 0) ? string.Empty : value.Substring(0, value.Length - 1);
        }
    }
}
