// ------------------------------------------------------------------------------
// <copyright file="TriggerDefinitionExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text;
using System.Text.RegularExpressions;

using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class TriggerDefinitionExtensions
    {
        public static string GetVariableName(this TriggerDefinition trigger)
        {
            return $"gg_trg_{trigger.GetTriggerIdentifierName()}";
        }

        public static string GetInitTrigFunctionName(this TriggerDefinition trigger)
        {
            return $"InitTrig_{trigger.GetTriggerIdentifierName()}";
        }

        public static string GetTrigConditionsFunctionName(this TriggerDefinition trigger)
        {
            return $"Trig_{trigger.GetTriggerIdentifierName()}_Conditions";
        }

        public static string GetTrigActionsFunctionName(this TriggerDefinition trigger)
        {
            return $"Trig_{trigger.GetTriggerIdentifierName()}_Actions";
        }

        public static string GetTrigIdentifierBaseName(this TriggerDefinition trigger)
        {
            return $"Trig_{trigger.GetTriggerIdentifierName()}_";
        }

        public static string GetTriggerIdentifierName(this TriggerDefinition trigger)
        {
            return Regex.Replace(trigger.Name, "[^A-Za-z0-9_]", match => new string('_', Encoding.UTF8.GetBytes(match.Value).Length));
        }
    }
}