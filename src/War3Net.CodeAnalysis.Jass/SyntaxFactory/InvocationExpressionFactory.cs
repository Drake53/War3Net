// ------------------------------------------------------------------------------
// <copyright file="InvocationExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax InvocationExpression(string identifier)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new FunctionCallSyntax(
                        Token(SyntaxTokenType.AlphanumericIdentifier, identifier),
                        Token(SyntaxTokenType.ParenthesisOpenSymbol),
                        Empty(),
                        Token(SyntaxTokenType.ParenthesisCloseSymbol))),
                Empty());
        }

        public static NewExpressionSyntax InvocationExpression(string identifier, ArgumentListSyntax argumentList)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new FunctionCallSyntax(
                        Token(SyntaxTokenType.AlphanumericIdentifier, identifier),
                        Token(SyntaxTokenType.ParenthesisOpenSymbol),
                        argumentList,
                        Token(SyntaxTokenType.ParenthesisCloseSymbol))),
                Empty());
        }

        public static NewExpressionSyntax InvocationExpression(string identifier, NewExpressionSyntax argument)
        {
            return InvocationExpression(identifier, ArgumentList(argument));
        }

        public static NewExpressionSyntax InvocationExpression(string identifier, params NewExpressionSyntax[] arguments)
        {
            return arguments.Any() ? InvocationExpression(identifier, ArgumentList(arguments)) : InvocationExpression(identifier);
        }
    }
}