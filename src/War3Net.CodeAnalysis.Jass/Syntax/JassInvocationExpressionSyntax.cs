// ------------------------------------------------------------------------------
// <copyright file="JassInvocationExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassInvocationExpressionSyntax : IExpressionSyntax, IInvocationSyntax
    {
        public JassInvocationExpressionSyntax(JassIdentifierNameSyntax identifierName, JassArgumentListSyntax arguments)
        {
            IdentifierName = identifierName;
            Arguments = arguments;
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public JassArgumentListSyntax Arguments { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassInvocationExpressionSyntax invocationExpression
                && IdentifierName.Equals(invocationExpression.IdentifierName)
                && Arguments.Equals(invocationExpression.Arguments);
        }

        public override string ToString() => $"{IdentifierName}{JassSymbol.LeftParenthesis}{Arguments}{JassSymbol.RightParenthesis}";
    }
}