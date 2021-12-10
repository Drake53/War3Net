using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileIfThenElseActionFunction(JassIfStatementSyntax ifStatement, [NotNullWhen(true)] out TriggerFunction? actionFunction)
        {
            var function = new TriggerFunction
            {
                Type = TriggerFunctionType.Action,
                IsEnabled = true,
                Name = "IfThenElseMultiple",
            };

            if (DeparenthesizeExpression(ifStatement.Condition) is JassInvocationExpressionSyntax conditionInvocationExpression &&
                Context.FunctionDeclarations.TryGetValue(conditionInvocationExpression.IdentifierName.Name, out var conditionsFunctionDeclaration) &&
                conditionsFunctionDeclaration.IsConditionsFunction)
            {
                var conditionsFunction = conditionsFunctionDeclaration.FunctionDeclaration;

                foreach (var conditionStatement in conditionsFunction.Body.Statements.SkipLast(1))
                {
                    if (TryDecompileTriggerConditionFunction(conditionStatement, true, out var conditionFunction))
                    {
                        conditionFunction.Branch = 0;
                        function.ChildFunctions.Add(conditionFunction);
                    }
                    else
                    {
                        actionFunction = null;
                        return false;
                    }
                }

                // Last statement must be "return true"
                if (conditionsFunction.Body.Statements.Last() is not JassReturnStatementSyntax finalReturnStatement ||
                    finalReturnStatement.Value is not JassBooleanLiteralExpressionSyntax returnBooleanLiteralExpression ||
                    !returnBooleanLiteralExpression.Value)
                {
                    actionFunction = null;
                    return false;
                }
            }
            else
            {
                actionFunction = null;
                return false;
            }

            if (TryDecompileTriggerActionFunctions(ifStatement.Body, out var thenActions) &&
                TryDecompileTriggerActionFunctions(ifStatement.ElseClause.Body, out var elseActions))
            {
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
            }
            else
            {
                actionFunction = null;
                return false;
            }

            actionFunction = function;
            return true;
        }
    }
}