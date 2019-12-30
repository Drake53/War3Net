// ------------------------------------------------------------------------------
// <copyright file="InvocationExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax InvocationExpression(string identifier)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new FunctionCallSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, identifier), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisOpenSymbol), 0),
                        new EmptyNode(0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisCloseSymbol), 0))),
                new EmptyNode(0));
        }

        public static NewExpressionSyntax InvocationExpression(string identifier, NewExpressionSyntax argument)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new FunctionCallSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, identifier), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisOpenSymbol), 0),
                        new ArgumentListSyntax(
                            argument,
                            new ArgumentListTailSyntax(new EmptyNode(0))),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisCloseSymbol), 0))),
                new EmptyNode(0));
        }

        public static NewExpressionSyntax InvocationExpression(string identifier, ArgumentListSyntax arguments)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new FunctionCallSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, identifier), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisOpenSymbol), 0),
                        arguments,
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisCloseSymbol), 0))),
                new EmptyNode(0));
        }

        public static NewExpressionSyntax InvocationExpression(string identifier, params NewExpressionSyntax[] arguments)
        {
            if (arguments.Length == 0)
            {
                return InvocationExpression(identifier);
            }

            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new FunctionCallSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, identifier), 0),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisOpenSymbol), 0),
                        ArgumentList(arguments),
                        new TokenNode(new SyntaxToken(SyntaxTokenType.ParenthesisCloseSymbol), 0))),
                new EmptyNode(0));
        }
    }
}