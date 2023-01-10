// ------------------------------------------------------------------------------
// <copyright file="JassIdentifierNameSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIdentifierNameSyntax : JassExpressionSyntax
    {
        internal JassIdentifierNameSyntax(
            JassSyntaxToken token)
        {
            Token = token;
        }

        public JassSyntaxToken Token { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassIdentifierNameSyntax identifierName
                && Token.IsEquivalentTo(identifierName.Token);
        }

        public override void WriteTo(TextWriter writer)
        {
            Token.WriteTo(writer);
        }

        public override string ToString() => Token.ToString();

        public override JassSyntaxToken GetFirstToken() => Token;

        public override JassSyntaxToken GetLastToken() => Token;

        protected internal override JassIdentifierNameSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassIdentifierNameSyntax(newToken);
        }

        protected internal override JassIdentifierNameSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassIdentifierNameSyntax(newToken);
        }
    }
}