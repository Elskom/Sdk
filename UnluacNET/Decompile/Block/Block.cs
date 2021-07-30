// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public abstract class Block : Statement, IComparable<Block>
{
    protected Block(LFunction function, int begin, int end)
    {
        this.Function = function;
        this.Begin = begin;
        this.End = end;
    }

    public int Begin { get; set; }

    public int End { get; set; }

    public bool LoopRedirectAdjustment { get; set; }

    public virtual int ScopeEnd => this.End - 1;

    public abstract bool Breakable { get; }

    public abstract bool IsContainer { get; }

    public abstract bool IsUnprotected { get; }

    protected LFunction Function { get; }

    public static bool operator ==(Block left, Block right)
        => left is null ? right is null : left.Equals(right);

    public static bool operator !=(Block left, Block right)
        => !(left == right);

    public static bool operator <(Block left, Block right)
        => left is null ? right is null : left.CompareTo(right) < 0;

    public static bool operator <=(Block left, Block right)
        => left is null || left.CompareTo(right) <= 0;

    public static bool operator >(Block left, Block right)
        => left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(Block left, Block right)
        => left is null ? right is null : left.CompareTo(right) >= 0;

    public abstract void AddStatement(Statement statement);

    public abstract int GetLoopback();

    public virtual int CompareTo(Block other)
        => this.Begin < other.Begin
        ? -1
        : this.Begin == other.Begin
        ? this.End < other.End
        ? 1
        : this.End == other.End
        ? this.IsContainer && !other.IsContainer
        ? -1
        : !this.IsContainer && other.IsContainer
        ? 1
        : 0
        : -1
        : 1;

    public virtual bool Contains(Block block)
        => this.Begin <= block.Begin && this.End >= block.End;

    public virtual bool Contains(int line)
        => this.Begin <= line && line < this.End;

    public virtual Operation Process(Decompiler d)
    {
        var statement = this;
        return new LambdaOperation(this.End - 1, (_, _) => statement);
    }

    public override bool Equals(object obj)
        => ReferenceEquals(this, obj) && obj is not null && this.CompareTo((Block)obj) is 0;

    public override int GetHashCode()
        => throw new NotImplementedException();
}
