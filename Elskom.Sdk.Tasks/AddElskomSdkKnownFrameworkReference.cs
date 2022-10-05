namespace Elskom.Sdk.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

public class AddElskomSdkKnownFrameworkReference : Task
{
    [Output]
    public ITaskItem[] KnownFrameworkReferences { get; set; } = null!;

    public override bool Execute()
    {
        var knownFrameworkReference = TaskHelpers.AddKnownFrameworkReference();
        KnownFrameworkReferences = TaskHelpers.ReturnItemOrEmpty(
            knownFrameworkReference is not null,
            knownFrameworkReference!);
        return true;
    }
}
