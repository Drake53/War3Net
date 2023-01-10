// ------------------------------------------------------------------------------
// <copyright file="JassArrayDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassArrayDeclaratorSyntax : JassVariableOrArrayDeclaratorSyntax
    {
        internal JassArrayDeclaratorSyntax(
            JassTypeSyntax type,
            JassSyntaxToken arrayToken,
            JassIdentifierNameSyntax identifierName)
        {
            Type = type;
            ArrayToken = arrayToken;
            IdentifierName = identifierName;
        }

        public JassTypeSyntax Type { get; }

        public JassSyntaxToken ArrayToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassArrayDeclaratorSyntax arrayDeclarator
                && Type.IsEquivalentTo(arrayDeclarator.Type)
                && IdentifierName.IsEquivalentTo(arrayDeclarator.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            ArrayToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override string ToString() => $"{Type} {ArrayToken} {IdentifierName}";

        public override JassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override JassArrayDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassArrayDeclaratorSyntax(
                Type.ReplaceFirstToken(newToken),
                ArrayToken,
                IdentifierName);
        }

        protected internal override JassArrayDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassArrayDeclaratorSyntax(
                Type,
                ArrayToken,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}