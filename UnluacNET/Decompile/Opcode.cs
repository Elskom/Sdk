// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public static class Opcode
    {
        // TODO: Optimize method
        public static string CodePointToString(Op opcode, LInstruction code)
        {
            var name = opcode.GetType().Name;
            return opcode switch
            {
                // A
                Op.CLOSE or Op.LOADKX => $"{name} {code.A}",

                // A_B
                Op.MOVE or Op.LOADNIL or Op.GETUPVAL or Op.SETUPVAL or Op.UNM or Op.NOT or Op.LEN or Op.RETURN or Op.VARARG => $"{name} {code.A} {code.B}",

                // A_C
                Op.TEST or Op.TFORLOOP or Op.TFORCALL => $"{name} {code.A} {code.C}",

                // A_B_C
                Op.LOADBOOL or Op.GETTABLE or Op.SETTABLE or Op.NEWTABLE or Op.SELF or Op.ADD or Op.SUB or Op.MUL or Op.DIV or Op.MOD or Op.POW or Op.CONCAT or Op.EQ or Op.LT or Op.LE or Op.TESTSET or Op.CALL or Op.TAILCALL or Op.SETLIST or Op.GETTABUP or Op.SETTABUP => $"{name} {code.A} {code.B} {code.C}",

                // A_Bx
                Op.LOADK or Op.GETGLOBAL or Op.SETGLOBAL or Op.CLOSURE => $"{name} {code.A} {code.Bx}",

                // A_sBx
                Op.FORLOOP or Op.FORPREP => $"{name} {code.A} {code.SBx}",

                // Ax
                Op.EXTRAARG => $"{name} <Ax>",

                // sBx
                Op.JMP => $"{name} {code.SBx}",
                _ => name,
            };
        }
    }
}
