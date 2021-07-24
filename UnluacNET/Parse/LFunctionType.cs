// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics;
    using System.IO;
    using Elskom.Generic.Libs.UnluacNET.IO;

    public class LFunctionType : BObjectType<LFunction>
    {
        public static readonly LFunctionType TYPE51 = new();
        public static readonly LFunctionType TYPE52 = new LFunctionType52();

        public override LFunction Parse(Stream stream, BHeader header)
        {
            if (header.Debug)
            {
                Debug.WriteLine("-- beginning to parse function");
                Debug.WriteLine("-- parsing name...start...end...upvalues...params...varargs...stack");
            }

            LFunctionParseState s = new();
            this.ParseMain(stream, header, s);
            return new(
                header,
                s.Code,
                s.Locals.AsArray(),
                s.Constants.AsArray(),
                s.Upvalues,
                s.Functions.AsArray(),
                s.MaximumStackSize,
                s.LenUpvalues,
                s.LenParameter,
                s.VarArg);
        }

        protected void ParseCode(Stream stream, BHeader header, LFunctionParseState s)
        {
            if (header.Debug)
            {
                Debug.WriteLine("-- beginning to parse bytecode list");
            }

            // HACK HACK HACK
            var bigEndian = header.BigEndian;
            s.Length = header.Integer.Parse(stream, header).AsInteger();
            s.Code = new int[s.Length];
            for (var i = 0; i < s.Length; i++)
            {
                s.Code[i] = stream.ReadInt32(bigEndian);
                if (header.Debug)
                {
                    Debug.WriteLine($"-- parsed codepoint 0x{s.Code[i]:X}");
                }
            }
        }

        protected void ParseConstants(Stream stream, BHeader header, LFunctionParseState s)
        {
            if (header.Debug)
            {
                Debug.WriteLine("-- beginning to parse constants list");
            }

            s.Constants = header.Constant.ParseList(stream, header);
            if (header.Debug)
            {
                Debug.WriteLine("-- beginning to parse functions list");
            }

            s.Functions = header.Function.ParseList(stream, header);
        }

        protected virtual void ParseDebug(Stream stream, BHeader header, LFunctionParseState s)
        {
            if (header.Debug)
            {
                Debug.WriteLine("-- beginning to parse source lines list");
            }

            s.Lines = header.Integer.ParseList(stream, header);
            if (header.Debug)
            {
                Debug.WriteLine("-- beginning to parse locals list");
            }

            s.Locals = header.Local.ParseList(stream, header);
            if (header.Debug)
            {
                Debug.WriteLine("-- beginning to parse upvalues list");
            }

            var upvalNames = header.String.ParseList(stream, header);
            var count = upvalNames.Length.AsInteger();
            for (var i = 0; i < count; i++)
            {
                s.Upvalues[i].Name = upvalNames[i].DeRef();
            }
        }

        protected virtual void ParseMain(Stream stream, BHeader header, LFunctionParseState s)
        {
            s.Name = header.String.Parse(stream, header);
            s.LineBegin = header.Integer.Parse(stream, header).AsInteger();
            s.LineEnd = header.Integer.Parse(stream, header).AsInteger();
            s.LenUpvalues = stream.ReadByte();
            s.LenParameter = stream.ReadByte();
            s.VarArg = stream.ReadByte();
            s.MaximumStackSize = stream.ReadByte();
            this.ParseCode(stream, header, s);
            this.ParseConstants(stream, header, s);
            this.ParseUpvalues(stream, header, s);
            this.ParseDebug(stream, header, s);
        }

        protected virtual void ParseUpvalues(Stream stream, BHeader header, LFunctionParseState s)
        {
            s.Upvalues = new LUpvalue[s.LenUpvalues];
            for (var i = 0; i < s.LenUpvalues; i++)
            {
                s.Upvalues[i] = new();
            }
        }

        protected sealed class LFunctionParseState
        {
            public LString Name { get; set; }

            public int LineBegin { get; set; }

            public int LineEnd { get; set; }

            public int LenUpvalues { get; set; }

            public int LenParameter { get; set; }

            public int VarArg { get; set; }

            public int MaximumStackSize { get; set; }

            public int Length { get; set; }

            public int[] Code { get; set; }

            public BList<LObject> Constants { get; set; }

            public BList<LFunction> Functions { get; set; }

            public BList<BInteger> Lines { get; set; }

            public BList<LLocal> Locals { get; set; }

            public LUpvalue[] Upvalues { get; set; }
        }
    }
}
