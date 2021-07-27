// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs;

using System.IO;

/// <summary>
/// Interface for Els_kom kom entry Encryption and Decryption plugins (Version 1.5.0.0 or newer).
///
/// Interface plugins can be made in C++ Common Language Runtime (/clr) to mix
/// unmanaged and managed code to avoid anyone from easily decompiling the code to
/// C# and getting private things like encryption keys to an particular kom file.
/// </summary>
public interface IEncryptionPlugin
{
    /// <summary>
    /// Gets the name of the Encryption plugin.
    /// </summary>
    /// <value>
    /// The name of the Encryption plugin.
    /// </value>
    string PluginName { get; }

    /// <summary>
    /// Decrypts an KOM file entry. If KOM file algorithm is not supported
    /// throw <see cref="NotUnpackableException"/>.
    /// </summary>
    /// <exception cref="NotUnpackableException">
    /// When the KOM file algorithm is not suppoted by the curently installed
    /// encryption plugin.
    /// </exception>
    /// <param name="input">The input data to Decrypt output replaces everything in the Stream.</param>
    /// <param name="kOMFileName">The file name the entry is from.</param>
    /// <param name="algorithm">The algorithm the entry is.</param>
    void DecryptEntry(Stream input, string kOMFileName, uint algorithm);

    /// <summary>
    /// Encrypts an KOM file entry. If KOM file algorithm is not supported
    /// throw <see cref="NotPackableException"/>.
    /// </summary>
    /// <exception cref="NotPackableException">
    /// When the KOM file algorithm is not suppoted by the curently installed
    /// encryption plugin.
    /// </exception>
    /// <param name="input">The input data to Encrypt output replaces everything in the Stream.</param>
    /// <param name="kOMFileName">The file name the entry is from.</param>
    /// <param name="algorithm">The algorithm the entry is.</param>
    void EncryptEntry(Stream input, string kOMFileName, uint algorithm);
}
