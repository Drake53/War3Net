namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateUserDefinedVariables(Map map, IndentedTextWriter writer)
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

            foreach (var variable in mapTriggers.Variables)
            {
                var variableType = TriggerData.TriggerTypes.TryGetValue(variable.Type, out var triggerType) && !string.IsNullOrEmpty(triggerType.BaseType)
                    ? triggerType.BaseType
                    : variable.Type;

                if (variable.IsArray)
                {
                    writer.WriteAlignedGlobal(
                        $"{variableType} {JassKeyword.Array}",
                        variable.GetVariableName());
                }
                else if (string.Equals(variable.Type, JassKeyword.String, StringComparison.Ordinal))
                {
                    writer.WriteAlignedGlobal(
                        JassKeyword.String,
                        variable.GetVariableName());
                }
                else
                {
                    var defaultValue = variable.Type switch
                    {
                        JassKeyword.Integer => "0",
                        JassKeyword.Real => "0",
                        JassKeyword.Boolean => JassKeyword.False,

                        _ => JassKeyword.Null,
                    };

                    writer.WriteAlignedGlobal(
                        variableType,
                        variable.GetVariableName(),
                        defaultValue);
                }
            }
        }

        protected internal virtual bool ShouldGenerateUserDefinedVariables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Triggers?.Variables is not null
                && map.Triggers.Variables.Count > 0;
        }
    }
}