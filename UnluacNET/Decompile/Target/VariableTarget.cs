// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    
    public class VariableTarget : Target
    {
        public Declaration Declaration { get; private set; }

        public override bool IsLocal => true;

        public override bool IsDeclaration(Declaration decl)
            => this.Declaration == decl;

        public override bool Equals(object obj)
        {
            if (obj is VariableTarget)
                return this.Declaration == ((VariableTarget)obj).Declaration;
            else
                return false;
        }

        public override int GetIndex()
            => this.Declaration.Register;

        public override void Print(Output output)
            => output.Print(this.Declaration.Name);

        public override void PrintMethod(Output output)
            => throw new InvalidOperationException();

        public VariableTarget(Declaration decl)
            => this.Declaration = decl;
    }
}
