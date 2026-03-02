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

            functions.Add(DecompileCustomScriptAction(ifStatement.IfClause.IfClauseDeclarator.ToString()));

            if (TryDecompileActionStatements(ifStatement.IfClause.Statements, out var thenActions))
            {
                functions.AddRange(thenActions);
            }
            else
            {
                return false;
            }

            foreach (var elseIfClause in ifStatement.ElseIfClauses)
            {
                DecompileLeadingTrivia(elseIfClause.ElseIfClauseDeclarator.ElseIfToken.LeadingTrivia, ref functions);
                functions.Add(DecompileCustomScriptAction(elseIfClause.ElseIfClauseDeclarator.ToString()));

                if (TryDecompileActionStatements(elseIfClause.Statements, out var elseIfActions))
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
                DecompileLeadingTrivia(ifStatement.ElseClause.ElseToken.LeadingTrivia, ref functions);
                functions.Add(DecompileCustomScriptAction(ifStatement.ElseClause.ElseToken.ToString()));

                if (TryDecompileActionStatements(ifStatement.ElseClause.Statements, out var elseActions))
                {
                    functions.AddRange(elseActions);
                }
                else
                {
                    return false;
                }
            }

            DecompileLeadingTrivia(ifStatement.EndIfToken.LeadingTrivia, ref functions);
            functions.Add(DecompileCustomScriptAction(ifStatement.EndIfToken.ToString()));

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
                ifStatement.IfClause.Statements.Length == 1 &&
                ifStatement.IfClause.Statements[0] is JassReturnStatementSyntax returnStatement &&
                returnStatement.Expression is not null &&
                returnStatement.Expression.TryGetBooleanExpressionValue(out var returnStatementValue) &&
                returnStatementValue != returnValue)
            {
                var conditionExpression = ifStatement.IfClause.IfClauseDeclarator.Condition.Deparenthesize();

                if (returnValue)
                {
                    if (conditionExpression is JassUnaryExpressionSyntax unaryExpression &&
                        unaryExpression.SyntaxKind == JassSyntaxKind.LogicalNotExpression)
                    {
                        conditionExpression = unaryExpression.Operand.Deparenthesize();
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