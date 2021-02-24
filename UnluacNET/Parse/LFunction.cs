// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class LFunction : BObject
    {
        public LFunction(BHeader header, int[] code, LLocal[] locals, LObject[] constants, LUpvalue[] upvalues, LFunction[] functions, int maximumStackSize, int numUpValues, int numParams, int vararg)
        {
            this.Header = header;
            this.Code = code;
            this.Locals = locals;
            this.Constants = constants;
            this.UpValues = upvalues;
            this.Functions = functions;
            this.MaxStackSize = maximumStackSize;
            this.NumUpValues = numUpValues;
            this.NumParams = numParams;
            this.VarArg = vararg;
        }

        public BHeader Header { get; set; }

        public int[] Code { get; set; }

        public LLocal[] Locals { get; set; }

        public LObject[] Constants { get; set; }

        public LUpvalue[] UpValues { get; set; }

        public LFunction[] Functions { get; set; }

        public int MaxStackSize { get; set; }

        public int NumUpValues { get; set; }

        public int NumParams { get; set; }

        public int VarArg { get; set; }
    }
}
