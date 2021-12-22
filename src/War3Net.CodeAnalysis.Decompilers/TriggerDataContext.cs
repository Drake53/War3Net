// ------------------------------------------------------------------------------
// <copyright file="TriggerDataContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using War3Net.Build.Script;

namespace War3Net.CodeAnalysis.Decompilers
{
    public class TriggerDataContext
    {
        public TriggerDataContext(TriggerData? triggerData = null)
        {
            TriggerData = triggerData ?? TriggerData.Default;

            TriggerTypes = GetTriggerTypes(TriggerData.TriggerTypes.Values);
            TriggerParams = GetTriggerParams(TriggerData.TriggerParams.Values);
            TriggerConditions = GetTriggerConditions(TriggerData.TriggerConditions.Values);
            TriggerActions = GetTriggerActions(TriggerData.TriggerActions.Values);
        }

        public TriggerData TriggerData { get; }

        public ImmutableDictionary<string, ImmutableDictionary<string, TriggerData.TriggerType>> TriggerTypes { get; }

        public ImmutableDictionary<string, ImmutableDictionary<string, ImmutableArray<TriggerData.TriggerParam>>> TriggerParams { get; }

        public ImmutableDictionary<string, TriggerData.TriggerCondition> TriggerConditions { get; }

        public ImmutableDictionary<string, ImmutableArray<TriggerData.TriggerAction>> TriggerActions { get; }

        private static ImmutableDictionary<string, ImmutableDictionary<string, TriggerData.TriggerType>> GetTriggerTypes(IEnumerable<TriggerData.TriggerType> triggerTypes)
        {
            return triggerTypes
                .Where(type => !string.IsNullOrEmpty(type.BaseType))
                .GroupBy(type => type.BaseType!)
                .ToImmutableDictionary(
                    grouping => grouping.Key,
                    grouping => grouping
                        .ToImmutableDictionary(
                            type => type.TypeName,
                            type => type,
                            StringComparer.Ordinal),
                    StringComparer.Ordinal);
        }

        private static ImmutableDictionary<string, ImmutableDictionary<string, ImmutableArray<TriggerData.TriggerParam>>> GetTriggerParams(IEnumerable<TriggerData.TriggerParam> triggerParams)
        {
            return triggerParams
                .GroupBy(param => param.VariableType)
                .ToImmutableDictionary(
                    grouping => grouping.Key,
                    grouping => grouping
                        .GroupBy(param => param.ScriptText)
                        .ToImmutableDictionary(
                            grouping => grouping.Key,
                            grouping => grouping.ToImmutableArray(),
                            StringComparer.Ordinal),
                    StringComparer.Ordinal);
        }

        private static ImmutableDictionary<string, TriggerData.TriggerCondition> GetTriggerConditions(IEnumerable<TriggerData.TriggerCondition> triggerConditions)
        {
            return triggerConditions
                .Where(condition => condition.ArgumentTypes.Length == 3)
                .Where(condition => string.Equals(condition.ArgumentTypes[0], condition.ArgumentTypes[2], StringComparison.Ordinal))
                .ToImmutableDictionary(
                    condition => condition.ArgumentTypes[0],
                    condition => condition,
                    StringComparer.Ordinal);
        }

        private static ImmutableDictionary<string, ImmutableArray<TriggerData.TriggerAction>> GetTriggerActions(IEnumerable<TriggerData.TriggerAction> triggerActions)
        {
            return triggerActions
                .GroupBy(action => action.ScriptName ?? action.FunctionName)
                .ToImmutableDictionary(
                    grouping => grouping.Key,
                    grouping => grouping.ToImmutableArray(),
                    StringComparer.Ordinal);
        }
    }
}