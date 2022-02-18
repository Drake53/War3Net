// ------------------------------------------------------------------------------
// <copyright file="IfStatementDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileIfStatement(
            JassIfStatementSyntax ifStatement,
            ref List<TriggerFunction> functions)
        {
            if (TryDecompileIfThenElseActionFunction(ifStatement, out var function))
            {
                functions.Add(function);
                return true;
            }

            functions.Add(DecompileCustomScriptAction(new JassIfCustomScriptAction(ifStatement.Condition)));

            if (TryDecompileActionStatementList(ifStatement.Body, out var thenActions))
            {
                functions.AddRange(thenActions);
            }
            else
            {
                return false;
            }

            foreach (var elseIfClause in ifStatement.ElseIfClauses)
            {
                functions.Add(DecompileCustomScriptAction(new JassElseIfCustomScriptAction(elseIfClause.Condition)));

                if (TryDecompileActionStatementList(elseIfClause.Body, out var elseIfActions))
                {
                    functions.AddRange(elseIfActions);
                }
                else
                {
                    return false;
                }
            }

            if (ifStatement.ElseClause is not null)
            {
                functions.Add(DecompileCustomScriptAction(JassElseCustomScriptAction.Value));

                if (TryDecompileActionStatementList(ifStatement.ElseClause.Body, out var elseActions))
                {
                    functions.AddRange(elseActions);
                }
                else
                {
                    return false;
                }
            }

            functions.Add(DecompileCustomScriptAction(JassEndIfCustomScriptAction.Value));

            return true;
        }

        /// <param name="returnValue"><see langword="true"/> for AND conditions, <see langword="false"/> for OR conditions.</param>
        private bool TryDecompileIfStatement(
            JassIfStatementSyntax ifStatement,
            bool returnValue,
            [NotNullWhen(true)] out TriggerFunction? function)
        {
            if (ifStatement.ElseIfClauses.IsEmpty &&
                ifStatement.ElseClause is null &&
                ifStatement.Body.Statements.Length == 1 &&
                ifStatement.Body.Statements[0] is JassReturnStatementSyntax returnStatement &&
                returnStatement.Value is JassBooleanLiteralExpressionSyntax booleanLiteralExpression &&
                booleanLiteralExpression.Value != returnValue)
            {
                var conditionExpression = ifStatement.Condition.Deparenthesize();

                if (returnValue)
                {
                    if (conditionExpression is JassUnaryExpressionSyntax unaryExpression &&
                        unaryExpression.Operator == UnaryOperatorType.Not)
                    {
                        conditionExpression = unaryExpression.Expression.Deparenthesize();
                    }
                    else
                    {
                        function = null;
                        return false;
                    }
                }

                return TryDecompileConditionExpression(conditionExpression, out function);
            }

            function = null;
            return false;
        }
    }
}