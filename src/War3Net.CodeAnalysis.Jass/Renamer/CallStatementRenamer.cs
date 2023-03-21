// ------------------------------------------------------------------------------
// <copyright file="CallStatementRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameCallStatement(JassCallStatementSyntax callStatement, [NotNullWhen(true)] out JassStatementSyntax? renamedCallStatement)
        {
            if (RenameExecuteFuncArgument &&
                string.Equals(callStatement.IdentifierName.Token.Text, "ExecuteFunc", StringComparison.Ordinal))
            {
                if (callStatement.ArgumentList.ArgumentList.Items.Length == 1 &&
                    callStatement.ArgumentList.ArgumentList.Items[0] is JassLiteralExpressionSyntax stringLiteralExpression &&
                    stringLiteralExpression.Token.SyntaxKind == JassSyntaxKind.StringLiteralToken &&
                    TryRenameFunctionIdentifierToken(stringLiteralExpression.Token, out var renamedToken))
                {
                    renamedCallStatement = new JassCallStatementSyntax(
                        callStatement.CallToken,
                        callStatement.IdentifierName,
                        new JassArgumentListSyntax(
                            callStatement.ArgumentList.OpenParenToken,
                            SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken>.Create(new JassLiteralExpressionSyntax(renamedToken)),
                            callStatement.ArgumentList.CloseParenToken));

                    return true;
                }

                renamedCallStatement = null;
                return false;
            }

            if (TryRenameFunctionIdentifierName(callStatement.IdentifierName, out var renamedIdentifierName) |
                TryRenameArgumentList(callStatement.ArgumentList, out var renamedArguments))
            {
                renamedCallStatement = new JassCallStatementSyntax(
                    callStatement.CallToken,
                    renamedIdentifierName ?? callStatement.IdentifierName,
                    renamedArguments ?? callStatement.ArgumentList);

                return true;
            }

            renamedCallStatement = null;
            return false;
        }
    }
}