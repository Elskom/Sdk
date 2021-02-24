// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public enum Op
    {
        /*------------------------------------------------------------------------
        name           args    description
        --------------------------------------------------------------------------*/
        /*   A B     R(A) := R(B)                                       */
        MOVE,

        /*   A Bx    R(A) := Kst(Bx)                                    */
        LOADK,

        /*   A B C   R(A) := (Bool)B; if (C) pc++                       */
        LOADBOOL,

        /*   A B     R(A) := ... := R(B) := nil                         */
        LOADNIL,

        /*   A B     R(A) := UpValue[B]                                 */
        GETUPVAL,

        /*   A Bx    R(A) := Gbl[Kst(Bx)]                               */
        GETGLOBAL,

        /*   A B C   R(A) := R(B)[RK(C)]                                */
        GETTABLE,

        /*   A Bx    Gbl[Kst(Bx)] := R(A)                               */
        SETGLOBAL,

        /*   A B     UpValue[B] := R(A)                                 */
        SETUPVAL,

        /*   A B C   R(A)[RK(B)] := RK(C)                               */
        SETTABLE,

        /*   A B C   R(A) := {} (size = B,C)                            */
        NEWTABLE,

        /*   A B C   R(A+1) := R(B); R(A) := R(B)[RK(C)]                */
        SELF,

        /*   A B C   R(A) := RK(B) + RK(C)                              */
        ADD,

        /*   A B C   R(A) := RK(B) - RK(C)                              */
        SUB,

        /*   A B C   R(A) := RK(B) * RK(C)                              */
        MUL,

        /*   A B C   R(A) := RK(B) / RK(C)                              */
        DIV,

        /*   A B C   R(A) := RK(B) % RK(C)                              */
        MOD,

        /*   A B C   R(A) := RK(B) ^ RK(C)                              */
        POW,

        /*   A B     R(A) := -R(B)                                      */
        UNM,

        /*   A B     R(A) := not R(B)                                   */
        NOT,

        /*   A B     R(A) := length of R(B)                             */
        LEN,

        /*   A B C   R(A) := R(B).. ... ..R(C)                          */
        CONCAT,

        /*   sBx     pc+=sBx (different in 5.2)                         */
        JMP,

        /*   A B C   if ((RK(B) == RK(C)) ~= A) then pc++               */
        EQ,

        /*   A B C   if ((RK(B) <  RK(C)) ~= A) then pc++               */
        LT,

        /*   A B C   if ((RK(B) <= RK(C)) ~= A) then pc++               */
        LE,

        /*   A C     if not (R(A) <=> C) then pc++                      */
        TEST,

        /*   A B C   if (R(B) <=> C) then R(A) := R(B) else pc++        */
        TESTSET,

        /*   A B C   R(A), ... ,R(A+C-2) := R(A)(R(A+1), ... ,R(A+B-1)) */
        CALL,

        /*   A B C   return R(A)(R(A+1), ... ,R(A+B-1))                 */
        TAILCALL,

        /*   A B     return R(A), ... ,R(A+B-2)      (see note)         */
        RETURN,

        /*   A sBx   R(A)+=R(A+2);
             if R(A) <?= R(A+1) then { pc+=sBx; R(A+3)=R(A) }           */
        FORLOOP,

        /*   A sBx   R(A)-=R(A+2); pc+=sBx                              */
        FORPREP,

        /*   A C     R(A+3), ... ,R(A+3+C) := R(A)(R(A+1), R(A+2));
                if R(A+3) ~= nil then { pc++; R(A+2)=R(A+3); }          */
        TFORLOOP,

        /*   A B C   R(A)[(C-1)*FPF+i] := R(A+i), 1 <= i <= B           */
        SETLIST,

        /*   A       close all variables in the stack up to (>=) R(A)   */
        CLOSE,

        /*   A Bx    R(A) := closure(KPROTO[Bx], R(A), ... ,R(A+n))     */
        CLOSURE,

        /*   A B     R(A), R(A+1), ..., R(A+B-1) = vararg               */
        VARARG,

        // Lua 5.2 Opcodes
        LOADKX,
        GETTABUP,
        SETTABUP,
        TFORCALL,
        EXTRAARG,
    }

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
}
