// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

#if WITH_XDOCUMENT_EXTENSIONS
namespace XmlAbstraction
{
    using System.IO;
    using System.Xml.Linq;

    /// <summary>
    /// A hack class to bridge the gaps in <see cref="XDocument"/> for .NET Standard pre 2.0.
    /// </summary>
    internal static class XDocumentExtensions
    {
        /// <summary>
        /// Serialize this <see cref="XDocument"/> to a file, overwriting an existing file,
        /// if it exists.
        /// </summary>
        /// <param name="xdoc">The <see cref="XDocument"/> for which to save.</param>
        /// <param name="fileName">
        /// A string that contains the name of the file.
        /// </param>
        internal static void Save(this XDocument xdoc, string fileName)
        {
            using (var fstream = File.Create(fileName))
            {
                xdoc.Save(fstream);
            }
        }
    }
}
#endif
