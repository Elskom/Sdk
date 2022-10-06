namespace Elskom.Sdk.Tasks;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

public class AddElskomSdkFrameworkReference : Task
{
    public bool DisableImplicitReference { get; set; }
    [Output]
    public ITaskItem[] FrameworkReferences { get; set; } = null!;

    public override bool Execute()
    {
        FrameworkReferences = TaskHelpers.ReturnItemOrEmpty(
            !this.DisableImplicitReference,
            TaskHelpers.AddFrameworkReference());
        return true;
    }
}
