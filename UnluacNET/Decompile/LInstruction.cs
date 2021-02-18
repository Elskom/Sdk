// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public sealed class LInstruction
    {
        /*
        ** Size and position of opcode arguments
        */
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int SIZE_C = 9;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int SIZE_B = 9;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int SIZE_Bx = SIZE_C + SIZE_B;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int SIZE_A = 8;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int SIZE_OP = 6;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int POS_OP = 0;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int POS_A = POS_OP + SIZE_OP;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int POS_C = POS_A + SIZE_A;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int POS_B = POS_C + SIZE_C;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int POS_Bx = POS_C;

        /*
        ** Limits for opcode arguments
        ** (signed) int used to manipulate most arguments
        */
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int MAXARG_Bx = (1 << SIZE_Bx) - 1;
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private const int MAXARG_sBx = MAXARG_Bx >> 1;

        /*
         ** Macros to operate RK indices
         */
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_GETOPCODE = MASK1(SIZE_OP, 0);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_SETOPCODE = MASK1(SIZE_OP, POS_OP);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_GETA = MASK1(SIZE_A, 0);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_SETA = MASK1(SIZE_A, POS_A);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_GETB = MASK1(SIZE_B, 0);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_SETB = MASK1(SIZE_B, POS_B);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_GETC = MASK1(SIZE_C, 0);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_SETC = MASK1(SIZE_C, POS_C);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_GETBx = MASK1(SIZE_Bx, 0);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Don't care for now.")]
        private static readonly int MASK_SETBx = MASK1(SIZE_Bx, POS_Bx);
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private int m_value;

        public LInstruction(int value)
            => this.m_value = value;

        public LInstruction(Op op, int a, int bx)
            => this.m_value = ((int)op << POS_OP) | (a << POS_A) | (bx << POS_Bx);

        public LInstruction(Op op, int a, int b, int c)
            => this.m_value = ((int)op << POS_OP) | (a << POS_A) | (b << POS_B) | (c << POS_C);

        public Op Op
        {
            get => (Op)GetOpCode(this.m_value);
            set =>
                this.m_value = (this.m_value & ~MASK_SETOPCODE) |
                                (((int)value << POS_OP) & MASK_SETOPCODE);
        }

        public int A
        {
            get => GetArgA(this.m_value);
            set =>
                this.m_value = (this.m_value & ~MASK_SETA) |
                                ((value << POS_A) & MASK_SETA);
        }

        public int B
        {
            get => GetArgB(this.m_value);
            set =>
                this.m_value = (this.m_value & ~MASK_SETB) |
                                ((value << POS_B) & MASK_SETB);
        }

        public int C
        {
            get => GetArgC(this.m_value);
            set =>
                this.m_value = (this.m_value & ~MASK_SETC) |
                                ((value << POS_C) & MASK_SETC);
        }

        public int Bx
        {
            get => GetArgBx(this.m_value);
            set =>
                this.m_value = (this.m_value & ~MASK_SETBx) |
                                ((value << POS_Bx) & MASK_SETBx);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Don't care for now.")]
        public int SBx
        {
            get => GetArgsBX(this.m_value);
            set => this.Bx = value + MAXARG_sBx;
        }

        public static implicit operator LInstruction(int value)
            => new(value);

        public static implicit operator int(LInstruction value)
            => value.m_value;

        public static LInstruction CreateABC(Op op, int a, int b, int c)
            => new(op, a, b, c);

        public static LInstruction CreateABx(Op op, int a, int bx)
            => new(op, a, bx);

        public static int GetOpCode(int codePoint)
            => (codePoint >> POS_OP) & MASK_GETOPCODE;

        public static int GetArgA(int codePoint)
            => (codePoint >> POS_A) & MASK_GETA;

        public static int GetArgB(int codePoint)
            => (codePoint >> POS_B) & MASK_GETB;

        public static int GetArgC(int codePoint)
            => (codePoint >> POS_C) & MASK_GETC;

        public static int GetArgBx(int codePoint)
            => (codePoint >> POS_Bx) & MASK_GETBx;

        public static int GetArgsBX(int codePoint)
            => GetArgBx(codePoint) - MAXARG_sBx;

        private static int MASK1(int n, int p)
            => (~(~0 << n)) << p;
    }
}
