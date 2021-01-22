// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public class Function
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:Variable names should not be prefixed", Justification = "Don't care for now.")]
        private readonly Constant[] m_constants;

        public Function(LFunction function)
        {
            this.m_constants = new Constant[function.Constants.Length];
            for (var i = 0; i < this.m_constants.Length; i++)
            {
                this.m_constants[i] = new Constant(function.Constants[i]);
            }
        }

        public string GetGlobalName(int constantIndex)
            => this.m_constants[constantIndex].AsName();

        public ConstantExpression GetConstantExpression(int constantIndex)
            => new ConstantExpression(this.m_constants[constantIndex], constantIndex);

        public GlobalExpression GetGlobalExpression(int constantIndex)
            => new GlobalExpression(this.GetGlobalName(constantIndex), constantIndex);
    }
}
