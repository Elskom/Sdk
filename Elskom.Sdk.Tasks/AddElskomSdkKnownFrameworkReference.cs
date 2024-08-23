namespace Elskom.Sdk.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

public class AddElskomSdkKnownFrameworkReference : Task
{
    [Required]
    public string SharedFrameworkVersion { get; set; } = null!;

    [Output]
    public ITaskItem[] KnownFrameworkReferences { get; set; } = null!;

    public override bool Execute()
    {
        var knownFrameworkReference = TaskHelpers.AddKnownFrameworkReference(this.SharedFrameworkVersion);
        this.KnownFrameworkReferences = TaskHelpers.ReturnItemOrEmpty(
            knownFrameworkReference is not null,
            knownFrameworkReference!);
        return true;
    }
}
