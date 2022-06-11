// ------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Extensions
{
    internal static class ObjectExtensions
    {
        internal static string Optional(this object? obj)
        {
            return obj?.ToString() ?? string.Empty;
        }

        internal static string OptionalPrefixed(this object? obj, string prefix = " ")
        {
            if (obj is null)
            {
                return string.Empty;
            }

            return $"{prefix}{obj}";
        }

        internal static string OptionalSuffixed(this object? obj, string suffix = " ")
        {
            if (obj is null)
            {
                return string.Empty;
            }

            return $"{obj}{suffix}";
        }
    }
}