// ------------------------------------------------------------------------------
// <copyright file="RunInitializationTriggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateRunInitializationTriggers(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapTriggers = map.Triggers;
            if (mapTriggers is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.RunInitializationTriggers}' cannot be generated without {nameof(MapTriggers)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.RunInitializationTriggers);

            foreach (var trigger in mapTriggers.TriggerItems)
            {
                if (trigger is TriggerDefinition triggerDefinition &&
                    ShouldGenerateRunInitializationTriggersForTrigger(map, triggerDefinition))
                {
                    writer.WriteCall(
                        NativeName.ConditionalTriggerExecute,
                        triggerDefinition.GetVariableName());
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateRunInitializationTriggers(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Triggers is not null
                && map.Triggers.TriggerItems.Any(trigger =>
                       trigger is TriggerDefinition triggerDefinition &&
                       ShouldGenerateRunInitializationTriggersForTrigger(map, triggerDefinition));
        }

        protected internal virtual bool ShouldGenerateRunInitializationTriggersForTrigger(Map map, TriggerDefinition triggerDefinition)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            return triggerDefinition.IsEnabled
                && triggerDefinition.IsInitiallyOn
                && triggerDefinition.Functions.Any(function =>
                       function.Type == TriggerFunctionType.Event &&
                       function.IsEnabled &&
                       string.Equals(function.Name, TriggerFunctionName.MapInitializationEvent, StringComparison.Ordinal));
        }
    }
}