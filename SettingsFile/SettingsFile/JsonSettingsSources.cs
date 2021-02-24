// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Json settings file Sources data.
    /// </summary>
    public class JsonSettingsSources
    {
        internal JsonSettingsSources()
            => this.Source = new List<string>();

        /// <summary>
        /// Gets the list of sources.
        /// </summary>
        [JsonPropertyName("Source")]
        public List<string> Source { get; private set; }
    }
}
