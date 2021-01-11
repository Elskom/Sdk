// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.IO;

    public static class MemoryStreamExtensions
    {
        public static void Clear(this MemoryStream ms)
        {
            if (ms == null)
            {
                throw new ArgumentNullException(nameof(ms));
            }

            var buffer = ms.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            ms.Position = 0;
            ms.SetLength(0);
            ms.Capacity = 0; // <<< this one ******
        }
    }
}
