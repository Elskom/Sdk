// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class Output
{
    private readonly TextWriter m_writer;

    public Output()
        : this(Console.Out)
    {
    }

    public Output(TextWriter writer)
        => this.m_writer = writer;

    public int IndentationLevel { get; set; }

    public int Position { get; private set; }

    public void IncreaseIndent()
        => this.IndentationLevel += 2;

    public void DecreaseIndent()
        => this.IndentationLevel -= 2;

    public void Print(string str)
    {
        this.Start();
        this.m_writer.Write(str);
        this.Position += str.Length;
    }

    public void PrintLine()
    {
        this.Start();
        this.m_writer.WriteLine();
        this.Position = 0;
    }

    public void PrintLine(string str)
    {
        this.Start();
        this.m_writer.WriteLine(str);
        this.Position = 0;
    }

    private void Start()
    {
        if (this.Position == 0)
        {
            for (var i = this.IndentationLevel; i != 0; i--)
            {
                this.m_writer.Write(" ");
                this.Position++;
            }
        }
    }
}
