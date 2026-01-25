// ------------------------------------------------------------------------------
// <copyright file="InitCustomTriggers.cs" company="Drake53">
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
        protected internal virtual void GenerateInitCustomTriggers(Map map, IndentedTextWriter writer)
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
                throw new ArgumentException($"Function '{GeneratedFunctionName.InitCustomTriggers}' cannot be generated without {nameof(MapTriggers)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.InitCustomTriggers);

            foreach (var trigger in mapTriggers.TriggerItems)
            {
                if (trigger is TriggerDefinition triggerDefinition &&
                    triggerDefinition.IsEnabled)
                {
                    writer.WriteCall(triggerDefinition.GetInitTrigFunctionName());
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitCustomTriggers(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Triggers is not null
                && map.Triggers.TriggerItems.Any(trigger => trigger is TriggerDefinition triggerDefinition && triggerDefinition.IsEnabled);
        }
    }
}