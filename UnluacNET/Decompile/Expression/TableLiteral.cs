// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

using System;
using System.Collections.Generic;

public class TableLiteral : Expression
{
    private readonly List<Entry> m_entries;
    private readonly int m_capacity;
    private bool m_isObject = true;
    private bool m_isList = true;
    private int m_listLength = 1;

    public TableLiteral(int arraySize, int hashSize)
        : base(PRECEDENCE_ATOMIC)
    {
        this.m_capacity = arraySize + hashSize;
        this.m_entries = new(this.m_capacity);
    }

    public override int ConstantIndex
    {
        get
        {
            var index = -1;
            foreach (var entry in this.m_entries)
            {
                index = Math.Max(entry.Key.ConstantIndex, index);
                index = Math.Max(entry.Value.ConstantIndex, index);
            }

            return index;
        }
    }

    public override bool IsBrief => false;

    public override bool IsNewEntryAllowed => this.m_entries.Count < this.m_capacity;

    public override bool IsTableLiteral => true;

    public override void AddEntry(Entry entry)
    {
        this.m_entries.Add(entry);
        this.m_isObject = this.m_isObject && (entry.IsList || entry.Key.IsIdentifier);
        this.m_isList = this.m_isList && entry.IsList;
    }

    public override void Print(Output output)
    {
        this.m_entries.Sort();
        this.m_listLength = 1;
        if (this.m_entries.Count == 0)
        {
            output.Print("{}");
        }
        else
        {
            var lineBreak = (this.m_isList && this.m_entries.Count > 5) ||
                            (this.m_isObject && this.m_entries.Count > 2) ||
                            !this.m_isObject;
            if (!lineBreak)
            {
                foreach (var entry in this.m_entries)
                {
                    var value = entry.Value;
                    if (!value.IsBrief)
                    {
                        lineBreak = true;
                        break;
                    }
                }
            }

            output.Print("{");
            if (lineBreak)
            {
                output.PrintLine();
                output.IncreaseIndent();
            }

            this.PrintEntry(0, output);
            if (!this.m_entries[0].Value.IsMultiple)
            {
                for (var i = 1; i < this.m_entries.Count; i++)
                {
                    output.Print(",");
                    if (lineBreak)
                    {
                        output.PrintLine();
                    }
                    else
                    {
                        output.Print(" ");
                    }

                    this.PrintEntry(i, output);
                    if (this.m_entries[i].Value.IsMultiple)
                    {
                        break;
                    }
                }
            }

            if (lineBreak)
            {
                output.PrintLine();
                output.DecreaseIndent();
            }

            output.Print("}");
        }
    }

    private void PrintEntry(int index, Output output)
    {
        var entry = this.m_entries[index];
        var key = entry.Key;
        var value = entry.Value;
        var isList = entry.IsList;
        var multiple = index + 1 >= this.m_entries.Count || value.IsMultiple;
        if (isList && key.IsInteger && this.m_listLength == key.AsInteger())
        {
            if (multiple)
            {
                value.PrintMultiple(output);
            }
            else
            {
                value.Print(output);
            }

            this.m_listLength++;
        }
        else if (this.m_isObject && key.IsIdentifier)
        {
            output.Print(key.AsName());
            output.Print(" = ");
            value.Print(output);
        }
        else
        {
            output.Print("[");
            key.Print(output);
            output.Print("] = ");
            value.Print(output);
        }
    }

    public sealed class Entry : IComparable<Entry>
    {
        public Entry(Expression key, Expression value, bool isList, int timestamp)
        {
            this.Key = key;
            this.Value = value;
            this.IsList = isList;
            this.Timestamp = timestamp;
        }

        public Expression Key { get; private set; }

        public Expression Value { get; private set; }

        public bool IsList { get; private set; }

        public int Timestamp { get; private set; }

        public static bool operator ==(Entry left, Entry right)
            => left is null ? right is null : left.Equals(right);

        public static bool operator !=(Entry left, Entry right)
            => !(left == right);

        public static bool operator <(Entry left, Entry right)
            => left is null ? right is null : left.CompareTo(right) < 0;

        public static bool operator <=(Entry left, Entry right)
            => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(Entry left, Entry right)
            => left is not null && left.CompareTo(right) > 0;

        public static bool operator >=(Entry left, Entry right)
            => left is null ? right is null : left.CompareTo(right) >= 0;

        public int CompareTo(Entry other)
            => this.Timestamp.CompareTo(other.Timestamp);

        public override bool Equals(object obj)
            => ReferenceEquals(this, obj) && obj is not null && this.CompareTo((Entry)obj) == 0;

        public override int GetHashCode()
            => throw new NotImplementedException();
    }
}
