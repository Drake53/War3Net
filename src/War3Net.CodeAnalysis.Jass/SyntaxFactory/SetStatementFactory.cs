// ------------------------------------------------------------------------------
// <copyright file="SetStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewStatementSyntax SetStatement(string variableName, EqualsValueClauseSyntax equalsValueClause)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new SetStatementSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.SetKeyword), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, variableName), 0),
                        new EmptyNode(0),
                        equalsValueClause)),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }

        public static NewStatementSyntax SetStatement(string variableName, NewExpressionSyntax arrayIndex, EqualsValueClauseSyntax equalsValueClause)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new SetStatementSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.SetKeyword), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, variableName), 0),
                        new BracketedExpressionSyntax(
                            new TokenNode(new SyntaxToken(SyntaxTokenType.SquareBracketOpenSymbol), 0),
                            arrayIndex,
                            new TokenNode(new SyntaxToken(SyntaxTokenType.SquareBracketCloseSymbol), 0)),
                        equalsValueClause)),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }
    }
}