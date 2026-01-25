// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassFunctionDeclarationSyntax FunctionDeclaration(JassFunctionDeclaratorSyntax functionDeclarator, params JassStatementSyntax[] statements)
        {
            return new JassFunctionDeclarationSyntax(
                functionDeclarator,
                statements.ToImmutableArray(),
                Token(JassSyntaxKind.EndFunctionKeyword));
        }

        public static JassFunctionDeclarationSyntax FunctionDeclaration(JassFunctionDeclaratorSyntax functionDeclarator, IEnumerable<JassStatementSyntax> statements)
        {
            return new JassFunctionDeclarationSyntax(
                functionDeclarator,
                statements.ToImmutableArray(),
                Token(JassSyntaxKind.EndFunctionKeyword));
        }

        public static JassFunctionDeclarationSyntax FunctionDeclaration(JassFunctionDeclaratorSyntax functionDeclarator, ImmutableArray<JassStatementSyntax> statements)
        {
            return new JassFunctionDeclarationSyntax(
                functionDeclarator,
                statements,
                Token(JassSyntaxKind.EndFunctionKeyword));
        }

        public static JassFunctionDeclarationSyntax FunctionDeclaration(JassFunctionDeclaratorSyntax functionDeclarator, ImmutableArray<JassStatementSyntax> statements, JassSyntaxToken endFunctionToken)
        {
            ThrowHelper.ThrowIfInvalidToken(endFunctionToken, JassSyntaxKind.EndFunctionKeyword);

            return new JassFunctionDeclarationSyntax(
                functionDeclarator,
                statements,
                endFunctionToken);
        }
    }
}