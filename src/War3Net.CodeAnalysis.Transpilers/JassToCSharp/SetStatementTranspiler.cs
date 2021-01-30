// ------------------------------------------------------------------------------
// <copyright file="SetStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassSetStatementSyntax setStatement)
        {
            if (setStatement.Indexer is null)
            {
                return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(Transpile(setStatement.IdentifierName)),
                    Transpile(setStatement.Value.Expression)));
            }
            else
            {
                return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.ElementAccessExpression(
                        SyntaxFactory.IdentifierName(Transpile(setStatement.IdentifierName)),
                        SyntaxFactory.BracketedArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(Transpile(setStatement.Indexer))))),
                    Transpile(setStatement.Value.Expression)));
            }
        }
    }
}