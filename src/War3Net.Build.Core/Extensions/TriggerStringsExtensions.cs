// ------------------------------------------------------------------------------
// <copyright file="TriggerStringsExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class TriggerStringsExtensions
    {
        public static bool TryGetValue(this TriggerStrings triggerStrings, string? trigstr, out string? value)
        {
            if (trigstr is not null &&
                trigstr.StartsWith("TRIGSTR_", StringComparison.Ordinal) &&
                uint.TryParse(trigstr["TRIGSTR_".Length..], out var key))
            {
                var triggerString = triggerStrings.Strings.FirstOrDefault(s => s.Key == key);
                if (triggerString is not null)
                {
                    value = triggerString.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }
    }
}