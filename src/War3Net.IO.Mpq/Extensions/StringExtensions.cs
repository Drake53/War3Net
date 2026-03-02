using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace War3Net.IO.Mpq.Extensions
{
    internal static class StringExtensions
    {
        internal static ulong GetStringHash(this string s) => MpqHash.GetHashedFileName(s);

        [return: NotNullIfNotNull("s")]
        internal static string? GetFileName(this string? s) => s?.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
    }
}