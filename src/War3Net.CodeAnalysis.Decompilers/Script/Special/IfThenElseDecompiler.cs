namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileIfThenElseActionFunction(JassIfStatementSyntax ifStatement, [NotNullWhen(true)] out TriggerFunction? actionFunction)
        {
            if (ifStatement.ElseIfClauses.IsEmpty &&
                ifStatement.ElseClause is not null)
            {
                var function = new TriggerFunction
                {
                    Type = TriggerFunctionType.Action,
                    IsEnabled = true,
                    Name = "IfThenElseMultiple",
                };

                if (!TryDecompileActionStatements(ifStatement.IfClause.Statements, out var thenActions) ||
                    !TryDecompileActionStatements(ifStatement.ElseClause.Statements, out var elseActions))
                {
                    actionFunction = null;
                    return false;
                }

                var conditionExpression = ifStatement.IfClause.IfClauseDeclarator.Condition.Deparenthesize();

                if (conditionExpression is JassInvocationExpressionSyntax conditionInvocationExpression &&
                    Context.FunctionDeclarations.TryGetValue(conditionInvocationExpression.IdentifierName.Token.Text, out var conditionsFunctionDeclaration) &&
                    conditionsFunctionDeclaration.IsConditionsFunction)
                {
                    var conditionsFunction = conditionsFunctionDeclaration.FunctionDeclaration;

                    if (conditionsFunction.Statements.Length == 1 &&
                        thenActions.Count == 1 &&
                        elseActions.Count == 1 &&
                        TryDecompileConditionStatement(conditionsFunction.Statements[0], true, out var conditionFunction))
                    {
                        function.Name = "IfThenElse";

                        function.Parameters.Add(new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Function,
                            Value = string.Empty,
                            Function = conditionFunction,
                        });

                        function.Parameters.Add(new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Function,
                            Value = "DoNothing",
                            Function = thenActions[0],
                        });

                        function.Parameters.Add(new TriggerFunctionParameter
                        {
                            Type = TriggerFunctionParameterType.Function,
                            Value = "DoNothing",
                            Function = elseActions[0],
                        });

                        actionFunction = function;
                        return true;
                    }
                    else
                    {
                        if (TryDecompileConditionStatements(conditionsFunction.Statements, out var conditionFunctions))
                        {
                            foreach (var condition in conditionFunctions)
                            {
                                condition.Branch = 0;
                            }

                            function.ChildFunctions.AddRange(conditionFunctions);
                        }
                        else
                        {
                            actionFunction = null;
                            return false;
                        }
                    }
                }
                else if (TryDecompileConditionExpression(conditionExpression, out var conditionFunction))
                {
                    conditionFunction.Branch = 0;
                    function.ChildFunctions.Add(conditionFunction);
                }
                else
                {
                    actionFunction = null;
                    return false;
                }

                foreach (var action in thenActions)
                {
                    action.Branch = 1;
                }

                foreach (var action in elseActions)
                {
                    action.Branch = 2;
                }

                function.ChildFunctions.AddRange(thenActions);
                function.ChildFunctions.AddRange(elseActions);

                actionFunction = function;
                return true;
            }
            else
            {
                actionFunction = null;
                return false;
            }
        }
    }
}