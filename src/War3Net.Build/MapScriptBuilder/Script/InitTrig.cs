// ------------------------------------------------------------------------------
// <copyright file="InitTrig.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateInitTrig(Map map, TriggerDefinition triggerDefinition, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var triggerVariableName = triggerDefinition.GetVariableName();

            writer.WriteFunction(triggerDefinition.GetInitTrigFunctionName());

            writer.WriteSet(
                triggerVariableName,
                JassExpression.InvokeSpaced(NativeName.CreateTrigger));

            if (!triggerDefinition.IsInitiallyOn)
            {
                writer.WriteCall(
                    NativeName.DisableTrigger,
                    triggerVariableName);
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldRenderTrigger(Map map, TriggerDefinition triggerDefinition)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            return triggerDefinition.IsEnabled;
        }
    }
}