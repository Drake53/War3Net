// ------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class StringExtensions
    {
        [return: NotNullIfNotNull("s")]
        public static string? Localize(this string? s, TriggerStrings? triggerStrings)
        {
            if (s is null || triggerStrings is null)
            {
                return s;
            }

            return s.StartsWith("TRIGSTR_", StringComparison.Ordinal) && uint.TryParse(s["TRIGSTR_".Length..], out var key)
                ? triggerStrings.Strings.First(s => s.Key == key).Value ?? s
                : s;
        }
    }
}