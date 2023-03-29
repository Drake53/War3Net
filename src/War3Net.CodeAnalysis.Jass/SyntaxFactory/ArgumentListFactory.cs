// ------------------------------------------------------------------------------
// <copyright file="ArgumentListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassArgumentListSyntax ArgumentList(params JassExpressionSyntax[] arguments)
        {
            return new JassArgumentListSyntax(
                Token(JassSyntaxKind.OpenParenToken),
                SeparatedSyntaxList(JassSyntaxKind.CommaToken, arguments),
                Token(JassSyntaxKind.CloseParenToken));
        }

        public static JassArgumentListSyntax ArgumentList(IEnumerable<JassExpressionSyntax> arguments)
        {
            return new JassArgumentListSyntax(
                Token(JassSyntaxKind.OpenParenToken),
                SeparatedSyntaxList(JassSyntaxKind.CommaToken, arguments),
                Token(JassSyntaxKind.CloseParenToken));
        }

        public static JassArgumentListSyntax ArgumentList(JassSyntaxToken openParenToken, SeparatedSyntaxList<JassExpressionSyntax, JassSyntaxToken> argumentList, JassSyntaxToken closeParenToken)
        {
            ThrowHelper.ThrowIfInvalidToken(openParenToken, JassSyntaxKind.OpenParenToken);
            ThrowHelper.ThrowIfInvalidSeparatedSyntaxList(argumentList, JassSyntaxKind.CommaToken);
            ThrowHelper.ThrowIfInvalidToken(closeParenToken, JassSyntaxKind.CloseParenToken);

            return new JassArgumentListSyntax(
                openParenToken,
                argumentList,
                closeParenToken);
        }
    }
}