// ------------------------------------------------------------------------------
// <copyright file="Triggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateTriggerVariables(Map map, IndentedTextWriter writer)
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
                return;
            }

            foreach (var triggerItem in mapTriggers.TriggerItems.Where(ShouldGenerateTriggerVariable))
            {
                writer.WriteAlignedGlobal(
                    TypeName.Trigger,
                    triggerItem.GetVariableName(),
                    JassKeyword.Null);
            }
        }

        protected internal virtual bool ShouldGenerateTriggerVariables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Triggers is not null
                && map.Triggers.TriggerItems.Any(ShouldGenerateTriggerVariable);
        }

        protected internal virtual bool ShouldGenerateTriggerVariable(TriggerItem triggerItem)
        {
            if (triggerItem is null)
            {
                throw new ArgumentNullException(nameof(triggerItem));
            }

            return triggerItem is TriggerDefinition triggerDefinition
                && !triggerDefinition.IsComment;
        }
    }
}