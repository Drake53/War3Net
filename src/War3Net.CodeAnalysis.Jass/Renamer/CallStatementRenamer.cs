// ------------------------------------------------------------------------------
// <copyright file="CallStatementRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameCallStatement(JassCallStatementSyntax callStatement, [NotNullWhen(true)] out IStatementSyntax? renamedCallStatement)
        {
            if (RenameExecuteFuncArgument &&
                string.Equals(callStatement.IdentifierName.Name, "ExecuteFunc", StringComparison.Ordinal))
            {
                if (callStatement.Arguments.Arguments.Length == 1 &&
                    callStatement.Arguments.Arguments[0] is JassStringLiteralExpressionSyntax stringLiteralExpression &&
                    _functionDeclarationRenames.TryGetValue(stringLiteralExpression.Value, out var renamedValue))
                {
                    renamedCallStatement = new JassCallStatementSyntax(
                        callStatement.IdentifierName,
                        new JassArgumentListSyntax(new IExpressionSyntax[] { new JassStringLiteralExpressionSyntax(renamedValue.Name) }.ToImmutableArray()));

                    return true;
                }

                renamedCallStatement = null;
                return false;
            }

            if (TryRenameFunctionIdentifierName(callStatement.IdentifierName, out var renamedIdentifierName) |
                TryRenameArgumentList(callStatement.Arguments, out var renamedArguments))
            {
                renamedCallStatement = new JassCallStatementSyntax(
                    renamedIdentifierName ?? callStatement.IdentifierName,
                    renamedArguments ?? callStatement.Arguments);

                return true;
            }

            renamedCallStatement = null;
            return false;
        }
    }
}