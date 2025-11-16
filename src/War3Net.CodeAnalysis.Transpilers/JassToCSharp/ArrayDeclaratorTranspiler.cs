// ------------------------------------------------------------------------------
// <copyright file="ArrayDeclaratorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        private VariableDeclarationSyntax Transpile(
            JassArrayDeclaratorSyntax arrayDeclarator,
            bool isGlobalDeclaration)
        {
            return Transpile(
                arrayDeclarator.GetLeadingTrivia(),
                arrayDeclarator,
                arrayDeclarator.GetTrailingTrivia(),
                isGlobalDeclaration);
        }

        private VariableDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassArrayDeclaratorSyntax arrayDeclarator,
            bool isGlobalDeclaration)
        {
            return Transpile(
                leadingTrivia,
                arrayDeclarator,
                arrayDeclarator.GetTrailingTrivia(),
                isGlobalDeclaration);
        }

        private VariableDeclarationSyntax Transpile(
            JassArrayDeclaratorSyntax arrayDeclarator,
            JassSyntaxTriviaList trailingTrivia,
            bool isGlobalDeclaration)
        {
            return Transpile(
                arrayDeclarator.GetLeadingTrivia(),
                arrayDeclarator,
                trailingTrivia,
                isGlobalDeclaration);
        }

        private VariableDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassArrayDeclaratorSyntax arrayDeclarator,
            JassSyntaxTriviaList trailingTrivia,
            bool isGlobalDeclaration)
        {
            var typeNode = isGlobalDeclaration
                ? TranspileAligned(
                    leadingTrivia,
                    arrayDeclarator.Type,
                    JassSyntaxFactory.MergeTriviaLists(
                        arrayDeclarator.Type.GetTrailingTrivia(),
                        arrayDeclarator.ArrayToken.LeadingTrivia,
                        arrayDeclarator.ArrayToken.TrailingTrivia),
                    isArray: true)
                : Transpile(
                    leadingTrivia,
                    arrayDeclarator.Type,
                    MergeTrivia(
                        arrayDeclarator.Type.GetTrailingTrivia(),
                        arrayDeclarator.ArrayToken));

            var arrayTrivia = typeNode.GetTrailingTrivia();

            var arrayType = SyntaxFactory.ArrayType(
                typeNode.WithoutTrailingTrivia(),
                SyntaxFactory.SingletonList(
                    SyntaxFactory.ArrayRankSpecifier(
                        SyntaxFactory.Token(SyntaxKind.OpenBracketToken),
                        SyntaxFactory.SeparatedList<ExpressionSyntax>(),
                        SyntaxFactory.Token(
                            SyntaxTriviaList.Empty,
                            SyntaxKind.CloseBracketToken,
                            arrayTrivia))));

            return SyntaxFactory.VariableDeclaration(
                arrayType,
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.VariableDeclarator(
                        Transpile(arrayDeclarator.IdentifierName.Token, JassSyntaxTriviaList.SingleSpace),
                        null,
                        SyntaxFactory.EqualsValueClause(
                            TokenWithSpace(SyntaxKind.EqualsToken),
                            SyntaxFactory.ArrayCreationExpression(
                                TokenWithSpace(SyntaxKind.NewKeyword),
                                SyntaxFactory.ArrayType(
                                    typeNode.WithoutTrivia(),
                                    SyntaxFactory.SingletonList(
                                        SyntaxFactory.ArrayRankSpecifier(
                                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                SyntaxFactory.IdentifierName("JASS_MAX_ARRAY_SIZE"))))),
                                null)))))
                .WithTrailingTrivia(Transpile(trailingTrivia));
        }
    }
}