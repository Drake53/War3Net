// ------------------------------------------------------------------------------
// <copyright file="ParameterListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

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
        public ParameterListSyntax Transpile(
            JassParameterListOrEmptyParameterListSyntax parameterListOrEmptyParameterList,
            JassReturnClauseSyntax returnClause,
            bool discardTakesTokenLeadingTrivia)
        {
            return parameterListOrEmptyParameterList switch
            {
                JassParameterListSyntax parameterList => Transpile(parameterList, returnClause, discardTakesTokenLeadingTrivia),
                JassEmptyParameterListSyntax emptyParameterList => Transpile(emptyParameterList, returnClause, discardTakesTokenLeadingTrivia),
            };
        }

        public ParameterListSyntax Transpile(
            JassParameterListSyntax parameterList,
            JassReturnClauseSyntax returnClause,
            bool discardTakesTokenLeadingTrivia)
        {
            var openParenToken = Transpile(
                discardTakesTokenLeadingTrivia ? JassSyntaxTriviaList.Empty : parameterList.TakesToken.LeadingTrivia,
                SyntaxKind.OpenParenToken,
                MergeTrivia(parameterList.TakesToken.TrailingTrivia, parameterList.GetLeadingTrivia()));

            var closeParenToken = Transpile(
                MergeTrivia(parameterList.GetTrailingTrivia(), returnClause.ReturnsToken.LeadingTrivia),
                SyntaxKind.CloseParenToken,
                returnClause.ReturnsToken.TrailingTrivia);

            return SyntaxFactory.ParameterList(
                openParenToken,
                SyntaxFactory.SeparatedList(
                    parameterList.ParameterList.Items.Select(Transpile),
                    parameterList.ParameterList.Separators.Select(Transpile)).WithoutTrivia(),
                closeParenToken);
        }

        public ParameterListSyntax Transpile(
            JassEmptyParameterListSyntax emptyParameterList,
            JassReturnClauseSyntax returnClause,
            bool discardTakesTokenLeadingTrivia)
        {
            var openParenToken = Transpile(
                discardTakesTokenLeadingTrivia ? JassSyntaxTriviaList.Empty : emptyParameterList.TakesToken.LeadingTrivia,
                SyntaxKind.OpenParenToken,
                MergeTrivia(emptyParameterList.TakesToken.TrailingTrivia, emptyParameterList.NothingToken.LeadingTrivia));

            var closeParenToken = Transpile(
                MergeTrivia(emptyParameterList.NothingToken.TrailingTrivia, returnClause.ReturnsToken.LeadingTrivia),
                SyntaxKind.CloseParenToken,
                returnClause.ReturnsToken.TrailingTrivia);

            return SyntaxFactory.ParameterList(
                openParenToken,
                SyntaxFactory.SeparatedList<ParameterSyntax>(),
                closeParenToken);
        }
    }
}