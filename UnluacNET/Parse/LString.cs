// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class LString : LObject
{
    public LString(BSizeT size, string value)
    {
        this.Size = size;
        this.Value = value.Length is 0 ? string.Empty : value.AsSpan().Slice(0, value.Length - 1).ToString();
    }

    public BSizeT Size { get; }

    public string Value { get; }

    public override string DeRef()
        => this.Value;

    public override bool Equals(object obj)
        => obj is LString lstring && this.Value == lstring.Value;

    public override int GetHashCode()
        => throw new NotImplementedException();

    public override string ToString()
        => $"\"{this.Value}\"";
}
