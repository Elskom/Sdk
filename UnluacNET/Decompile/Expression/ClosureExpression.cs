// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class ClosureExpression : Expression
{
    private readonly LFunction m_function;
    private readonly int m_upvalueLine;

    public ClosureExpression(LFunction function, int upvalueLine)
        : base(PRECEDENCE_ATOMIC)
    {
        this.m_function = function;
        this.m_upvalueLine = upvalueLine;
    }

    public override int ConstantIndex => -1;

    public override int ClosureUpvalueLine => this.m_upvalueLine;

    public override bool IsClosure => true;

    public override bool IsUpvalueOf(int register)
    {
        foreach (var upvalue in this.m_function.UpValues)
        {
            if (upvalue.InStack && upvalue.Index == register)
            {
                return true;
            }
        }

        return false;
    }

    public override void Print(Output output)
    {
        Decompiler d = new(this.m_function);
        output.Print("function");
        this.PrintMain(output, d, true);
    }

    public override void PrintClosure(Output output, Target name)
    {
        Decompiler d = new(this.m_function);
        output.Print("function ");
        if (this.m_function.NumParams >= 1 && d.DeclList[0].Name.Equals("self") &&
            name is TableTarget)
        {
            name.PrintMethod(output);
            this.PrintMain(output, d, false);
        }
        else
        {
            name.Print(output);
            this.PrintMain(output, d, true);
        }
    }

    private void PrintMain(Output output, Decompiler d, bool includeFirst)
    {
        output.Print("(");
        var start = includeFirst ? 0 : 1;
        if (this.m_function.NumParams > start)
        {
            new VariableTarget(d.DeclList[start]).Print(output);
            for (var i = start + 1; i < this.m_function.NumParams; i++)
            {
                output.Print(", ");
                new VariableTarget(d.DeclList[i]).Print(output);
            }
        }

        if ((this.m_function.VarArg & 1) == 1)
        {
            output.Print(this.m_function.NumParams > start ? ", ..." : "...");
        }

        output.Print(")");
        output.PrintLine();
        output.IncreaseIndent();
        d.Decompile();
        d.Print(output);
        output.DecreaseIndent();
        output.Print("end");

        // output.PrintLine(); // This is an extra space for formatting
    }
}
