// Copyright (c) 2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;

    internal static class ArrayExtensions
    {
        internal static byte[] Remove(this byte[] bytes, byte item)
        {
            var removecnt = 0;
            var index = 0;
            while ((index = Array.IndexOf(bytes, item, index)) > -1)
            {
                var gap = bytes.Length - index;

                // copy the data after the specific index over
                for (var i = 0; i < gap; i++)
                {
                    bytes[index + i] = bytes[index + i + 1];
                }

                removecnt++;
            }

            Array.Resize(ref bytes, bytes.Length - removecnt);
            return bytes;
        }
    }
}
