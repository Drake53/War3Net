// ------------------------------------------------------------------------------
// <copyright file="TriggerDefinitionExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class TriggerDefinitionExtensions
    {
        public static string GetVariableName(this TriggerDefinition trigger)
        {
            return $"gg_trg_{trigger.GetEscapedTriggerName()}";
        }

        public static string GetInitTrigFunctionName(this TriggerDefinition trigger)
        {
            return $"InitTrig_{trigger.GetEscapedTriggerName()}";
        }

        public static string GetTriggerConditionsFunctionName(this TriggerDefinition trigger)
        {
            return $"Trig_{trigger.GetEscapedTriggerName()}_Conditions";
        }

        public static string GetTriggerActionsFunctionName(this TriggerDefinition trigger)
        {
            return $"Trig_{trigger.GetEscapedTriggerName()}_Actions";
        }

        private static string GetEscapedTriggerName(this TriggerDefinition trigger)
        {
            return trigger.Name.Replace(' ', '_');
        }
    }
}