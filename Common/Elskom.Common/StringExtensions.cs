// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs;

internal static class StringExtensions
{
    public static string ReplaceStr(this string str1, string str2, string str3, StringComparison comp)
    {
        var args = new object[] { str2, str3, comp };
        var method = typeof(string).GetMethod(
            "Replace",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly,
            null,
            Type.GetTypeArray(args),
            null);
        return method is not null ? (string)method.Invoke(str1, args) : str1.Replace(str2, str3);
    }
}
