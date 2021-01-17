// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;

    public abstract class Block : Statement, IComparable<Block>
    {
        protected LFunction Function { get; private set; }

        public int Begin { get; set; }
        public int End { get; set; }

        public bool LoopRedirectAdjustment { get; set; }

        public virtual int ScopeEnd => this.End - 1;

        public abstract bool Breakable { get; }
        public abstract bool IsContainer { get; }
        public abstract bool IsUnprotected { get; }
        
        public abstract void AddStatement(Statement statement);
        public abstract int GetLoopback();

        public virtual int CompareTo(Block block)
        {
            if (this.Begin < block.Begin)
            {
                return -1;
            }
            else if (this.Begin == block.Begin)
            {
                if (this.End < block.End)
                {
                    return 1;
                }
                else if (this.End == block.End)
                {
                    if (this.IsContainer && !block.IsContainer)
                    {
                        return -1;
                    }
                    else if (!this.IsContainer && block.IsContainer)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 1;
            }
        }

        public virtual bool Contains(Block block)
            => (this.Begin <= block.Begin) && (this.End >= block.End);

        public virtual bool Contains(int line)
            => (this.Begin <= line) && (line < this.End);

        public virtual Operation Process(Decompiler d)
        {
            var statement = this;
            return new LambdaOperation(this.End - 1, (r, block) => {
                return statement;
            });
        }

        public Block(LFunction function, int begin, int end)
        {
            this.Function = function;
            this.Begin = begin;
            this.End = end;
        }
    }
}
