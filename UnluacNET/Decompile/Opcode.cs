// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public static class Opcode
    {
        // TODO: Optimize method
        public static string CodePointToString(Op opcode, LInstruction code)
        {
            var name = opcode.GetType().Name;
            switch (opcode)
            {
                // A
                case Op.CLOSE:
                case Op.LOADKX:
                    return string.Format("{0} {1}", name, code.A);

                // A_B
                case Op.MOVE:
                case Op.LOADNIL:
                case Op.GETUPVAL:
                case Op.SETUPVAL:
                case Op.UNM:
                case Op.NOT:
                case Op.LEN:
                case Op.RETURN:
                case Op.VARARG:
                    return string.Format("{0} {1} {2}", name, code.A, code.B);

                // A_C
                case Op.TEST:
                case Op.TFORLOOP:
                case Op.TFORCALL:
                    return string.Format("{0} {1} {2}", name, code.A, code.C);

                // A_B_C
                case Op.LOADBOOL:
                case Op.GETTABLE:
                case Op.SETTABLE:
                case Op.NEWTABLE:
                case Op.SELF:
                case Op.ADD:
                case Op.SUB:
                case Op.MUL:
                case Op.DIV:
                case Op.MOD:
                case Op.POW:
                case Op.CONCAT:
                case Op.EQ:
                case Op.LT:
                case Op.LE:
                case Op.TESTSET:
                case Op.CALL:
                case Op.TAILCALL:
                case Op.SETLIST:
                case Op.GETTABUP:
                case Op.SETTABUP:
                    return string.Format("{0} {1} {2} {3}", name, code.A, code.B, code.C);

                // A_Bx
                case Op.LOADK:
                case Op.GETGLOBAL:
                case Op.SETGLOBAL:
                case Op.CLOSURE:
                    return string.Format("{0} {1} {2}", name, code.A, code.Bx);

                // A_sBx
                case Op.FORLOOP:
                case Op.FORPREP:
                    return string.Format("{0} {1} {2}", name, code.A, code.sBx);

                // Ax
                case Op.EXTRAARG:
                    return string.Format("{0} <Ax>", name);

                // sBx
                case Op.JMP:
                    return string.Format("{0} {1}", name, code.sBx);
                default:
                    return name;
            }
        }
    }
}
