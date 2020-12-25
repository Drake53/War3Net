// ------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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