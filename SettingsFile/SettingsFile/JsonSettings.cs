// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Json Setting file data.
    /// </summary>
    public class JsonSettings
    {
        [JsonPropertyName("WindowIcon")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int WindowIcon { get; private set; }

        [JsonPropertyName("ElsDir")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public string ElsDir { get; private set; }

        [JsonPropertyName("IconWhileElsNotRunning")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int IconWhileElsNotRunning { get; private set; }

        [JsonPropertyName("IconWhileElsRunning")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int IconWhileElsRunning { get; private set; }

        [JsonPropertyName("LoadPDB")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int LoadPDB { get; private set; }

        [JsonPropertyName("SaveToZip")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int SaveToZip { get; private set; }

        [JsonPropertyName("ShowTestMessages")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int ShowTestMessages { get; private set; }

        [JsonPropertyName("UseNotifications")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int UseNotifications { get; private set; }

        [JsonPropertyName("UseNotifications")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public JsonSettingsSources Sources { get; private set; }

        /// <summary>
        /// Deserializes the input json data to the target type for the settings file.
        /// </summary>
        /// <param name="json">The json data to Deserialize.</param>
        /// <param name="options">The options to deserialize with.</param>
        /// <returns>The target type instance for the settings file.</returns>
        public static JsonSettings Deserialize(string json, JsonSerializerOptions options = null)
            => JsonSerializer.Deserialize<JsonSettings>(json, options);

        /// <summary>
        /// Serializes the input type instance to a json string.
        /// </summary>
        /// <param name="value">The object for which holds the data to the input type instance.</param>
        /// <param name="options">The options to serialize with.</param>
        /// <returns>The json string to the input type instance's data.</returns>
        public static string Serialize(JsonSettings value, JsonSerializerOptions options = null)
            => JsonSerializer.Serialize(value, options);

        /// <summary>
        /// Returns a new instance of this type so this one can be discarded.
        /// </summary>
        /// <returns>A new instance of this type.</returns>
        public JsonSettings ReopenFile()
        {
            File.WriteAllText(SettingsFile.SettingsPath, Serialize(this));
            return Deserialize(File.ReadAllText(SettingsFile.SettingsPath));
        }
    }
}
