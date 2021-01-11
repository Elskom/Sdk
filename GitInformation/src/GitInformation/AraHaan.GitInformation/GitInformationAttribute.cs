// Copyright (c) 2019-2020, AraHaan.
// https://github.com/AraHaan/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace System.Runtime.InteropServices
{
    // This attribute can only be used on an assemly.

    /// <summary>
    /// Attribute that creates and registers a instance of the <see cref="GitInformation" /> class for the assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class GitInformationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitInformationAttribute"/> class.
        /// </summary>
        /// <param name="headdesc">
        /// The head description of the git repository to the assembly.
        /// </param>
        /// <param name="commit">
        /// The commit to the git repository to the assembly.
        /// </param>
        /// <param name="branchname">
        /// The branch name to the git repository to the assembly.
        /// </param>
        /// <param name="assemblyType">
        /// The <see langword="type"/> of the <see langword="assembly"/> <see langword="this"/> attribute <see langword="is"/> applied to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="assemblyType"/> is <see langword="null"/>.
        /// </exception>
        public GitInformationAttribute(string headdesc, string commit, string branchname, Type assemblyType/*object assembly*/)
        {
            if (assemblyType == null)
            {
                throw new ArgumentNullException(nameof(assemblyType));
            }

            GitInformation.ApplyAssemblyAttributes(typeof(GitInformation).Assembly);
            _ = new GitInformation(headdesc, commit, branchname, assemblyType.Assembly);
        }
    }
}
