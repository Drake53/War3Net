﻿// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassReturnStatementSyntax ReturnStatement(JassExpressionSyntax expression)
        {
            return new JassReturnStatementSyntax(
                Token(JassSyntaxKind.ReturnKeyword),
                expression);
        }

        public static JassReturnStatementSyntax ReturnStatement(JassSyntaxToken returnToken, JassExpressionSyntax expression)
        {
            ThrowHelper.ThrowIfInvalidToken(returnToken, JassSyntaxKind.ReturnKeyword);

            return new JassReturnStatementSyntax(
                returnToken,
                expression);
        }
    }
}