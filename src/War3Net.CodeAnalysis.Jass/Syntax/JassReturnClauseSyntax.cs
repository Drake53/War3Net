// ------------------------------------------------------------------------------
// <copyright file="JassReturnClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassReturnClauseSyntax : JassSyntaxNode
    {
        internal JassReturnClauseSyntax(
            JassSyntaxToken returnsToken,
            JassTypeSyntax returnType)
        {
            ReturnsToken = returnsToken;
            ReturnType = returnType;
        }

        public JassSyntaxToken ReturnsToken { get; }

        public JassTypeSyntax ReturnType { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassReturnClauseSyntax returnClause
                && ReturnType.IsEquivalentTo(returnClause.ReturnType);
        }

        public override void WriteTo(TextWriter writer)
        {
            ReturnsToken.WriteTo(writer);
            ReturnType.WriteTo(writer);
        }

        public override string ToString() => $"{ReturnsToken} {ReturnType}";

        public override JassSyntaxToken GetFirstToken() => ReturnsToken;

        public override JassSyntaxToken GetLastToken() => ReturnType.GetLastToken();

        protected internal override JassReturnClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassReturnClauseSyntax(
                newToken,
                ReturnType);
        }

        protected internal override JassReturnClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassReturnClauseSyntax(
                ReturnsToken,
                ReturnType.ReplaceLastToken(newToken));
        }
    }
}