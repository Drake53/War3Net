// ------------------------------------------------------------------------------
// <copyright file="JassInvocationExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassInvocationExpressionSyntax : JassExpressionSyntax
    {
        internal JassInvocationExpressionSyntax(
            JassIdentifierNameSyntax identifierName,
            JassArgumentListSyntax argumentList)
        {
            IdentifierName = identifierName;
            ArgumentList = argumentList;
        }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassArgumentListSyntax ArgumentList { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassInvocationExpressionSyntax invocationExpression
                && IdentifierName.IsEquivalentTo(invocationExpression.IdentifierName)
                && ArgumentList.IsEquivalentTo(invocationExpression.ArgumentList);
        }

        public override void WriteTo(TextWriter writer)
        {
            IdentifierName.WriteTo(writer);
            ArgumentList.WriteTo(writer);
        }

        public override string ToString() => $"{IdentifierName}{ArgumentList}";

        public override JassSyntaxToken GetFirstToken() => IdentifierName.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => ArgumentList.GetLastToken();

        protected internal override JassInvocationExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassInvocationExpressionSyntax(
                IdentifierName.ReplaceFirstToken(newToken),
                ArgumentList);
        }

        protected internal override JassInvocationExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassInvocationExpressionSyntax(
                IdentifierName,
                ArgumentList.ReplaceLastToken(newToken));
        }
    }
}