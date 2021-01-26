// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace GitBuildInfo
{
#if !NOTASKS
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
            this.SetGeneric(true);
            return base.Execute();
        }
    }
#else
    /// <summary>
    /// A MSBuild task that generates the msbuild information for an assembly.
    ///
    /// Note: use in the BeforeBuild target.
    /// </summary>
    public class GitBuildInfoGenericTask : GitBuildInfoTask
    {
    }
#endif
}
