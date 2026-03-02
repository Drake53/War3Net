namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileReturnStatement(
            JassReturnStatementSyntax returnStatement,
            ref List<TriggerFunction> functions)
        {
            if (returnStatement.Expression is null)
            {
                functions.Add(new TriggerFunction
                {
                    Type = TriggerFunctionType.Action,
                    IsEnabled = true,
                    Name = "ReturnAction",
                });

                return true;
            }

            return false;
        }

        private bool TryDecompileReturnStatement(
            JassReturnStatementSyntax returnStatement,
            [NotNullWhen(true)] out TriggerFunction? function)
        {
            var returnExpression = returnStatement.Expression.Deparenthesize();

            return TryDecompileConditionExpression(returnExpression, out function);
        }
    }
}