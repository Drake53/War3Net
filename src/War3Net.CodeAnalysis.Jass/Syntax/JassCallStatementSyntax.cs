// ------------------------------------------------------------------------------
// <copyright file="JassCallStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCallStatementSyntax : IStatementSyntax, IStatementLineSyntax, IInvocationSyntax, IJassSyntaxToken
    {
        public JassCallStatementSyntax(JassIdentifierNameSyntax identifierName, JassArgumentListSyntax arguments)
        {
            IdentifierName = identifierName;
            Arguments = arguments;
        }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public JassArgumentListSyntax Arguments { get; init; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassCallStatementSyntax callStatement
                && IdentifierName.Equals(callStatement.IdentifierName)
                && Arguments.Equals(callStatement.Arguments);
        }

        public bool Equals(IStatementLineSyntax? other)
        {
            return other is JassCallStatementSyntax callStatement
                && IdentifierName.Equals(callStatement.IdentifierName)
                && Arguments.Equals(callStatement.Arguments);
        }

        public override string ToString() => $"{JassKeyword.Call} {IdentifierName}{JassSymbol.LeftParenthesis}{Arguments}{JassSymbol.RightParenthesis}";
    }
}