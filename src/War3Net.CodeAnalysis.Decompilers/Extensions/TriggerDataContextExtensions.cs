// ------------------------------------------------------------------------------
// <copyright file="TriggerDataContextExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;

namespace War3Net.CodeAnalysis.Decompilers.Extensions
{
    internal static class TriggerDataContextExtensions
    {
        public static bool TryGetTriggerConditionForUnknownType(
            this TriggerDataContext triggerData,
            string scriptText,
            [NotNullWhen(true)] out TriggerData.TriggerCondition? triggerCondition,
            [NotNullWhen(true)] out TriggerData.TriggerParam? triggerParam)
        {
            foreach (var pair in triggerData.TriggerConditions)
            {
                if (triggerData.TriggerParams.TryGetValue(pair.Key, out var triggerParamsForType) &&
                    triggerParamsForType.TryGetValue(scriptText, out var triggerParams))
                {
                    triggerCondition = pair.Value;
                    triggerParam = triggerParams.Single();
                    return true;
                }
            }

            triggerCondition = null;
            triggerParam = null;
            return false;
        }
    }
}