namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileCallStatement(
            JassCallStatementSyntax callStatement,
            ref List<TriggerFunction> functions)
        {
            if (!Context.TriggerData.TriggerActions.TryGetValue(callStatement.IdentifierName.Token.Text, out var actions))
            {
                return false;
            }

            var action = actions.First(action => action.ArgumentTypes.Length == callStatement.ArgumentList.Arguments.Items.Length);

            if (TryDecompileForEachLoopActionFunction(callStatement, action.ArgumentTypes, out var loopActionFunction))
            {
                functions.Add(loopActionFunction);
                return true;
            }

            var function = new TriggerFunction
            {
                Type = TriggerFunctionType.Action,
                IsEnabled = true,
                Name = action.FunctionName,
            };

            for (var j = 0; j < callStatement.ArgumentList.Arguments.Items.Length; j++)
            {
                if (TryDecompileTriggerFunctionParameter(callStatement.ArgumentList.Arguments.Items[j], action.ArgumentTypes[j], out var functionParameter))
                {
                    function.Parameters.Add(functionParameter);
                }
                else
                {
                    return false;
                }
            }

            functions.Add(function);
            return true;
        }
    }
}