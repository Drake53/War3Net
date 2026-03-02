namespace War3Net.Build.Extensions
{
    public static class VariableDefinitionExtensions
    {
        public static string GetVariableName(this VariableDefinition variable)
        {
            return $"udg_{variable.Name}";
        }

        public static JassExpressionSyntax GetInitialValueExpression(this VariableDefinition variable)
        {
            throw new NotImplementedException();
        }
    }
}