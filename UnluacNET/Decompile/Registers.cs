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
    public class Registers
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Declaration[,] m_decls;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Function m_func;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly int[,] m_updated;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Expression[,] m_values;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private bool[] m_startedLines;

        public Registers(int registers, int length, Declaration[] declList, Function func)
        {
            this.NumRegisters = registers;
            this.Length = length;
            this.m_decls = new Declaration[registers, length + 1];
            for (var i = 0; i < declList.Length; i++)
            {
                var decl = declList[i];
                var register = 0;
                while (this.m_decls[register, decl.Begin] != null)
                {
                    register++;
                }

                decl.Register = register;
                for (var line = decl.Begin; line <= decl.End; line++)
                {
                    this.m_decls[register, line] = decl;
                }
            }

            this.m_values = new Expression[registers, length + 1];
            for (var register = 0; register < registers; register++)
            {
                this.m_values[register, 0] = Expression.NIL;
            }

            this.m_updated = new int[registers, length + 1];
            this.m_startedLines = new bool[length + 1];
            this.m_func = func;
        }

        public int NumRegisters { get; private set; }

        public int Length { get; private set; }

        public bool IsAssignable(int register, int line)
            => this.IsLocal(register, line) &&
               !this.GetDeclaration(register, line).ForLoop;

        public bool IsLocal(int register, int line)
        {
            if (register < 0)
            {
                return false;
            }

            return this.GetDeclaration(register, line) != null;
        }

        public bool IsNewLocal(int register, int line)
        {
            var decl = this.GetDeclaration(register, line);
            return decl != null && decl.Begin == line && !decl.ForLoop;
        }

        public Declaration GetDeclaration(int register, int line)
            => this.m_decls[register, line];

        public Expression GetExpression(int register, int line)
        {
            if (this.IsLocal(register, line - 1))
            {
                return new LocalVariable(this.GetDeclaration(register, line - 1));
            }
            else
            {
                return this.GetValue(register, line);
            }
        }

        public Expression GetKExpression(int register, int line)
        {
            if ((register & 0x100) != 0)
            {
                return this.m_func.GetConstantExpression(register & 0xFF);
            }
            else
            {
                return this.GetExpression(register, line);
            }
        }

        public List<Declaration> GetNewLocals(int line)
        {
            var locals = new List<Declaration>(this.NumRegisters);
            for (var register = 0; register < this.NumRegisters; register++)
            {
                if (this.IsNewLocal(register, line))
                {
                    locals.Add(this.GetDeclaration(register, line));
                }
            }

            return locals;
        }

        public Target GetTarget(int register, int line)
        {
            if (!this.IsLocal(register, line))
            {
                throw new InvalidOperationException("No declaration exists in register" + register + " at line " + line);
            }

            return new VariableTarget(this.GetDeclaration(register, line));
        }

        public int GetUpdated(int register, int line)
            => this.m_updated[register, line];

        public Expression GetValue(int register, int line)
            => this.m_values[register, line - 1];

        public void SetValue(int register, int line, Expression expression)
        {
            this.m_values[register, line] = expression;
            this.m_updated[register, line] = line;
        }

        public void SetInternalLoopVariable(int register, int begin, int end)
        {
            var decl = this.GetDeclaration(register, begin);
            if (decl == null)
            {
                decl = new Declaration("_FOR_", begin, end);
                decl.Register = register;
                this.NewDeclaration(decl, register, begin, end);
            }

            decl.ForLoop = true;
        }

        public void SetExplicitLoopVariable(int register, int begin, int end)
        {
            var decl = this.GetDeclaration(register, begin);
            if (decl == null)
            {
                decl = new Declaration("_FORV_" + register + "_", begin, end);
                decl.Register = register;
                this.NewDeclaration(decl, register, begin, end);
            }

            decl.ForLoopExplicit = true;
        }

        public void StartLine(int line)
        {
            this.m_startedLines[line] = true;
            for (var register = 0; register < this.NumRegisters; register++)
            {
                this.m_values[register, line] = this.m_values[register, line - 1];
                this.m_updated[register, line] = this.m_updated[register, line - 1];
            }
        }

        private void NewDeclaration(Declaration decl, int register, int begin, int end)
        {
            for (var line = begin; line <= end; line++)
            {
                this.m_decls[register, line] = decl;
            }
        }
    }
}
