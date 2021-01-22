// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs yet.")]
    public enum OpArgMask
    {
        /// <summary>
        /// Argument is not used.
        /// </summary>
        OpArgN,

        /// <summary>
        /// Argument is used.
        /// </summary>
        OpArgU,

        /// <summary>
        /// Argument is a register or a jump offset.
        /// </summary>
        OpArgR,

        /// <summary>
        /// Argument is a constant or register/constant.
        /// </summary>
        OpArgK,
    }
}
