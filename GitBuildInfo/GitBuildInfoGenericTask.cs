namespace GitBuildInfo
{
    /// <summary>
    /// A MSBuild task that generates the msbuild information for an assembly.
    ///
    /// Note: use in the BeforeBuild target.
    /// </summary>
    public class GitBuildInfoGenericTask : GitBuildInfoTask
    {
        /// <inheritdoc/>
        public override bool Execute()
        {
            this.isGeneric = true;
            return base.Execute();
        }
    }
}
