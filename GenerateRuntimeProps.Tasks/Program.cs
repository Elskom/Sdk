namespace GenerateRuntimeProps;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Net.Http;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GenerateRuntimePropsTask : Task
{
    [Required]
    public string FileName { get; set; } = null!;

    [Required]
    public string PackageName { get; set; } = null!;

    public override bool Execute()
    {
        using var httpClient = new HttpClient();
        var version = string.Empty;
        var versionResponse = httpClient.GetAsync($"https://api.nuget.org/v3-flatcontainer/{PackageName}/index.json").GetAwaiter().GetResult();
        if (versionResponse.IsSuccessStatusCode)
        {
            var content = versionResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var versionInfos = JsonSerializer.Deserialize<VersionInfo>(content);
            version = versionInfos.Versions.LastOrDefault();
        }

        File.WriteAllText(FileName, $@"<Project>

  <PropertyGroup>
    <ElskomSdkFrameworkVersion Condition=""'$(ElskomSdkFrameworkVersion)' == ''"">{version}</ElskomSdkFrameworkVersion>
  </PropertyGroup>

</Project>
");
        return true;
    }

    private struct VersionInfo
    {
        [JsonPropertyName("versions")]
        public string[] Versions { get; set; }
    }
}
