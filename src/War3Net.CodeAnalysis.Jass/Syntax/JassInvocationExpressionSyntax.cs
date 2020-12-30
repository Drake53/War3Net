// ------------------------------------------------------------------------------
// <copyright file="JassInvocationExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassInvocationExpressionSyntax : IExpressionSyntax
    {
        public JassInvocationExpressionSyntax(JassIdentifierNameSyntax identifierName, JassArgumentListSyntax arguments)
        {
            IdentifierName = identifierName;
            Arguments = arguments;
        }

        public JassInvocationExpressionSyntax(string functionName, params IExpressionSyntax[] arguments)
        {
            IdentifierName = new JassIdentifierNameSyntax(functionName);
            Arguments = new JassArgumentListSyntax(arguments);
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public JassArgumentListSyntax Arguments { get; init; }

        public bool Equals(IExpressionSyntax? other) => other is JassInvocationExpressionSyntax e && IdentifierName.Equals(e.IdentifierName) && Arguments.Equals(e.Arguments);

        public override string ToString() => $"{IdentifierName}({Arguments})";
    }
}