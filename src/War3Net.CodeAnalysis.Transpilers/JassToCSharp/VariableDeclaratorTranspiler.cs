// ------------------------------------------------------------------------------
// <copyright file="VariableDeclaratorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public VariableDeclarationSyntax Transpile(
            JassVariableDeclaratorSyntax variableDeclarator,
            bool isGlobalDeclaration)
        {
            return Transpile(
                variableDeclarator.GetLeadingTrivia(),
                variableDeclarator,
                variableDeclarator.GetTrailingTrivia(),
                isGlobalDeclaration);
        }

        private VariableDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassVariableDeclaratorSyntax variableDeclarator,
            bool isGlobalDeclaration)
        {
            return Transpile(
                leadingTrivia,
                variableDeclarator,
                variableDeclarator.GetTrailingTrivia(),
                isGlobalDeclaration);
        }

        private VariableDeclarationSyntax Transpile(
            JassVariableDeclaratorSyntax variableDeclarator,
            JassSyntaxTriviaList trailingTrivia,
            bool isGlobalDeclaration)
        {
            return Transpile(
                variableDeclarator.GetLeadingTrivia(),
                variableDeclarator,
                trailingTrivia,
                isGlobalDeclaration);
        }

        private VariableDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassVariableDeclaratorSyntax variableDeclarator,
            JassSyntaxTriviaList trailingTrivia,
            bool isGlobalDeclaration)
        {
            VariableDeclaratorSyntax declarator;

            if (variableDeclarator.Value is null)
            {
                declarator = SyntaxFactory.VariableDeclarator(
                    Transpile(variableDeclarator.IdentifierName.Token).WithSpace(),
                    null,
                    SyntaxFactory.EqualsValueClause(
                        TokenWithSpace(SyntaxKind.EqualsToken),
                        SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression)));
            }
            else
            {
                declarator = SyntaxFactory.VariableDeclarator(
                    Transpile(variableDeclarator.IdentifierName.Token),
                    null,
                    Transpile(variableDeclarator.Value));
            }

            var typeNode = isGlobalDeclaration
                ? TranspileAligned(leadingTrivia, variableDeclarator.Type, isArray: false)
                : Transpile(leadingTrivia, variableDeclarator.Type);

            return SyntaxFactory.VariableDeclaration(
                typeNode,
                SyntaxFactory.SingletonSeparatedList(
                    declarator.WithTrailingTrivia(Transpile(trailingTrivia))));
        }
    }
}