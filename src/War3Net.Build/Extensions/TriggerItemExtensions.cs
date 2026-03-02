using System.Text;
using System.Text.RegularExpressions;
using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class TriggerItemExtensions
    {
        public static string GetVariableName(this TriggerItem trigger)
        {
            return $"gg_trg_{trigger.GetTriggerIdentifierName()}";
        }

        public static string GetInitTrigFunctionName(this TriggerItem trigger)
        {
            return $"InitTrig_{trigger.GetTriggerIdentifierName()}";
        }

        public static string GetTrigConditionsFunctionName(this TriggerItem trigger)
        {
            return $"Trig_{trigger.GetTriggerIdentifierName()}_Conditions";
        }

        public static string GetTrigActionsFunctionName(this TriggerItem trigger)
        {
            return $"Trig_{trigger.GetTriggerIdentifierName()}_Actions";
        }

        public static string GetTrigIdentifierBaseName(this TriggerItem trigger)
        {
            return $"Trig_{trigger.GetTriggerIdentifierName()}_";
        }

        public static string GetTriggerIdentifierName(this TriggerItem trigger)
        {
            return Regex.Replace(trigger.Name, "[^A-Za-z0-9_]", match => new string('_', Encoding.UTF8.GetBytes(match.Value).Length));
        }
    }
}