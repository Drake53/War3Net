// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static EqualsValueClauseSyntax EqualsValueClause(NewExpressionSyntax expression)
        {
            return new EqualsValueClauseSyntax(
                new TokenNode(new SyntaxToken(SyntaxTokenType.Assignment), 0),
                expression);
        }
    }
}