// ------------------------------------------------------------------------------
// <copyright file="CallStatementFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewStatementSyntax CallStatement(string functionName)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new CallStatementSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.CallKeyword), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, functionName), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisOpenSymbol), 0),
                        new EmptyNode(0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisCloseSymbol), 0))),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }

        public static NewStatementSyntax CallStatement(string functionName, NewExpressionSyntax argument)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new CallStatementSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.CallKeyword), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, functionName), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisOpenSymbol), 0),
                        new ArgumentListSyntax(
                            argument,
                            new ArgumentListTailSyntax(new EmptyNode(0))),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisCloseSymbol), 0))),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }

        public static NewStatementSyntax CallStatement(string functionName, ArgumentListSyntax arguments)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new CallStatementSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.CallKeyword), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, functionName), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisOpenSymbol), 0),
                        arguments,
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisCloseSymbol), 0))),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }

        public static NewStatementSyntax CallStatement(string functionName, params NewExpressionSyntax[] arguments)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new CallStatementSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.CallKeyword), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, functionName), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisOpenSymbol), 0),
                        ArgumentList(arguments),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisCloseSymbol), 0))),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }
    }
}