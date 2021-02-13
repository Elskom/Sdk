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
        // -1 to tell Els_kom to loop it back to it's default value.
        [JsonPropertyName("WindowIcon")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int WindowIcon { get; set; } = -1;

        [JsonPropertyName("ElsDir")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public string ElsDir { get; set; } = string.Empty;

        // -1 to tell Els_kom to loop it back to it's default value.
        [JsonPropertyName("IconWhileElsNotRunning")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int IconWhileElsNotRunning { get; set; } = -1;

        // -1 to tell Els_kom to loop it back to it's default value.
        [JsonPropertyName("IconWhileElsRunning")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int IconWhileElsRunning { get; set; } = -1;

        [JsonPropertyName("LoadPDB")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int LoadPDB { get; set; }

        [JsonPropertyName("SaveToZip")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int SaveToZip { get; set; }

        [JsonPropertyName("ShowTestMessages")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int ShowTestMessages { get; set; }

        [JsonPropertyName("UseNotifications")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public int UseNotifications { get; set; }

        [JsonPropertyName("Sources")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No docs needed.")]
        public JsonSettingsSources Sources { get; private set; } = new JsonSettingsSources();

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
        {
            if (options == null)
            {
                options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
            }

            return JsonSerializer.Serialize(value, options);
        }

        /// <summary>
        /// Returns a new instance of this type either with preset contents
        /// from the settings file or with default settings to create a settings file.
        /// </summary>
        /// <returns>A new instance of this type.</returns>
        public static JsonSettings OpenFile()
            => File.Exists(SettingsFile.SettingsPath)
            ? Deserialize(File.ReadAllText(SettingsFile.SettingsPath))
            : new JsonSettings();

        /// <summary>
        /// Returns a new instance of this type so this one can be discarded.
        /// </summary>
        /// <returns>A new instance of this type.</returns>
        public JsonSettings ReopenFile()
        {
            this.Save();
            return Deserialize(File.ReadAllText(SettingsFile.SettingsPath));
        }

        /// <summary>
        /// Saves the data in this instance to file.
        /// </summary>
        public void Save()
            => File.WriteAllText(SettingsFile.SettingsPath, Serialize(this));
    }
}
