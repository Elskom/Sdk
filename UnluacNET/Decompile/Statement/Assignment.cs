// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Collections.Generic;

    public class Assignment : Statement
    {
        private readonly List<Target> m_targets = new(5);
        private readonly List<Expression> m_values = new(5);
        private bool m_allNil = true;
        private bool m_declare;
        private int m_declareStart;

        public Assignment()
        {
        }

        public Assignment(Target target, Expression value)
        {
            this.m_targets.Add(target);
            this.m_values.Add(value);
            this.m_allNil = this.m_allNil && value.IsNil;
        }

        public void AddFirst(Target target, Expression value)
        {
            this.m_targets.Insert(0, target);
            this.m_values.Insert(0, value);
            this.m_allNil = this.m_allNil && value.IsNil;
        }

        public void AddLast(Target target, Expression value)
        {
            if (this.m_targets.Contains(target))
            {
                var index = this.m_targets.IndexOf(target);
                value = this.m_values[index];
                this.m_targets.RemoveAt(index);
                this.m_values.RemoveAt(index);
            }

            this.m_targets.Add(target);
            this.m_values.Add(value);
            this.m_allNil = this.m_allNil && value.IsNil;
        }

        public bool AssignListEquals(List<Declaration> decls)
        {
            if (decls.Count != this.m_targets.Count)
            {
                return false;
            }

            foreach (var target in this.m_targets)
            {
                var found = false;
                foreach (var decl in decls)
                {
                    if (target.IsDeclaration(decl))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        public bool AssignsTarget(Declaration decl)
        {
            foreach (var target in this.m_targets)
            {
                if (target.IsDeclaration(decl))
                {
                    return true;
                }
            }

            return false;
        }

        public void Declare(int declareStart)
        {
            this.m_declare = true;
            this.m_declareStart = declareStart;
        }

        public int GetArity()
            => this.m_targets.Count;

        public Target GetFirstTarget()
            => this.m_targets[0];

        public Expression GetFirstValue()
            => this.m_values[0];

        public override void Print(Output output)
        {
            if (this.m_targets.Count > 0)
            {
                if (this.m_declare)
                {
                    output.Print("local ");
                }

                var functionSugar = false;
                var value = this.m_values[0];
                var target = this.m_targets[0];
                if (this.m_targets.Count is 1 && value.IsClosure && target.IsFunctionName)
                {
                    // This check only works in Lua version 0x51
                    if (!this.m_declare || this.m_declareStart >= value.ClosureUpvalueLine)
                    {
                        functionSugar = true;
                    }

                    if (target.IsLocal && value.IsUpvalueOf(target.GetIndex()))
                    {
                        functionSugar = true;
                    }
                }

                if (!functionSugar)
                {
                    target.Print(output);
                    for (var i = 1; i < this.m_targets.Count; i++)
                    {
                        output.Print(", ");
                        this.m_targets[i].Print(output);
                    }

                    if (!this.m_declare || !this.m_allNil)
                    {
                        output.Print(" = ");
                        Expression.PrintSequence(output, this.m_values, false, false);
                    }
                }
                else
                {
                    value.PrintClosure(output, target);
                }

                if (this.Comment is not null)
                {
                    output.Print(" -- ");
                    output.Print(this.Comment);
                }
            }
        }
    }
}
