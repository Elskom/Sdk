// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public enum Op
    {
        /*------------------------------------------------------------------------
        name           args    description
        --------------------------------------------------------------------------*/
        /*   A B     R(A) := R(B)                                       */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        MOVE,

        /*   A Bx    R(A) := Kst(Bx)                                    */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        LOADK,

        /*   A B C   R(A) := (Bool)B; if (C) pc++                       */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        LOADBOOL,

        /*   A B     R(A) := ... := R(B) := nil                         */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        LOADNIL,

        /*   A B     R(A) := UpValue[B]                                 */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        GETUPVAL,

        /*   A Bx    R(A) := Gbl[Kst(Bx)]                               */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        GETGLOBAL,

        /*   A B C   R(A) := R(B)[RK(C)]                                */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        GETTABLE,

        /*   A Bx    Gbl[Kst(Bx)] := R(A)                               */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        SETGLOBAL,

        /*   A B     UpValue[B] := R(A)                                 */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        SETUPVAL,

        /*   A B C   R(A)[RK(B)] := RK(C)                               */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        SETTABLE,

        /*   A B C   R(A) := {} (size = B,C)                            */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        NEWTABLE,

        /*   A B C   R(A+1) := R(B); R(A) := R(B)[RK(C)]                */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        SELF,

        /*   A B C   R(A) := RK(B) + RK(C)                              */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        ADD,

        /*   A B C   R(A) := RK(B) - RK(C)                              */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        SUB,

        /*   A B C   R(A) := RK(B) * RK(C)                              */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        MUL,

        /*   A B C   R(A) := RK(B) / RK(C)                              */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        DIV,

        /*   A B C   R(A) := RK(B) % RK(C)                              */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        MOD,

        /*   A B C   R(A) := RK(B) ^ RK(C)                              */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        POW,

        /*   A B     R(A) := -R(B)                                      */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        UNM,

        /*   A B     R(A) := not R(B)                                   */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        NOT,

        /*   A B     R(A) := length of R(B)                             */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        LEN,

        /*   A B C   R(A) := R(B).. ... ..R(C)                          */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        CONCAT,

        /*   sBx     pc+=sBx (different in 5.2)                         */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        JMP,

        /*   A B C   if ((RK(B) == RK(C)) ~= A) then pc++               */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        EQ,

        /*   A B C   if ((RK(B) <  RK(C)) ~= A) then pc++               */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        LT,

        /*   A B C   if ((RK(B) <= RK(C)) ~= A) then pc++               */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        LE,

        /*   A C     if not (R(A) <=> C) then pc++                      */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        TEST,

        /*   A B C   if (R(B) <=> C) then R(A) := R(B) else pc++        */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        TESTSET,

        /*   A B C   R(A), ... ,R(A+C-2) := R(A)(R(A+1), ... ,R(A+B-1)) */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        CALL,

        /*   A B C   return R(A)(R(A+1), ... ,R(A+B-1))                 */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        TAILCALL,

        /*   A B     return R(A), ... ,R(A+B-2)      (see note)         */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        RETURN,

        /*   A sBx   R(A)+=R(A+2);
             if R(A) <?= R(A+1) then { pc+=sBx; R(A+3)=R(A) }           */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "Not Code.")]
        FORLOOP,

        /*   A sBx   R(A)-=R(A+2); pc+=sBx                              */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        FORPREP,

        /*   A C     R(A+3), ... ,R(A+3+C) := R(A)(R(A+1), R(A+2));
                if R(A+3) ~= nil then { pc++; R(A+2)=R(A+3); }          */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "Not Code.")]
        TFORLOOP,

        /*   A B C   R(A)[(C-1)*FPF+i] := R(A+i), 1 <= i <= B           */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        SETLIST,

        /*   A       close all variables in the stack up to (>=) R(A)   */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        CLOSE,

        /*   A Bx    R(A) := closure(KPROTO[Bx], R(A), ... ,R(A+n))     */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        CLOSURE,

        /*   A B     R(A), R(A+1), ..., R(A+B-1) = vararg               */
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        VARARG,

        // Lua 5.2 Opcodes
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        LOADKX,
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        GETTABUP,
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        SETTABUP,
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        TFORCALL,
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "No docs yet.")]
        EXTRAARG,
    }

#pragma warning disable S125 // Sections of code should not be commented out
    /*===========================================================================
          Notes:
          (*) In OP_CALL, if (B == 0) then B = top. C is the number of returns - 1,
              and can be 0: OP_CALL then sets `top' to last_result+1, so
              next open instruction (OP_CALL, OP_RETURN, OP_SETLIST) may use `top'.

          (*) In OP_VARARG, if (B == 0) then use actual number of varargs and
              set top (like in OP_CALL with C == 0).

          (*) In OP_RETURN, if (B == 0) then return up to `top'

          (*) In OP_SETLIST, if (B == 0) then B = `top';
              if (C == 0) then next `instruction' is real C

          (*) For comparisons, A specifies what condition the test should accept
              (true or false).

          (*) All `skips' (pc++) assume that next instruction is a jump
        ===========================================================================*/
#pragma warning restore S125 // Sections of code should not be commented out
}
