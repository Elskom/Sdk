// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    public class Function
    {
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
            => new(this.m_constants[constantIndex], constantIndex);

        public GlobalExpression GetGlobalExpression(int constantIndex)
            => new(this.GetGlobalName(constantIndex), constantIndex);
    }
}
