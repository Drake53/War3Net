// ------------------------------------------------------------------------------
// <copyright file="CallStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewStatementSyntax CallStatement(string functionName)
        {
            return new CallStatementSyntax(
                Token(SyntaxTokenType.CallKeyword),
                Token(SyntaxTokenType.AlphanumericIdentifier, functionName),
                Token(SyntaxTokenType.ParenthesisOpenSymbol),
                Empty(),
                Token(SyntaxTokenType.ParenthesisCloseSymbol))
                .ToNewStatementSyntax();
        }

        public static NewStatementSyntax CallStatement(string functionName, ArgumentListSyntax argumentList)
        {
            return new CallStatementSyntax(
                Token(SyntaxTokenType.CallKeyword),
                Token(SyntaxTokenType.AlphanumericIdentifier, functionName),
                Token(SyntaxTokenType.ParenthesisOpenSymbol),
                argumentList,
                Token(SyntaxTokenType.ParenthesisCloseSymbol))
                .ToNewStatementSyntax();
        }

        public static NewStatementSyntax CallStatement(string functionName, NewExpressionSyntax argument)
        {
            return CallStatement(functionName, ArgumentList(argument));
        }

        public static NewStatementSyntax CallStatement(string functionName, params NewExpressionSyntax[] arguments)
        {
            return arguments.Any() ? CallStatement(functionName, ArgumentList(arguments)) : CallStatement(functionName);
        }

        private static NewStatementSyntax ToNewStatementSyntax(this CallStatementSyntax callStatement)
        {
            return new NewStatementSyntax(new StatementSyntax(callStatement), Newlines());
        }
    }
}