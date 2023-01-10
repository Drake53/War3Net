// ------------------------------------------------------------------------------
// <copyright file="JassParameterSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassParameterSyntax : JassSyntaxNode
    {
        internal JassParameterSyntax(
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName)
        {
            Type = type;
            IdentifierName = identifierName;
        }

        public JassTypeSyntax Type { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassParameterSyntax parameter
                && Type.IsEquivalentTo(parameter.Type)
                && IdentifierName.IsEquivalentTo(parameter.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override string ToString() => $"{Type} {IdentifierName}";

        public override JassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override JassParameterSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassParameterSyntax(
                Type.ReplaceFirstToken(newToken),
                IdentifierName);
        }

        protected internal override JassParameterSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassParameterSyntax(
                Type,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}