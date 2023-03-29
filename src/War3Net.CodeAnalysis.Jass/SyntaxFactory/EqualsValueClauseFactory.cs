﻿// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassEqualsValueClauseSyntax EqualsValueClause(JassExpressionSyntax expression)
        {
            return new JassEqualsValueClauseSyntax(
                Token(JassSyntaxKind.EqualsToken),
                expression);
        }

        public static JassEqualsValueClauseSyntax EqualsValueClause(JassSyntaxToken equalsToken, JassExpressionSyntax expression)
        {
            ThrowHelper.ThrowIfInvalidToken(equalsToken, JassSyntaxKind.EqualsToken);

            return new JassEqualsValueClauseSyntax(
                equalsToken,
                expression);
        }
    }
}