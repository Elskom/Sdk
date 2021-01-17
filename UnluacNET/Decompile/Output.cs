// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.IO;

    public class Output
    {
        private TextWriter m_writer;
        private int m_indentationLevel = 0;
        private int m_position = 0;

        public int IndentationLevel
        {
            get => this.m_indentationLevel;
            set => this.m_indentationLevel = value;
        }

        public int Position => this.m_position;

        public void IncreaseIndent()
            => this.m_indentationLevel += 2;

        public void DecreaseIndent()
            => this.m_indentationLevel -= 2;

        private void Start()
        {
            if (this.m_position == 0)
            {
                for (var i = this.m_indentationLevel; i != 0; i--)
                {
                    this.m_writer.Write(" ");
                    this.m_position++;
                }
            }
        }

        public void Print(string str)
        {
            this.Start();
            this.m_writer.Write(str);
            this.m_position += str.Length;
        }

        public void PrintLine()
        {
            this.Start();
            this.m_writer.WriteLine();
            this.m_position = 0;
        }

        public void PrintLine(string str)
        {
            this.Start();
            this.m_writer.WriteLine(str);
            this.m_position = 0;
        }

        public Output()
            : this(Console.Out) { }
        
        public Output(TextWriter writer)
            => this.m_writer = writer;
    }
}
