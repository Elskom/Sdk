// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Linq;
    using Elskom.Generic.Libs.Properties;

    /// <summary>
    /// Class that allows managing kom Files.
    /// </summary>
    public static class KOMManager
    {
        private static List<IKomPlugin> komplugins;
        private static List<IEncryptionPlugin> encryptionplugins;
        private static List<ICallbackPlugin> callbackplugins;

        /// <summary>
        /// The event to which allows getting the message to do stuff with.
        /// </summary>
        public static event EventHandler<MessageEventArgs> MessageEvent;

        /// <summary>
        /// Gets a value indicating whether the current state on packing KOM files.
        /// </summary>
        /// <returns>The current state on packing KOM files.</returns>
        public static bool PackingState { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current state on unpacking KOM files.
        /// </summary>
        /// <returns>The current state on unpacking KOM files.</returns>
        public static bool UnpackingState { get; private set; }

        /// <summary>
        /// Gets The list of <see cref="IKomPlugin"/> plugins.
        /// </summary>
        /// <value>
        /// The list of <see cref="IKomPlugin"/> plugins.
        /// </value>
        public static List<IKomPlugin> Komplugins => komplugins ??= new();

        /// <summary>
        /// Gets The list of <see cref="IEncryptionPlugin"/> plugins.
        /// </summary>
        /// <value>
        /// The list of <see cref="IEncryptionPlugin"/> plugins.
        /// </value>
        public static List<IEncryptionPlugin> Encryptionplugins => encryptionplugins ??= new();

        /// <summary>
        /// Gets the list of <see cref="ICallbackPlugin"/> plugins.
        /// </summary>
        /// <value>
        /// The list of <see cref="ICallbackPlugin"/> plugins.
        /// </value>
        public static List<ICallbackPlugin> Callbackplugins => callbackplugins ??= new();

        /// <summary>
        /// Copies Modified KOM files to the Elsword Directory that was Set in the Settings Dialog in Els_kom. Requires: File Name, Original Directory the File is in, And Destination Directory.
        /// </summary>
        /// <param name="fileName">The name of the file to copy.</param>
        /// <param name="origFileDir">The original kom file location.</param>
        /// <param name="destFileDir">The target to copy the kom file too.</param>
        public static void CopyKomFiles(string fileName, string origFileDir, string destFileDir)
        {
            if (destFileDir is null)
            {
                throw new ArgumentNullException(nameof(destFileDir));
            }

            if (File.Exists($"{origFileDir}{fileName}") && Directory.Exists(destFileDir))
            {
                MoveOriginalKomFiles(fileName, destFileDir, $"{destFileDir}{Path.DirectorySeparatorChar}backup");
                if (!destFileDir.EndsWith($"{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
                {
                    // we must add this before copying the file to the target location.
                    destFileDir += Path.DirectorySeparatorChar;
                }

                File.Copy($"{origFileDir}{fileName}", $"{destFileDir}{fileName}");
            }
        }

        /// <summary>
        /// Moves the Original KOM Files back to their original locations, overwriting the modified ones.
        /// </summary>
        /// <param name="fileName">The name of the file to move.</param>
        /// <param name="origFileDir">The original kom file location.</param>
        /// <param name="destFileDir">The target to move the kom file too.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="origFileDir"/> or <paramref name="destFileDir"/> are <see langword="null"/> or empty.</exception>
        public static void MoveOriginalKomFilesBack(string fileName, string origFileDir, string destFileDir)
        {
            if (string.IsNullOrEmpty(origFileDir))
            {
                throw new ArgumentNullException(nameof(origFileDir));
            }

            if (string.IsNullOrEmpty(destFileDir))
            {
                throw new ArgumentNullException(nameof(destFileDir));
            }

            if (!origFileDir.EndsWith($"{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            {
                origFileDir += Path.DirectorySeparatorChar;
            }

            if (!destFileDir.EndsWith($"{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            {
                destFileDir += Path.DirectorySeparatorChar;
            }

            if (File.Exists($"{origFileDir}{fileName}") && File.Exists($"{destFileDir}{fileName}"))
            {
                File.Copy($"{origFileDir}{fileName}", $"{destFileDir}{fileName}", true);
                File.Delete($"{origFileDir}{fileName}");
            }
        }

        /// <summary>
        /// Unpacks KOM files by invoking the extractor.
        /// </summary>
        public static void UnpackKoms()
        {
            UnpackingState = true;
            DirectoryInfo di = new($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms");
            foreach (var fi in di.GetFiles("*.kom"))
            {
                var kom_file = fi.Name;
                var kom_ver = GetHeaderVersion(kom_file);
                if (kom_ver != 0)
                {
                    // remove ".kom" on end of string.
                    var kom_data_folder = Path.GetFileNameWithoutExtension(
                        $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_file}");
                    foreach (var komplugin in Komplugins)
                    {
                        try
                        {
                            if (kom_ver == komplugin.SupportedKOMVersion)
                            {
                                komplugin.Unpack($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_file}", $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_data_folder}", kom_file);
                            }
                            else
                            {
                                // loop until the right plugin for this kom version is found.
                                continue;
                            }

                            // make the version dummy file for the packer.
                            try
                            {
                                using (File.Create($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_data_folder}{Path.DirectorySeparatorChar}KOMVERSION.{kom_ver}"))
                                {
                                    // just need to create the dummy file.
                                }
                            }
                            catch (DirectoryNotFoundException)
                            {
                                // cannot create this since nothing was written or made.
                            }

                            // delete original kom file.
                            komplugin.Delete($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_file}", false);
                        }
                        catch (NotUnpackableException)
                        {
                            // do not delete kom file.
                            MessageEventArgs args = new(
                                Resources.KOMManager_Unpacking_KOM_File_Failed,
                                Resources.Error,
                                ErrorLevel.Error);
                            InvokeMessageEvent(args);
                        }
                        catch (NotImplementedException)
                        {
                            MessageEventArgs args = new(
                                string.Format(
                                    Resources.KOMManager_Plugin_No_Unpacker_Function,
                                    komplugin.SupportedKOMVersion),
                                Resources.Error,
                                ErrorLevel.Error);
                            InvokeMessageEvent(args);
                        }
                    }
                }
                else
                {
                    MessageEventArgs args = new(
                        Resources.KOMManager_Unkown_KOM_Version,
                        Resources.Error,
                        ErrorLevel.Error);
                    InvokeMessageEvent(args);
                }
            }

            UnpackingState = false;
        }

        /// <summary>
        /// Packs KOM files by invoking the packer.
        /// </summary>
        public static void PackKoms()
        {
            PackingState = true;
            DirectoryInfo di = new($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms");
            foreach (var dri in di.GetDirectories())
            {
                var kom_data_folder = dri.Name;
                var kom_ver = CheckFolderVersion(
                    $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_data_folder}");
                if (kom_ver != 0)
                {
                    var kom_file = $"{kom_data_folder}.kom";

                    // pack kom based on the version of kom supplied.
                    if (kom_ver != -1)
                    {
                        foreach (var komplugin in Komplugins)
                        {
                            try
                            {
                                if (kom_ver.Equals(komplugin.SupportedKOMVersion))
                                {
                                    komplugin.Pack($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_data_folder}", $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_file}", kom_file);

                                    // delete unpacked kom folder data.
                                    komplugin.Delete($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_data_folder}", true);
                                }
                            }
                            catch (NotPackableException)
                            {
                                // do not delete kom data folder.
                                using (File.Create($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_data_folder}{Path.DirectorySeparatorChar}\\KOMVERSION.{komplugin.SupportedKOMVersion}"))
                                {
                                    // just need to create the dummy file.
                                }

                                MessageEventArgs args = new(
                                    Resources.KOMManager_Packing_KOM_File_Failed,
                                    Resources.Error,
                                    ErrorLevel.Error);
                                InvokeMessageEvent(args);
                            }
                            catch (NotImplementedException)
                            {
                                using (File.Create($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{kom_data_folder}{Path.DirectorySeparatorChar}KOMVERSION.{komplugin.SupportedKOMVersion}"))
                                {
                                    // just need to create the dummy file.
                                }

                                MessageEventArgs args = new(
                                    string.Format(
                                        Resources.KOMManager_Plugin_No_Packer_Function,
                                        komplugin.SupportedKOMVersion),
                                    Resources.Error,
                                    ErrorLevel.Error);
                                InvokeMessageEvent(args);
                            }
                        }
                    }
                    else
                    {
                        MessageEventArgs args = new(
                            Resources.KOMManager_Folder_Version_Check_Error,
                            Resources.Error,
                            ErrorLevel.Error);
                        InvokeMessageEvent(args);
                    }
                }
                else
                {
                    MessageEventArgs args = new(
                        Resources.KOMManager_Unkown_KOM_Version,
                        Resources.Error,
                        ErrorLevel.Error);
                    InvokeMessageEvent(args);
                }
            }

            PackingState = false;
        }

        /// <summary>
        /// Converts the KOM crc.xml file to the provided version,
        /// if it is not already that version.
        /// </summary>
        /// <param name="toVersion">The version to convert the CRC.xml file to.</param>
        /// <param name="crcpath">The path to the crc.xml file.</param>
        public static void ConvertCRC(int toVersion, string crcpath)
        {
            if (File.Exists(crcpath))
            {
                var crcversion = GetCRCVersion(File.ReadAllText(crcpath));
                if (crcversion != toVersion)
                {
                    foreach (var plugin in Komplugins)
                    {
                        if (toVersion.Equals(plugin.SupportedKOMVersion))
                        {
                            plugin.ConvertCRC(crcversion, crcpath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the crc.xml file if the folder has new files
        /// not in the crc.xml, or removes files listed in crc.xml that are
        /// no longer in the folder.
        /// </summary>
        /// <param name="crcversion">The version of the crc.xml file.</param>
        /// <param name="crcpath">The path to the crc.xml file.</param>
        /// <param name="checkpath">The directry that contains the crc.xml file.</param>
        public static void UpdateCRC(int crcversion, string crcpath, string checkpath)
        {
            FileInfo crcfile = new(crcpath);
            DirectoryInfo di1 = new(checkpath);
            foreach (var fi1 in di1.GetFiles())
            {
                if (!fi1.Name.Equals(crcfile.Name, StringComparison.Ordinal))
                {
                    var found = false;

                    // lookup the file entry in the crc.xml.
                    var xmldata = File.ReadAllText(crcpath);
                    var xml = XElement.Parse(xmldata);
                    if (crcversion > 2)
                    {
                        foreach (var fileElement in xml.Elements("File"))
                        {
                            var nameAttribute = fileElement.Attribute("Name");
                            var name = nameAttribute?.Value ?? "no value";
                            if (name.Equals(fi1.Name, StringComparison.Ordinal))
                            {
                                found = true;
                            }
                        }
                    }
                    else
                    {
                        // TODO: Iterate through every entry in the kom v2 crc.xml file.
                    }

                    if (!found)
                    {
                        foreach (var plugin in Komplugins)
                        {
                            if (crcversion.Equals(plugin.SupportedKOMVersion))
                            {
                                plugin.UpdateCRC(crcpath, checkpath);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deploys the Test Mods callback functions provided by plugins.
        /// This is a blocking call that has to run in a separate thread from Els_kom's main thread.
        /// NEVER UNDER ANY CIRCUMSTANCES RUN THIS IN THE MAIN THREAD, YOU WILL DEADLOCK ELS_KOM!!!.
        /// </summary>
        /// <param name="directRun">if the game was executed directly.</param>
        public static void DeployCallBack(bool directRun)
        {
            if (directRun)
            {
                foreach (var plugin in Callbackplugins)
                {
                    plugin.TestModsCallback();
                }
            }
        }

        /// <summary>
        /// Gets the base file name of the KOM File.
        /// </summary>
        /// <param name="fileName">The input kom file.</param>
        /// <returns>The base file name of the KOM File.</returns>
        public static string GetFileBaseName(string fileName)
        {
            FileInfo fi = new(fileName);
            return fi.Name;
        }

        /// <summary>
        /// Gets the crc.xml file version.
        /// </summary>
        /// <param name="xmldata">The data in the crc.xml file.</param>
        /// <returns>The version of the crc.xml file.</returns>
        internal static int GetCRCVersion(string xmldata)
        {
            var result = 0;
            var xml = XElement.Parse(xmldata);
            if (xml.Element("File") != null)
            {
                // version 3 or 4.
                foreach (var fileElement in xml.Elements("File"))
                {
                    var mappedIDAttribute = fileElement.Attribute("MappedID");
                    result = mappedIDAttribute != null ? 4 : 3;
                }
            }
            else
            {
                result = 2;
            }

            return result;
        }

        internal static void InvokeMessageEvent(MessageEventArgs e)
            => MessageEvent?.Invoke(null, e);

        private static void MoveOriginalKomFiles(string fileName, string origFileDir, string destFileDir)
        {
            if (!origFileDir.EndsWith($"{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            {
                origFileDir += Path.DirectorySeparatorChar;
            }

            if (!destFileDir.EndsWith($"{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            {
                destFileDir += Path.DirectorySeparatorChar;
            }

            if (File.Exists($"{origFileDir}{fileName}"))
            {
                if (!Directory.Exists(destFileDir))
                {
                    _ = Directory.CreateDirectory(destFileDir);
                }

                if (File.Exists($"{destFileDir}{fileName}"))
                {
                    File.Delete($"{destFileDir}{fileName}");
                }

                File.Move($"{origFileDir}{fileName}", $"{destFileDir}{fileName}");
            }
        }

        private static int GetHeaderVersion(string komfile)
        {
            var ret = 0;
            byte[] headerbuffer;

            // 27 is the size of the header string denoting the KOM file version number.
            using (var fs = File.OpenRead($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}koms{Path.DirectorySeparatorChar}{komfile}"))
            using (BinaryReader reader = new(fs, Encoding.ASCII))
            {
                headerbuffer = reader.ReadBytes(27);
            }

            var headerstring = Encoding.UTF8.GetString(headerbuffer);
            foreach (var komplugin in Komplugins)
            {
                // get version of kom file for unpacking it.
                if (string.IsNullOrEmpty(komplugin.KOMHeaderString))
                {
                    // skip this plugin it does not implement an packer or unpacker.
                    continue;
                }

                if (headerstring == komplugin.KOMHeaderString)
                {
                    ret = komplugin.SupportedKOMVersion;
                }
            }

            return ret;
        }

        private static int CheckFolderVersion(string datafolder)
        {
            var ret = 0;
            foreach (var komplugin in Komplugins)
            {
                if (komplugin.SupportedKOMVersion != 0 && File.Exists($"{datafolder}{Path.DirectorySeparatorChar}KOMVERSION.{komplugin.SupportedKOMVersion}"))
                {
                    try
                    {
                        File.Delete($"{datafolder}{Path.DirectorySeparatorChar}KOMVERSION.{komplugin.SupportedKOMVersion}");
                        ret = komplugin.SupportedKOMVersion;
                    }
                    catch (IOException)
                    {
                        ret = -1;
                    }
                }
            }

            return ret;
        }
    }
}
