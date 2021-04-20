// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET.IO
{
    using System;
    using System.IO;

    public static class StreamExtensions
    {
        public static char ReadChar(this Stream stream)
            => (char)stream.ReadByte();

        public static char[] ReadChars(this Stream stream, int count)
        {
            Span<char> chars = stackalloc char[count];
            for (var i = 0; i < count; i++)
            {
                chars[i] = stream.ReadChar();
            }

            return chars.ToArray();
        }

        public static short ReadInt16(this Stream stream, bool bigEndian = false)
        {
            Span<byte> buffer = stackalloc byte[2];
            _ = stream.Read(buffer);
            if (bigEndian)
            {
                buffer.Reverse();
            }

            return BitConverter.ToInt16(buffer.ToArray(), 0);
        }

        public static ushort ReadUInt16(this Stream stream, bool bigEndian = false)
        {
            Span<byte> buffer = stackalloc byte[2];
            _ = stream.Read(buffer);
            if (bigEndian)
            {
                buffer.Reverse();
            }

            return BitConverter.ToUInt16(buffer.ToArray(), 0);
        }

        public static int ReadInt32(this Stream stream, bool bigEndian = false)
        {
            Span<byte> buffer = stackalloc byte[4];
            _ = stream.Read(buffer);
            if (bigEndian)
            {
                buffer.Reverse();
            }

            return BitConverter.ToInt32(buffer.ToArray(), 0);
        }

        public static uint ReadUInt32(this Stream stream, bool bigEndian = false)
        {
            Span<byte> buffer = stackalloc byte[4];
            _ = stream.Read(buffer);
            if (bigEndian)
            {
                buffer.Reverse();
            }

            return BitConverter.ToUInt32(buffer.ToArray(), 0);
        }

        public static long ReadInt64(this Stream stream, bool bigEndian = false)
        {
            Span<byte> buffer = stackalloc byte[8];
            _ = stream.Read(buffer);
            if (bigEndian)
            {
                buffer.Reverse();
            }

            return BitConverter.ToInt64(buffer.ToArray(), 0);
        }

        public static ulong ReadUInt64(this Stream stream, bool bigEndian = false)
        {
            Span<byte> buffer = stackalloc byte[8];
            _ = stream.Read(buffer);
            if (bigEndian)
            {
                buffer.Reverse();
            }

            return BitConverter.ToUInt64(buffer.ToArray(), 0);
        }

        public static double ReadFloat(this Stream stream, bool bigEndian = false)
        {
            var val = (double)stream.ReadSingle(bigEndian);
            return Math.Round(val, 3);
        }

        public static float ReadSingle(this Stream stream, bool bigEndian = false)
        {
            Span<byte> buffer = stackalloc byte[4];
            _ = stream.Read(buffer);
            if (bigEndian)
            {
                buffer.Reverse();
            }

            return BitConverter.ToSingle(buffer.ToArray(), 0);
        }

        public static double ReadDouble(this Stream stream, bool bigEndian = false)
        {
            Span<byte> buffer = stackalloc byte[8];
            _ = stream.Read(buffer);
            if (bigEndian)
            {
                buffer.Reverse();
            }

            return BitConverter.ToDouble(buffer.ToArray(), 0);
        }

        private static int Read(this Stream stream, Span<byte> buffer)
        {
            var cnt = 0;
            while (cnt < buffer.Length)
            {
                buffer[cnt] = (byte)stream.ReadByte();
                if (buffer[cnt] is unchecked((byte)-1))
                {
                    break;
                }

                cnt++;
            }

            return cnt;
        }
    }
}
