﻿// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.IO;

    public class LSourceLines
    {
        // TODO: Encapsulate a LuaStream of some sort to automatically support Big-Endian
        public static LSourceLines Parse(Stream stream)
            => throw new InvalidOperationException("LSourceLines::Parse isn't implemented properly!");

        // var number = stream.ReadInt32();
        // while (number-- > 0)
        // {
        //     stream.ReadInt32();
        // }
        //
        // if (number > 0)
        // {
        //     stream.Position += (number * sizeof(int));
        // }
        //
        // return null;
    }
}
