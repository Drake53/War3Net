namespace War3Net.Build
{
    public partial class TriggerRenderer
    {
        private void RenderSetVariable(TriggerFunction function, TriggerRendererContext context)
        {
            var variableParameter = function.Parameters[0];
            var valueParameter = function.Parameters[1];

            if (!_variableTypes.TryGetValue(variableParameter.Value, out var type))
            {
                throw new InvalidOperationException($"Unable to determine the type of the global variable '{variableParameter.Value}'.");
            }

            var variableName = $"udg_{variableParameter.Value}";
            if (variableParameter.ArrayIndexer is not null)
            {
                variableName = JassExpression.ElementAccess(
                    variableName,
                    GetParameter(variableParameter.ArrayIndexer, "integer", 0, context.TrigFunctionIdentifierBuilder));
            }

            context.Writer.WriteSet(
                variableName,
                GetParameter(valueParameter, type, 1, context.TrigFunctionIdentifierBuilder));
        }
    }
}