// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET;

public class BHeader
{
    private static readonly int Signature = 0x61754C1B; // '\x1B\Lua'

    public BHeader(Stream stream)
    {
        // 4-byte Lua signature
        if (stream.ReadInt32() != Signature)
        {
            throw new InvalidOperationException("The input file does not have the signature of a valid Lua file.");
        }

        // 1-byte Lua version
        var version = stream.ReadByte();
        switch (version)
        {
            case 0x51:
            {
                this.Version = Version.LUA51;
                break;
            }

            case 0x52:
            {
                this.Version = Version.LUA52;
                break;
            }

            default:
            {
                var major = version >> 4;
                var minor = version & 0x0F;
                var error = $"The input chunk's Lua version is {major}.{minor}; unluac can only handle Lua 5.1 and Lua 5.2.";
                throw new InvalidOperationException(error);
            }
        }

        if (this.Debug)
        {
            System.Diagnostics.Debug.WriteLine($"-- version: 0x{version:X}");
        }

        // 1-byte Lua "format"
        var format = stream.ReadByte();
        if (format != 0)
        {
            throw new InvalidOperationException($"The input chunk reports a non-standard lua format: {format}");
        }

        if (this.Debug)
        {
            System.Diagnostics.Debug.WriteLine($"-- format: {format}");
        }

        // 1-byte endianness
        var endianness = stream.ReadByte();
        if (endianness > 1)
        {
            throw new InvalidOperationException($"The input chunk reports an invalid endianness: {endianness}");
        }

        this.BigEndian = endianness == 0;
        if (this.Debug)
        {
            System.Diagnostics.Debug.WriteLine($"-- endianness: {(this.BigEndian ? "0 (big)" : "1 (little)")}");
        }

        // 1-byte int size
        var intSize = stream.ReadByte();
        if (this.Debug)
        {
            System.Diagnostics.Debug.WriteLine($"-- int size: {intSize}");
        }

        this.Integer = new(intSize);

        // 1-byte sizeT size
        var sizeTSize = stream.ReadByte();
        if (this.Debug)
        {
            System.Diagnostics.Debug.WriteLine($"-- size_t size: {sizeTSize}");
        }

        this.SizeT = new(sizeTSize);

        // 1-byte instruction size
        var instructionSize = stream.ReadByte();
        if (this.Debug)
        {
            System.Diagnostics.Debug.WriteLine($"-- instruction size: {instructionSize}");
        }

        if (instructionSize != 4)
        {
            throw new InvalidOperationException($"The input chunk reports an unsupported instruction size: {instructionSize} bytes");
        }

        var lNumberSize = stream.ReadByte();
        if (this.Debug)
        {
            System.Diagnostics.Debug.WriteLine($"-- Lua number size: {lNumberSize}");
        }

        var lNumberIntegralCode = stream.ReadByte();
        if (this.Debug)
        {
            System.Diagnostics.Debug.WriteLine($"-- Lua number integral code: {lNumberIntegralCode}");
        }

        if (lNumberIntegralCode > 1)
        {
            throw new InvalidOperationException($"The input chunk reports an invalid code for lua number integralness: {lNumberIntegralCode}");
        }

        var lNumberIntegral = lNumberIntegralCode == 1;
        this.Number = new(lNumberSize, lNumberIntegral);
        this.Bool = new();
        this.String = new();
        this.Constant = new();
        this.Local = new();
        this.UpValue = new();
        this.Function = this.Version.GetLFunctionType();
        if (this.Version.HasHeaderTail)
        {
            Span<byte> luacTail = stackalloc byte[]
            {
                0x19, 0x93, 0x0D, 0x0A, 0x1A, 0x0A,
            };
            foreach (var t in luacTail)
            {
                if (stream.ReadByte() != t)
                {
                    throw new InvalidOperationException("The input chunk does not have the header tail of a valid Lua file.");
                }
            }
        }
    }

    public bool Debug { get; set; }

    public bool BigEndian { get; }

    public Version Version { get; }

    public BIntegerType Integer { get; }

    public BSizeTType SizeT { get; }

    public LBooleanType Bool { get; }

    public LNumberType Number { get; }

    public LStringType String { get; }

    public LConstantType Constant { get; }

    public LLocalType Local { get; }

    public LUpvalueType UpValue { get; }

    public LFunctionType Function { get; }
}
