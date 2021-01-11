// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class SRCategoryAttribute : CategoryAttribute
    {
        public SRCategoryAttribute(string category)
            : base(category)
        {
        }

        protected override string GetLocalizedString(string value)
            => SR.GetString(value);
    }
}
