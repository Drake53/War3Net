// ------------------------------------------------------------------------------
// <copyright file="ForEachLoopDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileForEachLoopActionFunction(JassCallStatementSyntax callStatement, ImmutableArray<string> parameters, [NotNullWhen(true)] out TriggerFunction? actionFunction)
        {
            if (parameters.Length > 0 &&
                string.Equals(parameters[^1], JassKeyword.Code, StringComparison.Ordinal) &&
                Context.TriggerData.TriggerActions.TryGetValue(callStatement.IdentifierName.Token.Text, out var actions))
            {
                var action = actions.First(action => action.ArgumentTypes.Length == callStatement.ArgumentList.ArgumentList.Items.Length - 1);

                if (callStatement.ArgumentList.ArgumentList.Items[^1] is JassFunctionReferenceExpressionSyntax functionReferenceExpression &&
                    Context.FunctionDeclarations.TryGetValue(functionReferenceExpression.IdentifierName.Token.Text, out var actionsFunctionDeclaration) &&
                    actionsFunctionDeclaration.IsActionsFunction &&
                    TryDecompileActionStatementList(actionsFunctionDeclaration.FunctionDeclaration.Statements, out var loopActionFunctions))
                {
                    var function = new TriggerFunction
                    {
                        Type = TriggerFunctionType.Action,
                        IsEnabled = true,
                        Name = action.FunctionName,
                    };

                    for (var i = 0; i < callStatement.ArgumentList.ArgumentList.Items.Length - 1; i++)
                    {
                        if (TryDecompileTriggerFunctionParameter(callStatement.ArgumentList.ArgumentList.Items[i], action.ArgumentTypes[i], out var functionParameter))
                        {
                            function.Parameters.Add(functionParameter);
                        }
                        else
                        {
                            actionFunction = null;
                            return false;
                        }
                    }

                    foreach (var loopActionFunction in loopActionFunctions)
                    {
                        loopActionFunction.Branch = 0;
                    }

                    function.ChildFunctions.AddRange(loopActionFunctions);

                    actionFunction = function;
                    return true;
                }
            }

            actionFunction = null;
            return false;
        }
    }
}