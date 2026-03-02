namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private TriggerFunction DecompileCustomScriptAction(string customScriptCode)
        {
            return new TriggerFunction
            {
                Type = TriggerFunctionType.Action,
                IsEnabled = true,
                Name = "CustomScriptCode",
                Parameters = new()
                {
                    new TriggerFunctionParameter
                    {
                        Type = TriggerFunctionParameterType.String,
                        Value = customScriptCode,
                    },
                },
            };
        }
    }
}