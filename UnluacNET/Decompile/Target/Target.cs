// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public abstract class Target
    {
        public virtual bool IsFunctionName => true;

        public virtual bool IsLocal => false;

        public virtual int GetIndex()
            => throw new InvalidOperationException();

        public virtual bool IsDeclaration(Declaration decl)
            => false;

        public abstract void Print(Output output);

        public abstract void PrintMethod(Output output);
    }
}
